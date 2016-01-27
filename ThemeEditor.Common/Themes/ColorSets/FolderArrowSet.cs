// --------------------------------------------------
// ThemeEditor.Common - FolderArrowSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class FolderArrowSet
    {
        public ColorArgb8888 _;
        public ColorRgb888 __;
        public ColorRgb888 ___;
        public ColorRgb888 ArrowGlow;
        public ColorRgb888 ArrowPressed;
        public ColorRgb888 ArrowUnpressed;
        public ColorArgb8888 Glow;
        public ColorRgb888 Highlight;
        public ColorRgb888 Main;
        public ColorRgb888 Shading;

        public static FolderArrowSet Read(BinaryReader br)
        {
            return new FolderArrowSet
            {
                _ = ColorArgb8888.Read(br),
                Shading = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Highlight = ColorRgb888.Read(br),
                Glow = ColorArgb8888.Read(br),
                __ = ColorRgb888.Read(br),
                ArrowGlow = ColorRgb888.Read(br),
                ArrowUnpressed = ColorRgb888.Read(br),
                ArrowPressed = ColorRgb888.Read(br),
                ___ = ColorRgb888.Read(br)
            };
        }

        public void Write(BinaryWriter bw)
        {
            _.Write(bw);
            Shading.Write(bw);
            Main.Write(bw);
            Highlight.Write(bw);
            Glow.Write(bw);
            __.Write(bw);
            ArrowGlow.Write(bw);
            ArrowUnpressed.Write(bw);
            ArrowPressed.Write(bw);
            ___.Write(bw);
        }
    }
}
