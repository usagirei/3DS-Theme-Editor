using System;
using System.Windows;
using System.Windows.Controls;

using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for BottomOverlaysPreview.xaml
    /// </summary>
    public partial class BottomOverlaysPreview : UserControl
    {
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register(nameof(Theme),
                typeof(ThemeViewModel),
                typeof(BottomOverlaysPreview),
                new PropertyMetadata(default(ThemeViewModel)));

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static BottomOverlaysPreview()
        {
            Type ownerType = typeof(BottomOverlaysPreview);
            IsEnabledProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false));
        }

        public BottomOverlaysPreview()
        {
            InitializeComponent();
        }
    }
}
