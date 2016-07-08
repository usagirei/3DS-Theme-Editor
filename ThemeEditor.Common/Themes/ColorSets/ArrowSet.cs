// --------------------------------------------------
// ThemeEditor.Common - ArrowSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class ArrowSet
    {
        /*
        * Data Order:
        * 
        * Dark
        * Main
        * Glow
        */

        public ColorRgb888 Border;
        public ColorRgb888 Pressed;
        public ColorRgb888 Unpressed;

        public static ArrowSet Read(BinaryReader br)
        {
            return new ArrowSet
            {
                Border = ColorRgb888.Read(br),
                Unpressed = ColorRgb888.Read(br),
                Pressed = ColorRgb888.Read(br)
            };
        }

        public void Write(BinaryWriter bw)
        {
            Border.Write(bw);
            Unpressed.Write(bw);
            Pressed.Write(bw);
        }
    }
}
