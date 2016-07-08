// --------------------------------------------------
// ThemeEditor.Common - DemoTextSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class DemoTextSet
    {
        /*
        * Data Order:
        * 
        * Main 
        * TextMain
        */

        public ColorRgb888 Main;
        public ColorRgb888 TextMain;

        public static DemoTextSet Read(BinaryReader br)
        {
            return new DemoTextSet
            {
                Main = ColorRgb888.Read(br),
                TextMain = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Main.Write(bw);
            TextMain.Write(bw);
        }
    }
}
