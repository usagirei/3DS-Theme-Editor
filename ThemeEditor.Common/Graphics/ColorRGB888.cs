// --------------------------------------------------
// ThemeEditor.Common - ColorRgb888.cs
// --------------------------------------------------

using System.IO;

namespace ThemeEditor.Common.Graphics
{
    public struct ColorRgb888
    {
        public byte R;
        public byte G;
        public byte B;

        public static explicit operator uint(ColorRgb888 self)
        {
            return (uint) (self.R << 16 | self.G << 8 | self.B);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(ColorRgb888 a, ColorRgb888 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ColorRgb888 a, ColorRgb888 b)
        {
            return !(a.Equals(b));
        }

        public bool Equals(ColorRgb888 other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        public static implicit operator ColorRgb888(uint value)
        {
            byte r = (byte) ((value >> 16) & 0xFF);
            byte g = (byte) ((value >> 8) & 0xFF);
            byte b = (byte) (value & 0xFF);
            return new ColorRgb888(r, g, b);
        }

        public ColorRgb888(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static ColorRgb888 Read(BinaryReader br)
        {
            return new ColorRgb888
            {
                R = br.ReadByte(),
                G = br.ReadByte(),
                B = br.ReadByte()
            };
        }

        public override string ToString()
        {
            return $"RGB888: '{R}, {G}, {B}'";
        }

        public void Write(BinaryWriter br)
        {
            br.Write(R);
            br.Write(G);
            br.Write(B);
        }
    }
}
