// --------------------------------------------------
// ThemeEditor.Common - ColorArgb8888.cs
// --------------------------------------------------

using System.IO;

namespace ThemeEditor.Common.Graphics
{
    public struct ColorArgb8888
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public ColorArgb8888(byte a, byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ColorArgb8888 other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        public static bool operator ==(ColorArgb8888 a, ColorArgb8888 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ColorArgb8888 a, ColorArgb8888 b)
        {
            return !(a.Equals(b));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
            }
        }

        public static explicit operator uint(ColorArgb8888 self)
        {
            return (uint) (self.A << 24 | self.R << 16 | self.G << 8 | self.B);
        }

        public static implicit operator ColorArgb8888(uint value)
        {
            byte a = (byte) ((value >> 24) & 0xFF);
            byte r = (byte) ((value >> 16) & 0xFF);
            byte g = (byte) ((value >> 8) & 0xFF);
            byte b = (byte) (value & 0xFF);
            return new ColorArgb8888(a, r, g, b);
        }

        public static ColorArgb8888 Read(BinaryReader br)
        {
            return new ColorArgb8888
            {
                R = br.ReadByte(),
                G = br.ReadByte(),
                B = br.ReadByte(),
                A = br.ReadByte(),
            };
        }

        public override string ToString()
        {
            return $"ARGB8888: '{A}, {R}, {G}, {B}'";
        }

        public void Write(BinaryWriter br)
        {
            br.Write(R);
            br.Write(G);
            br.Write(B);
            br.Write(A);
        }
    }
}
