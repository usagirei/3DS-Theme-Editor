// --------------------------------------------------
// 3DS Theme Editor - BottomScreenPreview.xaml.cs
// --------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for BottomScreenPreview.xaml
    /// </summary>
    public partial class BottomScreenPreview : UserControl
    {
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register(nameof(Theme),
                typeof(ThemeViewModel),
                typeof(BottomScreenPreview),
                new PropertyMetadata(default(ThemeViewModel)));

        private bool _isListening = false;

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static BottomScreenPreview()
        {
            Type ownerType = typeof(BottomScreenPreview);
            IsEnabledProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false));
        }

        public BottomScreenPreview()
        {
            InitializeComponent();
        }
    }
}
