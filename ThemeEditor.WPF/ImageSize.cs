// --------------------------------------------------
// 3DS Theme Editor - ImageSize.cs
// --------------------------------------------------

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.WPF
{
    internal struct ImageSize
    {
        public readonly int X;
        public readonly int Y;
        public readonly RawTexture.DataFormat Format;

        public ImageSize(int x, int y, RawTexture.DataFormat format) : this()
        {
            X = x;
            Y = y;
            Format = format;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(ImageSize other)
        {
            return X == other.X && Y == other.Y && Format == other.Format;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ (int) Format;
                return hashCode;
            }
        }
    }
}
