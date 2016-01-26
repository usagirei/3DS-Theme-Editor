// --------------------------------------------------
// ThemeEditor.Common - BottomBackgroundOuterSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class BottomBackgroundOuterSet
    {
        public ColorRgb888 Glow;
        public ColorRgb888 Main;
        public ColorRgb888 Stripe;

        public static BottomBackgroundOuterSet Read(BinaryReader br)
        {
            return new BottomBackgroundOuterSet
            {
                Stripe = ColorRgb888.Read(br),
                Main = ColorRgb888.Read(br),
                Glow = ColorRgb888.Read(br)
            };
        }

        public void Write(BinaryWriter bw)
        {
            Stripe.Write(bw);
            Main.Write(bw);
            Glow.Write(bw);
        }
    }
}
