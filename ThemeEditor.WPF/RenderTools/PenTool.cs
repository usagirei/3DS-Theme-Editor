// --------------------------------------------------
// 3DS Theme Editor - PenTool.cs
// --------------------------------------------------

using System.Windows.Media;

namespace ThemeEditor.WPF.RenderTools
{
    internal struct PenTool
    {
        public readonly Color Color;
        public readonly double Width;
        public readonly double Opacity;

        public PenTool(Color c, double w, double o = 1) : this()
        {
            Color = c;
            Width = w;
            Opacity = o;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(PenTool other)
        {
            return Color.Equals(other.Color) && Width.Equals(other.Width) && Opacity.Equals(other.Opacity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Opacity.GetHashCode();
                return hashCode;
            }
        }
    }
}
