// --------------------------------------------------
// ThemeEditor.Common - OpenCloseSet.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.Themes.ColorSets
{
    public class OpenCloseSet
    {
        public ColorArgb8888 _;
        public ColorRgb888 __;

        public ColorArgb8888 ___;
        public ColorRgb888 ____;
        public ColorRgb888 Glow;
        public ColorRgb888 Pressed;
        public ColorRgb888 TextGlow;
        public ColorRgb888 TextPressed;
        public ColorRgb888 TextUnpressed;
        public ColorRgb888 Unpressed;

        public static OpenCloseSet Read(BinaryReader br)
        {
            return new OpenCloseSet
            {
                _ = ColorArgb8888.Read(br),
                Pressed = ColorRgb888.Read(br),
                Unpressed = ColorRgb888.Read(br),
                Glow = ColorRgb888.Read(br),
                __ = ColorRgb888.Read(br),
                ___ = ColorArgb8888.Read(br),
                TextGlow = ColorRgb888.Read(br),
                TextUnpressed = ColorRgb888.Read(br),
                TextPressed = ColorRgb888.Read(br),
                ____ = ColorRgb888.Read(br),
            };
        }

        public void Write(BinaryWriter bw)
        {
            _.Write(bw);
            Pressed.Write(bw);
            Unpressed.Write(bw);
            Glow.Write(bw);
            __.Write(bw);
            ___.Write(bw);
            TextGlow.Write(bw);
            TextUnpressed.Write(bw);
            TextPressed.Write(bw);
            ____.Write(bw);
        }
    }
}
