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

namespace ThemeEditor.WPF
{
    /// <summary>
    ///     Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : INotifyPropertyChanged
    {
        private string _updateMessage;
        public string AppCompany => GetAttribute<AssemblyCompanyAttribute>(Assembly.GetEntryAssembly()).Company;
        public string AppCopyright => GetAttribute<AssemblyCopyrightAttribute>(Assembly.GetEntryAssembly()).Copyright;
        public string AppDescription => GetAttribute<AssemblyDescriptionAttribute>(Assembly.GetEntryAssembly()).Description;

        public string AppInformation
            => GetAttribute<AssemblyInformationalVersionAttribute>(Assembly.GetEntryAssembly()).InformationalVersion;

        public string AppName => Assembly.GetEntryAssembly().GetName().Name;
        public string AppTitle => GetAttribute<AssemblyTitleAttribute>(Assembly.GetEntryAssembly()).Title;
        public Version AppVersion => Assembly.GetEntryAssembly().GetName().Version;

        private Version _onlineVersion;

        public Version OnlineVersion
        {
            get { return _onlineVersion; }
            set
            {
                if (value == _onlineVersion)
                    return;
                _onlineVersion = value;
                OnPropertyChanged(nameof(OnlineVersion));
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

        public AboutWindow()
        {
            InitializeComponent();
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
            OnlineVersion = Update.OnlineVersion;
            UpdateMessage = hasUpdate
                                ? "Update Available"
                                : "You are Updated";
        }

        private void HandleLinkClick(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hl = (Hyperlink) sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
