// --------------------------------------------------
// ThemeEditor.Common - GameTextSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class GameTextSet
    {
        /*
        * Data Order:
        * 
        * Main
        * Light 
        * Shadow (Alpha)
        * TextMain
        */

        public ColorArgb8888 Shadow;
        public ColorRgb888 Light;
        public ColorRgb888 Main;
        public ColorRgb888 TextMain;

        public static GameTextSet Read(BinaryReader br)
        {
            return new GameTextSet
            {
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorArgb8888.Read(br),
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
