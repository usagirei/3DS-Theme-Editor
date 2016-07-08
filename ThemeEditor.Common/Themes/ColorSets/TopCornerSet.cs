// --------------------------------------------------
// ThemeEditor.Common - TopCornerSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class TopCornerSet
    {
        /*
        * Data Order:
        * 
        * TextShadow Pos (float)
        * 
        * Main 
        * Light 
        * Shadow
        * TextMain
        */

        public ColorRgb888 Light;
        public ColorRgb888 Shadow;
        public ColorRgb888 Main;
        public ColorRgb888 TextMain;

        public static TopCornerSet Read(BinaryReader br)
        {
            return new TopCornerSet
            {
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorRgb888.Read(br),
                TextMain = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Main.Write(bw);
            Light.Write(bw);
            Shadow.Write(bw);
            TextMain.Write(bw);
        }
    }
}
