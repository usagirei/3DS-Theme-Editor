// --------------------------------------------------
// 3DS Theme Editor - TopScreenPreview.xaml.cs
// --------------------------------------------------

using System.Windows;
using System.Windows.Controls;

using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for TopScreenPreview.xaml
    /// </summary>
    public partial class TopScreenPreview : UserControl
    {
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register(nameof(Theme),
                typeof(ThemeViewModel),
                typeof(TopScreenPreview),
                new PropertyMetadata(default(ThemeViewModel)));

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }


        public TopScreenPreview()
        {
            InitializeComponent();
        }
    }
}
