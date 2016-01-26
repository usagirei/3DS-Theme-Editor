// --------------------------------------------------
// ThemeEditor.Common - BottomBackgroundInnerSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class BottomBackgroundInnerSet
    {
        public ColorRgb888 Border;
        public ColorRgb888 Highlight;
        public ColorRgb888 Main;
        public ColorArgb8888 Shadow; // Shadow Related ? Maybe Opacity / Color

        public static BottomBackgroundInnerSet Read(BinaryReader br)
        {
            return new BottomBackgroundInnerSet
            {
                Highlight = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Border = ColorRgb888.Read(br),
                Shadow = ColorArgb8888.Read(br)
            };
        }

        public void Write(BinaryWriter bw)
        {
            Highlight.Write(bw);
            Main.Write(bw);
            Border.Write(bw);
            Shadow.Write(bw);
        }
    }
}
