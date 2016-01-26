// --------------------------------------------------
// ThemeEditor.Common - DemoTextSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class DemoTextSet
    {
        public ColorRgb888 Main;
        public ColorRgb888 Text;

        public static DemoTextSet Read(BinaryReader br)
        {
            return new DemoTextSet
            {
                Main = ColorRgb888.Read(br),
                Text = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Main.Write(bw);
            Text.Write(bw);
        }
    }
}
