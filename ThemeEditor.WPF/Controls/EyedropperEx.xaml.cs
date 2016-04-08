using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for Eyedropper.xaml
    /// </summary>
    public partial class EyedropperEx : Button
    {
        public static readonly DependencyProperty CaptureSizeProperty = DependencyProperty.Register(
            nameof(CaptureSize),
            typeof (Size),
            typeof (EyedropperEx),
            new PropertyMetadata(new Size(1, 1)));

        public static readonly DependencyProperty BitmapProperty = DependencyProperty.Register(
            nameof(Bitmap),
            typeof (BitmapSource),
            typeof (EyedropperEx),
            new PropertyMetadata(default(Bitmap)));

        public static readonly DependencyProperty CaptureCenterProperty = DependencyProperty.Register(
            nameof(CaptureCenter),
            typeof (Point),
            typeof (EyedropperEx),
            new PropertyMetadata(default(Point)));

        public static readonly DependencyProperty BitmapFinalProperty = DependencyProperty.Register(
            "BitmapFinal",
            typeof (BitmapSource),
            typeof (EyedropperEx),
            new FrameworkPropertyMetadata(default(BitmapSource),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PropertyChangedCallback));

        private bool _mDown;

        public BitmapSource BitmapFinal
        {
            get { return (BitmapSource) GetValue(BitmapFinalProperty); }
            set { SetValue(BitmapFinalProperty, value); }
        }

        public Size CaptureSize
        {
            get { return (Size) GetValue(CaptureSizeProperty); }
            set { SetValue(CaptureSizeProperty, value); }
        }

        public BitmapSource Bitmap
        {
            get { return (BitmapSource) GetValue(BitmapProperty); }
            set { SetValue(BitmapProperty, value); }
        }

        public Point CaptureCenter
        {
            get { return (Point) GetValue(CaptureCenterProperty); }
            set { SetValue(CaptureCenterProperty, value); }
        }

        private static Stream PickerCursor => (Stream) Extensions.GetResources(@"WidePicker\.cur").First().Value;

        public EyedropperEx()
        {
            InitializeComponent();
        }

        private static void PropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var self = s as EyedropperEx;
            if (self == null)
                return;
            switch (e.Property.Name)
            {
                case nameof(BitmapFinal):
                {
                    self.Bitmap = (BitmapSource) e.NewValue;
                    break;
                }
            }
        }

        private void Eyedropper_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mDown = true;
            Mouse.Capture(sender as UIElement);
            Stream cursorStream = PickerCursor;
            Mouse.OverrideCursor = new Cursor(cursorStream);
        }

        private void Eyedropper_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_mDown)
                return;

            var elem = sender as UIElement;
            var mPos = e.GetPosition(elem);
            if (elem == null)
                return;

            var sPos = elem.PointToScreen(mPos);
            var rectangle = new Rectangle((int) (sPos.X - CaptureCenter.X),
                (int) (sPos.Y - CaptureCenter.Y),
                (int) CaptureSize.Width,
                (int) CaptureSize.Height);

            var cap = Extensions.ScrCapW(rectangle);
            Bitmap = cap;
        }

        private void Eyedropper_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_mDown)
                return;
            _mDown = false;
            BitmapFinal = Bitmap;
            Mouse.Capture(null);
            Mouse.OverrideCursor = null;
        }
    }
}