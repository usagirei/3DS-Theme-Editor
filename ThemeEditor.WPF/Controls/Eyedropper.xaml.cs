using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for Eyedropper.xaml
    /// </summary>
    public partial class Eyedropper : Button
    {
        public static readonly DependencyProperty ColorProperty
            = DependencyProperty.Register(nameof(Color),
                typeof(Color),
                typeof(Eyedropper),
                new PropertyMetadata(default(Color)));

        private bool _mDown;

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public Eyedropper()
        {
            InitializeComponent();
        }

        private void Eyedropper_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mDown = true;
            Mouse.Capture(sender as UIElement);
            Stream cursorStream = PickerCursor;
            Mouse.OverrideCursor = new Cursor(cursorStream);
        }

        private static Stream PickerCursor => (Stream) Extensions.GetResources(@"Picker\.cur").First().Value;

        private void Eyedropper_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_mDown)
                return;

            var elem = sender as UIElement;
            var mPos = e.GetPosition(elem);
            Point sPos = elem.PointToScreen(mPos);
            Debug.WriteLine(sPos.ToString());
            Bitmap cap = Extensions.ScrCap(new Rectangle((int) sPos.X, (int) sPos.Y, 1, 1));
            System.Drawing.Color capCol = cap.GetPixel(0, 0);
            Color = Color.FromArgb(capCol.A, capCol.R, capCol.G, capCol.B);
        }

        private void Eyedropper_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_mDown)
                return;
            _mDown = false;
            Mouse.Capture(null);
            Mouse.OverrideCursor = null;
        }
    }
}
