// --------------------------------------------------
// ThemeEditor.Common - FolderArrowSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class FolderArrowSet
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

        public ColorRgb888 ___;

        public float ArrowShadowPos;
        public ColorRgb888 Glow;
        public ColorRgb888 ArrowShadow;
        public ColorRgb888 ArrowSelected;
        public ColorRgb888 ArrowMain;
        public ColorArgb8888 Shadow;
        public ColorRgb888 Light;
        public ColorRgb888 Main;
        public ColorRgb888 Dark;

        public static FolderArrowSet Read(BinaryReader br)
        {
            return new FolderArrowSet
            {
                ArrowShadowPos = br.ReadSingle(),
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorArgb8888.Read(br),
                Glow = ColorRgb888.Read(br),
                ArrowShadow = ColorRgb888.Read(br),
                ArrowMain = ColorRgb888.Read(br),
                ArrowSelected = ColorRgb888.Read(br),
                ___ = ColorRgb888.Read(br) // Padding
            };
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(ArrowShadowPos);
            Dark.Write(bw);
            Main.Write(bw);
            Light.Write(bw);
            Shadow.Write(bw);
            Glow.Write(bw);
            ArrowShadow.Write(bw);
            ArrowMain.Write(bw);
            ArrowSelected.Write(bw);
            ___.Write(bw);
        }
    }
}
