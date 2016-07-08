// --------------------------------------------------
// ThemeEditor.Common - BottomCornerSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class BottomCorner
    {
        /*
        * Data Order:
        * 
        * :Base
        * Dark 
        * Main 
        * Light 
        * Shadow
        * 
        * :Icon
        * Main
        * Light
        * TextMain
        */

        public ColorRgb888 BaseShadow;
        public ColorRgb888 IconTextMain;
        public ColorRgb888 BaseDark;
        public ColorRgb888 BaseLight;
        public ColorRgb888 IconMain;
        public ColorRgb888 IconLight;
        public ColorRgb888 BaseMain;

        public static BottomCorner Read(BinaryReader br)
        {
            return new BottomCorner
            {
                BaseDark = ColorRgb888.Read(br),
                BaseMain = ColorRgb888.Read(br),
                BaseLight = ColorRgb888.Read(br),
                BaseShadow = ColorRgb888.Read(br),

                IconMain = ColorRgb888.Read(br),
                IconLight = ColorRgb888.Read(br),
                IconTextMain = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            BaseDark.Write(bw);
            BaseMain.Write(bw);
            BaseLight.Write(bw);
            BaseShadow.Write(bw);

            IconMain.Write(bw);
            IconLight.Write(bw);
            IconTextMain.Write(bw);
        }
    }
}
