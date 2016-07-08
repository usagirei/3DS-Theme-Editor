// --------------------------------------------------
// ThemeEditor.Common - CursorSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class CursorSet
    {

        /*
         * Data Order:
         * 
         * Dark
         * Main
         * Light (Unused?)
         * Glow
         */

        public ColorRgb888 Light;

        public ColorRgb888 Glow;
        public ColorRgb888 Main;
        public ColorRgb888 Dark;

        public static CursorSet Read(BinaryReader br)
        {
            return new CursorSet
            {
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Glow = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Dark.Write(bw);
            Main.Write(bw);
            Light.Write(bw);
            Glow.Write(bw);
        }
    }
}
