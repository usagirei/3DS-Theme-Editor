// --------------------------------------------------
// CM3D2ModTool - AboutWindow.xaml.cs
// --------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF
{
    /// <summary>
    ///     Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : INotifyPropertyChanged
    {
        private Version _onlineVersion;
        private long _updateDownloadedBytes;
        private string _updateMessage;

        private long _updateTotalBytes;
        public string AppCompany => GetAttribute<AssemblyCompanyAttribute>(Assembly.GetEntryAssembly()).Company;
        public string AppCopyright => GetAttribute<AssemblyCopyrightAttribute>(Assembly.GetEntryAssembly()).Copyright;
        public string AppDescription => GetAttribute<AssemblyDescriptionAttribute>(Assembly.GetEntryAssembly()).Description;

        public string AppInformation
            => GetAttribute<AssemblyInformationalVersionAttribute>(Assembly.GetEntryAssembly()).InformationalVersion;

        public string AppName => Assembly.GetEntryAssembly().GetName().Name;
        public string AppTitle => GetAttribute<AssemblyTitleAttribute>(Assembly.GetEntryAssembly()).Title;
        public Version AppVersion => Assembly.GetEntryAssembly().GetName().Version;

        public Version OnlineVersion
        {
            get { return _onlineVersion; }
            set
            {
                if (value == _onlineVersion)
                    return;
                _onlineVersion = value;
                OnPropertyChanged(nameof(OnlineVersion));
                OnPropertyChanged(nameof(UpdateAvailable));
            }
        }

#if DEBUG
        public bool UpdateAvailable => true;
#else
        public bool UpdateAvailable => OnlineVersion > AppVersion;
#endif

        public ICommand UpdateDownloadCommand { get; }

        public long UpdateDownloadedBytes
        {
            get { return _updateDownloadedBytes; }
            set
            {
                if (value == _updateDownloadedBytes)
                    return;
                _updateDownloadedBytes = value;
                OnPropertyChanged(nameof(UpdateDownloadedBytes));
            }
        }

        public string UpdateMessage
        {
            get { return _updateMessage; }
            set
            {
                if (value == _updateMessage)
                    return;
                _updateMessage = value;
                OnPropertyChanged(nameof(UpdateMessage));
            }
        }

        public long UpdateTotalBytes
        {
            get { return _updateTotalBytes; }
            set
            {
                if (value == _updateTotalBytes)
                    return;
                _updateTotalBytes = value;
                OnPropertyChanged(nameof(UpdateTotalBytes));
            }
        }

        public AboutWindow()
        {
            InitializeComponent();
            UpdateDownloadCommand = new RelayCommandAsync<TempFile>(DownloadUpdate_Execute,
                () => UpdateAvailable && !string.IsNullOrEmpty(Update.UpdatePayloadUrl),
                null,
                PostExecute);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider assembly, bool inherit = false)
            where T : Attribute
        {
            return assembly
                .GetCustomAttributes(typeof(T), inherit)
                .OfType<T>()
                .FirstOrDefault();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AboutWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckLatestVersion();
        }

        private void AboutWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private async void CheckLatestVersion()
        {
            bool hasUpdate = await Update.CheckUpdateAvailable();
            OnlineVersion = Update.UpdateVersion;
            UpdateMessage = hasUpdate
                                ? "Update Available"
                                : "You are Updated";
        }

        private async Task<TempFile> DownloadUpdate_Execute()
        {
            return await Update.DownloadUpdatePayload((curr, total) =>
            {
                UpdateTotalBytes = total;
                UpdateDownloadedBytes = curr;
            });
        }

        private void HandleLinkClick(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hl = (Hyperlink) sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void PostExecute(TempFile tempFile)
        {
            if (tempFile == null)
                return;

            if (!Update.ApplyUpdatePayload(tempFile))
            {
                MessageBox.Show(
                                "An Error Ocurred During The Update Process.\n" +
                                "If The Application Fails to Function, Restore the Backup Present under 'bak_v" +
                                AppVersion +
                                "' And Try Again.");
                return;
            }

            // Restart
            var app = (App) Application.Current;
            var startupExe = Application.ResourceAssembly.Location;
            var args = app.LaunchArgs;
            var argsStr = args.Any() ? string.Join(" ", args.Select(x => $"\"{x}\"")) : "";

            // Save Settings Before Launching new instance
            Settings.Default.Save();
            Process.Start(startupExe, argsStr);
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
