using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThemeEditor.WPF.Controls.Renderers;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for TopFolderPreview.xaml
    /// </summary>
    public partial class TopFolderPreview : UserControl
    {
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register(nameof(Theme),
                typeof(ThemeViewModel),
                typeof(TopFolderPreview),
                new PropertyMetadata(default(ThemeViewModel)));

        public static readonly DependencyProperty FolderRotationProperty = DependencyProperty.Register(
            nameof(FolderRotation),
            typeof(double),
            typeof(TopFolderPreview),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty FolderBounceProperty = DependencyProperty.Register(
            nameof(FolderBounce),
            typeof(double),
            typeof(TopFolderPreview),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ShowPreviewProperty = DependencyProperty.Register(
            nameof(ShowPreview),
            typeof(bool),
            typeof(TopFolderPreview),
            new PropertyMetadata(default(bool)));

        private bool _isListening;

        public double FolderBounce
        {
            get { return (double)GetValue(FolderBounceProperty); }
            set { SetValue(FolderBounceProperty, value); }
        }

        public double FolderRotation
        {
            get { return (double)GetValue(FolderRotationProperty); }
            set { SetValue(FolderRotationProperty, value); }
        }

        public bool ShowPreview
        {
            get { return (bool)GetValue(ShowPreviewProperty); }
            set { SetValue(ShowPreviewProperty, value); }
        }

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static TopFolderPreview()
        {
            Type ownerType = typeof(TopFolderPreview);
            IsEnabledProperty
                .OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false, OnIsEnabledChanged));
        }

        public TopFolderPreview()
        {
            InitializeComponent();
        }

        private void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                if (!_isListening)
                {
                    StartListening();
                }
            }
            else
            {
                if (_isListening)
                {
                    StopListening();
                }
            }

            if (oldValue != newValue)
            {
                InvalidateVisual();
            }
        }

        private void OnRendering(object sender, EventArgs eventArgs)
        {
            FolderRotation = CompositionTargetEx.SecondsFromStart * -36;
            FolderBounce = (Math.Sin(CompositionTargetEx.SecondsFromStart) + 1) / 2;
        }

        private void StartListening()
        {
            VerifyAccess();
            if (_isListening)
                return;
            _isListening = true;
            CompositionTargetEx.FrameUpdating += OnRendering;
        }

        private void StopListening()
        {
            VerifyAccess();
            if (!_isListening)
                return;
            _isListening = false;
            CompositionTargetEx.FrameUpdating -= OnRendering;
            FolderBounce = 0;
            FolderRotation = 0;
        }

        private static void OnIsEnabledChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var target = elem as TopFolderPreview;
            if (target == null)
            {
                return;
            }
            bool oldValue = (bool)args.OldValue;
            bool newValue = (bool)args.NewValue;
            target.OnIsEnabledChanged(oldValue, newValue);
        }
    }
}