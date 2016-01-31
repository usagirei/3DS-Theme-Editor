// --------------------------------------------------
// ThemeEditor.WPF - CrashWindow.xaml.cs
// --------------------------------------------------

using System;
using System.Windows;


namespace ThemeEditor.WPF
{
    /// <summary>
    ///     Interaction logic for CrashWindow.xaml
    /// </summary>
    internal partial class CrashWindow : Window
    {
        public CrashWindow()
        {
            InitializeComponent();
        }

        public CrashWindow(Exception ex)
        {
            InitializeComponent();
            ExceptionTextBox.Text = ex.ToString();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
