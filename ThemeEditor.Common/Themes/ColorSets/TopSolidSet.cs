// --------------------------------------------------
// ThemeEditor.Common - TopSolidSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class TopBackgroundSet
    {
        public bool EnableAlt;
        public bool FadeToWhite;
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
                EnableAlt = typeTwo && br.ReadBoolean(),
                FadeToWhite = typeTwo && br.ReadBoolean(),
            };
        }

        public void Write(BinaryWriter bw, bool typeTwo)
        {
            Main.Write(bw);
            bw.Write(Gradient);
            bw.Write(TextureOpacity);
            if (!typeTwo)
                return;
            bw.Write(EnableAlt);
            bw.Write(FadeToWhite);
        }
    }
}
