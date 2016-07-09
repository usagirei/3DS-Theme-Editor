// --------------------------------------------------
// ThemeEditor.Common - TopSolidSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class TopBackgroundSet
    {
        public ColorRgb888 Main;
        public byte Gradient;
        public byte TextureOpacity;
        public byte AlternateOpacity;
        public byte GradientColor;

        private static byte Inflate(byte v)
        {
            return (byte) (v * 255 / 100).Clamp(0, 255);
        }

        private static byte Deflate(byte v)
        {
            return (byte)(v * 100 / 255).Clamp(0, 100);
        }

        public static TopBackgroundSet Read(BinaryReader br, bool typeTwo)
        {
            var tbs = new TopBackgroundSet
            {
                Main = ColorRgb888.Read(br),
                Gradient = br.ReadByte(),
                TextureOpacity = br.ReadByte(),
                AlternateOpacity = typeTwo ? br.ReadByte() : (byte)0,
                GradientColor = typeTwo ? br.ReadByte() : (byte)0,
            };

            tbs.Main.B = Inflate(tbs.Main.B);
            tbs.Main.G = Inflate(tbs.Main.G);
            tbs.Main.R = Inflate(tbs.Main.R);
            tbs.Gradient = Inflate(tbs.Gradient);
            tbs.TextureOpacity = Inflate(tbs.TextureOpacity);
            tbs.AlternateOpacity = Inflate(tbs.AlternateOpacity);
            tbs.GradientColor = Inflate(tbs.GradientColor);

            return tbs;
        }

        public void Write(BinaryWriter bw, bool typeTwo)
        {
            ColorRgb888 temp = Main;
            temp.R = Deflate(temp.R);
            temp.G = Deflate(temp.G);
            temp.B = Deflate(temp.B);
            temp.Write(bw);

            bw.Write(Deflate(Gradient));
            bw.Write(Deflate(TextureOpacity));
            if (!typeTwo)
                return;
            
            bw.Write(Deflate(AlternateOpacity));
            bw.Write(Deflate(GradientColor));
        }
    }
}
