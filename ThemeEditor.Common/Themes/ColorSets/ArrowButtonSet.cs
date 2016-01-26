// --------------------------------------------------
// ThemeEditor.Common - ArrowButtonSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class ArrowButtonSet
    {
        public ColorArgb8888 _;
        public ColorRgb888 Glow;
        public ColorRgb888 Main;
        public ColorRgb888 Shading;

        public static ArrowButtonSet Read(BinaryReader br)
        {
            return new ArrowButtonSet
            {
                Shading = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Glow = ColorRgb888.Read(br),
                _ = ColorArgb8888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Shading.Write(bw);
            Main.Write(bw);
            Glow.Write(bw);
            _.Write(bw);
        }
    }
}
