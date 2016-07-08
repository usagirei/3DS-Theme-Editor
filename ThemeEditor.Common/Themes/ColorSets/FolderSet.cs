// --------------------------------------------------
// ThemeEditor.Common - FolderSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class FolderSet
    {
        /*
        * Data Order:
        * 
        * Dark
        * Main
        * Light (Unused?)
        * Shadow (Unused?)
        */

        public ColorRgb888 Light;
        public ColorRgb888 Shadow;
        public ColorRgb888 Main;
        public ColorRgb888 Dark;

        public static FolderSet Read(BinaryReader br)
        {
            return new FolderSet
            {
                Dark = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Light = ColorRgb888.Read(br),
                Shadow = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Dark.Write(bw);
            Main.Write(bw);
            Light.Write(bw);
            Shadow.Write(bw);
        }
    };
}
