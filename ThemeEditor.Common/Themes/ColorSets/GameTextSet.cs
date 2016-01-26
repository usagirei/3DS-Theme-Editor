// --------------------------------------------------
// ThemeEditor.Common - GameTextSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class GameTextSet
    {
        public ColorArgb8888 _;
        public ColorRgb888 __;
        public ColorRgb888 Main;
        public ColorRgb888 Text;

        public static GameTextSet Read(BinaryReader br)
        {
            return new GameTextSet
            {
                Main = ColorRgb888.Read(br),
                _ = ColorArgb8888.Read(br),
                __ = ColorRgb888.Read(br),
                Text = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            Main.Write(bw);
            _.Write(bw);
            __.Write(bw);
            Text.Write(bw);
        }
    }
}
