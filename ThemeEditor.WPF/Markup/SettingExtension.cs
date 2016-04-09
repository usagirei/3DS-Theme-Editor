using System.Windows.Data;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF.Markup
{
    class SettingExtension : Binding
    {
        public SettingExtension()
        {
            SetBinding();
        }

        public SettingExtension(string path) : base(path)
        {
            SetBinding();
        }


        public void SetBinding()
        {
            Source = Settings.Default;
            Mode = BindingMode.TwoWay;
        }
    }
}
