// --------------------------------------------------
// ThemeEditor.Common - RawTexture.cs
// --------------------------------------------------

using System;
using System.IO;

namespace ThemeEditor.Common.Graphics
{
    public class RawTexture
    {
        public enum DataFormat
        {
            Invalid = 0,
            A8 = 1,
            Bgr565 = 2,
            Bgr888 = 3,
        }

        public byte[] Data; // { get; private set; }
        public int Height; // { get; private set; }

        public int Width; // { get; private set; }
        public DataFormat Format { get; private set; }

        public RawTexture(int width, int height, DataFormat format)
        {
            Width = width;
            Height = height;
            var tgtSize = (int)format * width * height;
            Format = format;
            Data = new byte[tgtSize];
        }

        public RawTexture(int width, int height, DataFormat format, byte[] data)
            : this(width, height, format)
        {
            if (data.Length != Data.Length)
                throw new ArgumentException("Provided Data is of invalid Size", nameof(data));
            Buffer.BlockCopy(data, 0, Data, 0, data.Length);
        }

        public RawTexture()
        {
            Width = 0;
            Height = 0;
            Data = new byte[0];
            Format = DataFormat.Invalid;
        }

        public static void DecToCoord(uint dec, out uint x, out uint y)
        {
            x = dec;
            y = (x >> 1);
            x &= 0x55555555;
            y &= 0x55555555;
            x |= (x >> 1);
            y |= (y >> 1);
            x &= 0x33333333;
            y &= 0x33333333;
            x |= (x >> 2);
            y |= (y >> 2);
            x &= 0x0f0f0f0f;
            y &= 0x0f0f0f0f;
            x |= (x >> 4);
            y |= (y >> 4);
            x &= 0x00ff00ff;
            y &= 0x00ff00ff;
            x |= (x >> 8);
            y |= (y >> 8);
            x &= 0x0000ffff;
            y &= 0x0000ffff;
        }

        private static uint CoordToDec(uint x, uint y)
        {
            x &= 0x0000ffff;
            y &= 0x0000ffff;
            x |= (x << 8);
            y |= (y << 8);
            x &= 0x00ff00ff;
            y &= 0x00ff00ff;
            x |= (x << 4);
            y |= (y << 4);
            x &= 0x0f0f0f0f;
            y &= 0x0f0f0f0f;
            x |= (x << 2);
            y |= (y << 2);
            x &= 0x33333333;
            y &= 0x33333333;
            x |= (x << 1);
            y |= (y << 1);
            x &= 0x55555555;
            y &= 0x55555555;
            return x | (y << 1);
        }

        private static int GreatestCommonMultiple(int num, int mult)
        {
            var gcm = ((num + mult - 1) / mult) * mult;
            return gcm;
        }


        private static int NextLargestPowerOfTwo(int x)
        {
            x--;
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (x + 1);
        }

        public byte[] Decode()
        {
            switch (Format)
            {
                case DataFormat.Bgr565:
                    return Decode_Bgr565();
                case DataFormat.Bgr888:
                    return Decode_Bgr888();
                case DataFormat.A8:
                    return Decode_A8();
            }
            return null;
        }

        public void Encode(byte[] bgrData, int width, int height, DataFormat format)
        {
            if (Format != format || Width != width || Height != height)
            {
                Width = width;
                Height = height;
                var tgtSize = (int)format * width * height;
                Format = format;
                Data = new byte[tgtSize];
            }
            Encode(bgrData);
        }

        public void Encode(byte[] bgrData)
        {
            switch (Format)
            {
                case DataFormat.Invalid:
                    return;
                case DataFormat.Bgr565:
                    Encode_Rgb565(bgrData);
                    return;
                case DataFormat.Bgr888:
                    Encode_Rgb888(bgrData);
                    return;
                case DataFormat.A8:
                    Encode_A8(bgrData);
                    return;
            }
        }

        public void EdgeBleed(int x, int y, int sx, int sy)
        {
            byte[] bgrData = Decode();
            int eSize = 3; // BGR Data

            void EdgeBleedY(int r, int y0, int y1)
            {
                if (y0 > y1)
                {
                    int t = y1;
                    y1 = y0;
                    y0 = t;
                }

                int dSize = Width * 3;
                byte[] line = new byte[dSize];
                Buffer.BlockCopy(bgrData, r * dSize, line, 0, dSize);
                for (int i = y0; i <= y1; i++)
                    Buffer.BlockCopy(line, 0, bgrData, i * dSize, dSize);
            }

            void EdgeBleedX(int r, int x0, int x1)
            {
                byte[] bd = bgrData;
                int es = eSize;
                int w = Width;

                if (x0 > x1)
                {
                    int t = x1;
                    x1 = x0;
                    x0 = t;
                }

                int dSize = Height * eSize;
                byte[] line = new byte[dSize];
                for (int j = 0; j < Height; j++)
                    Buffer.BlockCopy(bd, (j * w + r) * es, line, j * es, es);

                for (int i = x0; i <= x1; i++)
                    for (int j = 0; j < Height; j++)
                        Buffer.BlockCopy(line, j * es, bd, (j * w + i) * es, es);

            }

            EdgeBleedX(x, 0, x);
            EdgeBleedX(x + sx - 1, x + sx - 1, Width - 1);
            EdgeBleedY(y, 0, y);
            EdgeBleedY(y + sy - 1, y + sy - 1, Height - 1);

            Encode(bgrData);
        }

        public void Read(Stream s)
        {
            if (s.Length - s.Position < Data.Length)
                throw new ArgumentException("Stream doesn't contain enough data", nameof(s));
            s.Read(Data, 0, Data.Length);
        }

        public override string ToString()
        {
            return $"Format: '{Format}' - {Width}x{Height}";
        }

        private byte[] Decode_A8()
        {
            var bgrData = new byte[Width * Height * 3];

            var p = GreatestCommonMultiple(Width, 8) / 8;
            if (p == 0)
                p = 1;

            for (uint i = 0; i < Data.Length; i += 1)
            {
                byte a = Data[i + 0]; // 8

                uint x, y;
                DecToCoord(i % 64, out x, out y);
                uint tile = i / 64;

                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                var idx = 3 * (y * Width + x);

                bgrData[idx + 0] = a;
                bgrData[idx + 1] = a;
                bgrData[idx + 2] = a;
            }
            GraphicUtils.RotatebgrDataClockwise(bgrData, 3 * Width);
            return bgrData;
        }

        private byte[] Decode_Bgr565()
        {
            var bgrData = new byte[Width * Height * 3];

            var p = GreatestCommonMultiple(Width, 8) / 8;
            if (p == 0)
                p = 1;

            for (uint i = 0, j = 0; i < Data.Length; i += 2, j += 1)
            {
                int px = (Data[i + 1] << 8 | Data[i]);

                byte r = (byte)(((px >> 11) & 0x1f) << 3); // 5
                byte g = (byte)(((px >> 5) & 0x3f) << 2); // 6
                byte b = (byte)(((px >> 0) & 0x1f) << 3); // 5

                uint x, y;
                DecToCoord(j % 64, out x, out y);
                uint tile = j / 64;

                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                var idx = 3 * (y * Width + x);

                bgrData[idx + 0] = b;
                bgrData[idx + 1] = g;
                bgrData[idx + 2] = r;
            }

            return bgrData;
        }

        private byte[] Decode_Bgr888()
        {
            var bgrData = new byte[Width * Height * 3];

            var p = GreatestCommonMultiple(Width, 8) / 8;
            if (p == 0)
                p = 1;

            for (uint i = 0, j = 0; i < Data.Length; i += 3, j += 1)
            {
                byte b = Data[i + 0]; // 8
                byte g = Data[i + 1]; // 8
                byte r = Data[i + 2]; // 8

                uint x, y;
                DecToCoord(j % 64, out x, out y);
                uint tile = j / 64;

                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                var idx = 3 * (y * Width + x);

                bgrData[idx + 0] = b;
                bgrData[idx + 1] = g;
                bgrData[idx + 2] = r;
            }

            return bgrData;
        }

        private void Encode_A8(byte[] bgrData)
        {
            if (bgrData.Length != 3 * Width * Height)
                throw new ArgumentException("RGB Data size is invalid", nameof(bgrData));

            var gscData = new byte[bgrData.Length];
            Buffer.BlockCopy(bgrData, 0, gscData, 0, bgrData.Length);

            GraphicUtils.BgrDataToGrayScale(gscData);
            GraphicUtils.RotatebgrDataCounterClockwise(gscData, 3 * Width);

            var p = GreatestCommonMultiple(Width, 8) / 8;
            if (p == 0)
                p = 1;

            for (uint i = 0; i < Data.Length; i += 1)
            {
                uint x, y;
                DecToCoord(i % 64, out x, out y);
                uint tile = i / 64;

                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                var idx = 3 * (y * Width + x);
                var g = gscData[idx + 0];
                Data[i] = g;
            }
        }

        private void Encode_Rgb565(byte[] bgrData)
        {
            if (bgrData.Length != 3 * Width * Height)
                throw new ArgumentException("RGB Data size is invalid", nameof(bgrData));

            var bayData = new byte[bgrData.Length];
            Buffer.BlockCopy(bgrData, 0, bayData, 0, bgrData.Length);
            GraphicUtils.Bayer565Dither(bayData, 3 * Width);

            var p = GreatestCommonMultiple(Width, 8) / 8;
            if (p == 0)
                p = 1;

            for (uint i = 0, j = 0, k = 0; k < Data.Length; i += 3, j += 1, k += 2)
            {
                uint x, y;
                DecToCoord(j % 64, out x, out y);
                uint tile = j / 64;

                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                var idx = 3 * (y * Width + x);

                var b = bayData[idx + 0] >> 3 & 0x1f;
                var g = bayData[idx + 1] >> 2 & 0x3f;
                var r = bayData[idx + 2] >> 3 & 0x1f;

                int px = (r << 11 | g << 5 | b) & 0xffff;

                Data[k + 0] = (byte)(px >> 0 & 0xFF);
                Data[k + 1] = (byte)(px >> 8 & 0xFF);
            }
        }

        private void Encode_Rgb888(byte[] bgrData)
        {
            if (bgrData.Length != 3 * Width * Height)
                throw new ArgumentException("RGB Data size is invalid", nameof(bgrData));

            var p = GreatestCommonMultiple(Width, 8) / 8;
            if (p == 0)
                p = 1;

            for (uint i = 0, j = 0; i < Data.Length; i += 3, j += 1)
            {
                uint x, y;
                DecToCoord(j % 64, out x, out y);
                uint tile = j / 64;

                x += (uint)(tile % p) * 8;
                y += (uint)(tile / p) * 8;

                var idx = 3 * (y * Width + x);

                var b = bgrData[idx + 0];
                var g = bgrData[idx + 1];
                var r = bgrData[idx + 2];

                Data[i + 0] = b;
                Data[i + 1] = g;
                Data[i + 2] = r;
            }
        }
    }
}
