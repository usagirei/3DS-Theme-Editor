// --------------------------------------------------
// ThemeEditor.Common - TopSolidSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class TopBackgroundSet
    {
        public byte _;
        public byte __;
        public byte Gradient;
        public ColorRgb888 Main;
        public byte TextureOpacity;

        public static TopBackgroundSet Read(BinaryReader br, bool typeTwo)
        {
            return new TopBackgroundSet
            {
                Main = ColorRgb888.Read(br),
                Gradient = br.ReadByte(),
                TextureOpacity = br.ReadByte(),
                _ = typeTwo ? br.ReadByte() : (byte) 0,
                __ = typeTwo ? br.ReadByte() : (byte) 0,
            };
        }

        public void Write(BinaryWriter bw, bool typeTwo)
        {
            Main.Write(bw);
            bw.Write(Gradient);
            bw.Write(TextureOpacity);
            if (!typeTwo)
                return;
            bw.Write(_);
            bw.Write(__);
        }
    }
}
