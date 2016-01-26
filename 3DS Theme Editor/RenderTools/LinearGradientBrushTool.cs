// --------------------------------------------------
// 3DS Theme Editor - LinearGradientBrushTool.cs
// --------------------------------------------------

using System.Windows.Media;

namespace ThemeEditor.WPF.RenderTools
{
    internal struct LinearGradientBrushTool
    {
        public readonly Color ColorA;
        public readonly Color ColorB;
        public readonly double Angle;
        public readonly double Opacity;

        public LinearGradientBrushTool(Color ca, Color cb, double a, double o = 1) : this()
        {
            ColorA = ca;
            ColorB = cb;
            Angle = a;
            Opacity = o;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is LinearGradientBrushTool && Equals((LinearGradientBrushTool) obj);
        }

        public bool Equals(LinearGradientBrushTool other)
        {
            return ColorA.Equals(other.ColorA) && ColorB.Equals(other.ColorB) && Angle.Equals(other.Angle)
                   && Opacity.Equals(other.Opacity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ColorA.GetHashCode();
                hashCode = (hashCode * 397) ^ ColorB.GetHashCode();
                hashCode = (hashCode * 397) ^ Angle.GetHashCode();
                hashCode = (hashCode * 397) ^ Opacity.GetHashCode();
                return hashCode;
            }
        }
    }
}
