// --------------------------------------------------
// ThemeEditor.Common - BottomBackgroundOuterSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class BottomBackgroundOuterSet
    {
        /*
         * Data Order:
         * 
         * Dark
         * Main
         * Light 
         */

        public ColorRgb888 Light;
        public ColorRgb888 Main;
        public ColorRgb888 Dark;

        public static BottomBackgroundOuterSet Read(BinaryReader br)
        {
            return new BottomBackgroundOuterSet
            {
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br)
            };
        }

        public void Write(BinaryWriter bw)
        {
            Dark.Write(bw);
            Main.Write(bw);
            Light.Write(bw);
        }
    }
}
