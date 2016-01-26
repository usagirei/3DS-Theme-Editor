// --------------------------------------------------
// 3DS Theme Editor - SolidColorBrushTool.cs
// --------------------------------------------------

using System.Windows.Media;

namespace ThemeEditor.WPF.RenderTools
{
    internal struct SolidColorBrushTool
    {
        public readonly Color Color;
        public readonly double Opacity;

        public SolidColorBrushTool(Color c, double o = 1) : this()
        {
            Color = c;
            Opacity = o;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(SolidColorBrushTool other)
        {
            return Color.Equals(other.Color) && Opacity.Equals(other.Opacity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode() * 397) ^ Opacity.GetHashCode();
            }
        }
    }
}
