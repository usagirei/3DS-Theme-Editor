// --------------------------------------------------
// ThemeEditor.Common - BottomCornerSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class BottomCorner
    {
        public ColorRgb888 _;
        public ColorRgb888 __;
        public ColorRgb888 Border;
        public ColorRgb888 Highlight;
        public ColorRgb888 IconBottom;
        public ColorRgb888 IconTop;
        public ColorRgb888 Main;

        public static BottomCorner Read(BinaryReader br)
        {
            return new BottomCorner
            {
                Border = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Highlight = ColorRgb888.Read(br),
                _ = ColorRgb888.Read(br),
                IconBottom = ColorRgb888.Read(br),
                IconTop = ColorRgb888.Read(br),
                __ = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Border.Write(bw);
            Main.Write(bw);
            Highlight.Write(bw);
            __.Write(bw);
            IconTop.Write(bw);
            IconBottom.Write(bw);
            _.Write(bw);
        }
    }
}
