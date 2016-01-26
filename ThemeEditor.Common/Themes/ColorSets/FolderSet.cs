// --------------------------------------------------
// ThemeEditor.Common - FolderSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class FolderSet
    {
        public ColorRgb888 _;
        public ColorRgb888 __;
        public ColorRgb888 Main;
        public ColorRgb888 Shading;

        public static FolderSet Read(BinaryReader br)
        {
            return new FolderSet
            {
                Shading = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                _ = ColorRgb888.Read(br),
                __ = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Shading.Write(bw);
            Main.Write(bw);
            _.Write(bw);
            __.Write(bw);
        }
    };
}
