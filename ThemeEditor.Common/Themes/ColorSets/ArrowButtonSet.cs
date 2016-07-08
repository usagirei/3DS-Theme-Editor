// --------------------------------------------------
// ThemeEditor.Common - ArrowButtonSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class ArrowButtonSet
    {
        /*
        * Data Order:
        * 
        * Dark
        * Main
        * Light 
        * Shadow (Unused?)
        */

        public ColorArgb8888 Shadow;
        public ColorRgb888 Light;
        public ColorRgb888 Main;
        public ColorRgb888 Dark;

        public static ArrowButtonSet Read(BinaryReader br)
        {
            return new ArrowButtonSet
            {
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorArgb8888.Read(br),
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
