// --------------------------------------------------
// ThemeEditor.Common - BottomBackgroundInnerSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class BottomBackgroundInnerSet
    {
        /*
         * Data Order:
         * 
         * Dark
         * Main
         * Light 
         * Shadow (Alpha)
         */

        public ColorRgb888 Light;
        public ColorRgb888 Dark;
        public ColorRgb888 Main;
        public ColorArgb8888 Shadow;

        public static BottomBackgroundInnerSet Read(BinaryReader br)
        {
            return new BottomBackgroundInnerSet
            {
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorArgb8888.Read(br)
            };
        }

        public void Write(BinaryWriter bw)
        {
            Dark.Write(bw);
            Main.Write(bw);
            Light.Write(bw);
            Shadow.Write(bw);
        }
    }
}
