// --------------------------------------------------
// ThemeEditor.Common - OpenCloseSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    /*
    * Data Order:
    * 
    * TextShadow Pos (float)
    * 
    * Dark 
    * Main 
    * Light 
    * Shadow (Alpha)
    * 
    * Glow
    * TextShadow
    * TextMain
    * TextSelected
    */

    public class OpenCloseSet
    {
        public ColorRgb888 ____;

        public float TextShadowPos;
        public ColorRgb888 Glow;
        public ColorArgb8888 Shadow;
        public ColorRgb888 Light;
        public ColorRgb888 Dark;
        public ColorRgb888 TextShadow;
        public ColorRgb888 TextSelected;
        public ColorRgb888 TextMain;
        public ColorRgb888 Main;

        public static OpenCloseSet Read(BinaryReader br)
        {
            return new OpenCloseSet
            {
                TextShadowPos = br.ReadSingle(),
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorArgb8888.Read(br),
                Glow = ColorRgb888.Read(br),
                TextShadow = ColorRgb888.Read(br),
                TextMain = ColorRgb888.Read(br),
                TextSelected = ColorRgb888.Read(br),
                ____ = ColorRgb888.Read(br), // Padding to 16
            };
        }

        public void Write(BinaryWriter bw)
        {
            //TextShadowPos.Write(bw);
            bw.Write(TextShadowPos);
            Dark.Write(bw);
            Main.Write(bw);
            Light.Write(bw);
            Shadow.Write(bw);
            Glow.Write(bw);
            TextShadow.Write(bw);
            TextMain.Write(bw);
            TextSelected.Write(bw);
            ____.Write(bw);
        }
    }
}
