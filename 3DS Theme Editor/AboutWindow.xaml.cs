// --------------------------------------------------
// CM3D2ModTool - AboutWindow.xaml.cs
// --------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private Version _onlineVersion;
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
            }
        }

        public string UpdateUrl => @"https://github.com/usagirei/3DS-Theme-Editor/releases/latest";

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
            OnlineVersion = await Task<Version>.Factory.StartNew(GetLatestVersion);
        }

        private Version GetLatestVersion()
        {
            var req = WebRequest.CreateHttp(UpdateUrl);
            req.Method = "HEAD";
            req.AllowAutoRedirect = true;


            var myResp = (HttpWebResponse) req.GetResponse();
            if (myResp.StatusCode == HttpStatusCode.OK)
            {
                var latestReleaseUrl = myResp.ResponseUri.ToString().TrimEnd('/');
                var lastSlash = latestReleaseUrl.LastIndexOf('/');
                var versionStr = latestReleaseUrl.Substring(lastSlash + 1);
                versionStr = versionStr.StartsWith("v") ? versionStr.Substring(1) : versionStr;
                Version version;
                if (Version.TryParse(versionStr, out version))
                    return version;
            }
            return null;
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
