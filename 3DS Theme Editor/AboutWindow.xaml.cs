// --------------------------------------------------
// CM3D2ModTool - AboutWindow.xaml.cs
// --------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace ThemeEditor.WPF
{
    /// <summary>
    ///     Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        public string AppDescription => GetAttribute<AssemblyDescriptionAttribute>(Assembly.GetEntryAssembly()).Description;
        public string AppCompany => GetAttribute<AssemblyCompanyAttribute>(Assembly.GetEntryAssembly()).Company;
        public string AppCopyright => GetAttribute<AssemblyCopyrightAttribute>(Assembly.GetEntryAssembly()).Copyright;
        public string AppTitle => GetAttribute<AssemblyTitleAttribute>(Assembly.GetEntryAssembly()).Title;
        public string AppInformation => GetAttribute<AssemblyInformationalVersionAttribute>(Assembly.GetEntryAssembly()).InformationalVersion;
        public string AppName => Assembly.GetEntryAssembly().GetName().Name;
        public string AppVersion => Assembly.GetEntryAssembly().GetName().Version.ToString();

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

        private void AboutWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
