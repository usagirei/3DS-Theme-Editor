// --------------------------------------------------
// 3DS Theme Editor - ImageBrushTool.cs
// --------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace ThemeEditor.WPF.RenderTools
{
    internal struct ImageBrushTool
    {
        public readonly ImageSource Image;
        public readonly TileMode Mode;
        public readonly Rect Viewport;
        public readonly BrushMappingMode ViewportUnits;
        public readonly double Opacity;

        public ImageBrushTool(ImageSource i, TileMode m, Rect v, BrushMappingMode u, double o = 1) : this()
        {
            Image = i;
            Mode = m;
            Viewport = v;
            ViewportUnits = u;
            Opacity = o;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ImageBrushTool other)
        {
            return Equals(Image, other.Image)
                   && Mode == other.Mode
                   && Viewport.Equals(other.Viewport)
                   && ViewportUnits == other.ViewportUnits
                   && Opacity.Equals(other.Opacity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Image?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (int) Mode;
                hashCode = (hashCode * 397) ^ Viewport.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) ViewportUnits;
                hashCode = (hashCode * 397) ^ Opacity.GetHashCode();
                return hashCode;
            }
        }
    }
}
