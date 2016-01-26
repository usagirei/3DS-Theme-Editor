// --------------------------------------------------
// ThemeEditor.Common - CursorSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class CursorSet
    {
        public ColorRgb888 _;
        public ColorRgb888 Glow;
        public ColorRgb888 Main;
        public ColorRgb888 Shading;

        public static CursorSet Read(BinaryReader br)
        {
            return new CursorSet
            {
                Shading = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                _ = ColorRgb888.Read(br),
                Glow = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Shading.Write(bw);
            Main.Write(bw);
            _.Write(bw);
            Glow.Write(bw);
        }
    }
}
