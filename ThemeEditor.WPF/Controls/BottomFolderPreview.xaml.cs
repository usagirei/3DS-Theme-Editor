using System;
using System.Windows;
using System.Windows.Controls;

using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for BottomFolderPreview.xaml
    /// </summary>
    public partial class BottomFolderPreview : UserControl
    {
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register(nameof(Theme),
                typeof(ThemeViewModel),
                typeof(BottomFolderPreview),
                new PropertyMetadata(default(ThemeViewModel)));

        public static readonly DependencyProperty ShowFolderProperty = DependencyProperty.Register(
                                                        nameof(ShowFolder),
            typeof(bool),
            typeof(BottomFolderPreview),
            new PropertyMetadata(default(bool)));

        public bool ShowFolder
        {
            get { return (bool) GetValue(ShowFolderProperty); }
            set { SetValue(ShowFolderProperty, value); }
        }

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static BottomFolderPreview()
        {
            Type ownerType = typeof(BottomFolderPreview);
            IsEnabledProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false));
        }

        public BottomFolderPreview()
        {
            InitializeComponent();
        }
    }
}
