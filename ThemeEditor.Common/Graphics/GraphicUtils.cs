// --------------------------------------------------
// ThemeEditor.Common - GraphicUtils.cs
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace ThemeEditor.Common.Graphics
{
    public static class GraphicUtils
    {
        public static readonly byte[] Bayer8X8 =
        {
            1, 49, 13, 61, 4, 52, 16, 64, //
            33, 17, 45, 29, 36, 20, 48, 32, //
            9, 57, 5, 53, 12, 60, 8, 56, //
            41, 25, 37, 21, 44, 28, 40, 24, //
            3, 51, 15, 63, 2, 50, 14, 62, //
            35, 19, 47, 31, 34, 18, 46, 30, //
            11, 59, 7, 55, 10, 58, 6, 54, //
            43, 27, 39, 23, 42, 26, 38, 22, //
        };

        /// <summary>
        ///     Dithers Bgr888 Data into Bgr565-Safe Data.
        /// </summary>
        /// <param name="bgrData">Bgr888 Data</param>
        /// <param name="stride">Data Stride</param>
        public static void Bayer565Dither(byte[] bgrData, int stride)
        {
            var width = stride / 3;
            var height = bgrData.Length / stride;

            //byte[] rgbOut = new byte[bgrData.Length];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    int pIdx = 3 * (y * width + x);
                    var tIdx = ((y & 7) << 3) + (x & 7);

                    var t = Bayer8X8[tIdx];

                    const int gB = 1 << 3;
                    const int gG = 1 << 2;
                    const int gR = 1 << 3;

                    byte b = Clamp(bgrData[pIdx + 0] + t * gB / 65);
                    byte g = Clamp(bgrData[pIdx + 1] + t * gG / 65);
                    byte r = Clamp(bgrData[pIdx + 2] + t * gR / 65);

                    byte bAfter = GetClosestColor(b, 5);
                    byte gAfter = GetClosestColor(g, 6);
                    byte rAfter = GetClosestColor(r, 5);

                    bgrData[pIdx + 0] = bAfter;
                    bgrData[pIdx + 1] = gAfter;
                    bgrData[pIdx + 2] = rAfter;

                    /*
                    rgbOut[pIdx + 2] = bAfter;
                    rgbOut[pIdx + 1] = gAfter;
                    rgbOut[pIdx + 0] = rAfter;
                    */
                }
            }
            //return rgbOut;
        }

        public static void BgrDataToGrayScale(byte[] data)
        {
            //byte[] averaged = new byte[data.Length];
            for (int i = 0; i < data.Length; i += 3)
            {
                byte r = (byte) (data[i + 0] * 76 / 255);
                byte g = (byte) (data[i + 1] * 150 / 255);
                byte b = (byte) (data[i + 2] * 29 / 255);

                //averaged[i + 0] = averaged[i + 1] = averaged[i + 2] = (byte) (r + g + b);
                data[i + 0] = data[i + 1] = data[i + 2] = (byte) (r + g + b);
            }
            //return data;
        }

        /// <summary>
        ///     Blits a Region of Bgr888 Data from <paramref name="srcData" /> into <paramref name="tgtData" />
        /// </summary>
        /// <param name="srcData">Source Bgr888 Data</param>
        /// <param name="srcStride">Source Bgr888 Data Stride</param>
        /// <param name="tgtData">Target Bgr888 Data</param>
        /// <param name="tgtStride">Target Bgr888 Data Stride</param>
        /// <param name="srcX">Source X Coordinate</param>
        /// <param name="srcY">Source Y Coordinate</param>
        /// <param name="tgtX">Target X Coordinate</param>
        /// <param name="tgtY">Target Y Coordinate</param>
        /// <param name="width">Region Width</param>
        /// <param name="height">Region Height</param>
        /// <returns>True if Successful</returns>
        public static bool BlitbgrData
            (
            byte[] srcData,
            int srcStride,
            byte[] tgtData,
            int tgtStride,
            int srcX,
            int srcY,
            int tgtX,
            int tgtY,
            int width,
            int height)
        {
            int toCopy = width * 3;
            int tgtMax = (tgtY + height - 1) * tgtStride + (tgtX * 3) + toCopy;
            int srcMax = (srcY + height - 1) * srcStride + (srcX * 3) + toCopy;

            if (tgtMax > tgtData.Length || srcMax > srcData.Length)
                return false;

            for (int i = 0; i < height; i++)
            {
                int srcOffset = (srcY + i) * srcStride + srcX;
                int tgtOffset = (tgtY + i) * tgtStride + tgtX;
                Buffer.BlockCopy(srcData, srcOffset, tgtData, tgtOffset, toCopy);
            }
            return true;
        }

        /// <summary>
        ///     Generates a Palette of up to <paramref name="max" /> Colors from the specified <paramref name="bgrData" />
        /// </summary>
        /// <param name="bgrData">Bgr888 Data</param>
        /// <param name="max">Max Colors</param>
        /// <param name="tolerance">Tolerance</param>
        /// <returns>Palette</returns>
        public static List<ColorRgb888> PaletteGen(byte[] bgrData, int max, float tolerance = 0.025f)
        {
            var poll = GenBuckets(bgrData, 5);
            var sortedPoll = poll.OrderByDescending(p => p.Value.Count);

            List<ColorRgb888> output = new List<ColorRgb888>(max);
            int inTolerance = (int) (tolerance * 255 * 255 * 3);

            if (!poll.Any())
                return output;

            output.Add(sortedPoll.First().Key);
            for (var i = 1; i < max; i++)
            {
                var added = output.Take(i);
                try
                {
                    var toAdd = sortedPoll.First(pair => added.All(oldC =>
                    {
                        var newC = pair.Key;

                        var dist = 0.0;
                        dist += Math.Pow(newC.R - oldC.R, 2);
                        dist += Math.Pow(newC.G - oldC.G, 2);
                        dist += Math.Pow(newC.B - oldC.B, 2);

                        return dist > inTolerance;
                    })).Key;
                    output.Add(toAdd);
                }
                catch (InvalidOperationException)
                {
                    break;
                }
            }

            return output
                .Select(c =>
                {
                    double h, s, l;
                    c.ToHsl(out h, out s, out l);
                    return new
                    {
                        c,
                        h,
                        s,
                        l
                    };
                })
                .OrderBy(c => c.s)
                .ThenBy(c => c.h)
                .ThenBy(c => c.l)
                .Select(c => c.c)
                .ToList();
        }

        public static void RotatebgrDataClockwise(byte[] bgrData, int stride)
        {
            var width = stride / 3;
            var height = bgrData.Length / stride;

            if (width != height)
                throw new ArgumentException("RGB Data is not Square", nameof(bgrData));

            var rotBuf = new byte[bgrData.Length];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var idxSrc = 3 * (j * width + i);
                    var idxTgt = 3 * ((width - 1 - i) * width + j);

                    rotBuf[idxTgt + 0] = bgrData[idxSrc + 0];
                    rotBuf[idxTgt + 1] = bgrData[idxSrc + 1];
                    rotBuf[idxTgt + 2] = bgrData[idxSrc + 2];
                }
            }
            Buffer.BlockCopy(rotBuf, 0, bgrData, 0, rotBuf.Length);
        }

        public static void RotatebgrDataCounterClockwise(byte[] bgrData, int stride)
        {
            var width = stride / 3;
            var height = bgrData.Length / stride;

            if (width != height)
                throw new ArgumentException("RGB Data is not Square", nameof(bgrData));

            var rotBufA8 = new byte[bgrData.Length];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var idxSrc = 3 * ((width - 1 - i) * width + j);
                    var idxTgt = 3 * (j * width + i);

                    rotBufA8[idxTgt + 0] = bgrData[idxSrc + 0];
                    rotBufA8[idxTgt + 1] = bgrData[idxSrc + 1];
                    rotBufA8[idxTgt + 2] = bgrData[idxSrc + 2];
                }
            }
            Buffer.BlockCopy(rotBufA8, 0, bgrData, 0, rotBufA8.Length);
        }

        private static byte Clamp(int a)
        {
            return (byte) (a < 0
                               ? 0
                               : a > 255
                                     ? 255
                                     : a);
        }

        private static Dictionary<ColorRgb888, Bucket<ColorRgb888>> GenBuckets(byte[] bgrData, int colorDepth)
        {
            var graph = new Dictionary<ColorRgb888, Bucket<ColorRgb888>>();
            for (var i = 0; i < bgrData.Length; i += 3)
            {
                var c = new ColorRgb888
                {
                    B = bgrData[i + 0],
                    G = bgrData[i + 1],
                    R = bgrData[i + 2],
                };
                var d = GetClosestColor(c, colorDepth);
                if (!graph.ContainsKey(d))
                    graph[d] = new Bucket<ColorRgb888>();
                graph[d].Elements.Add(c);
            }
            return graph;
        }

        private static ColorRgb888 GetClosestColor(ColorRgb888 col, int depth)
        {
            int invDepth = 8 - depth;
            return new ColorRgb888
            {
                R = (byte) ((col.R >> invDepth) << invDepth),
                G = (byte) ((col.G >> invDepth) << invDepth),
                B = (byte) ((col.B >> invDepth) << invDepth),
            };
        }

        private static byte GetClosestColor(byte channel, int depth)
        {
            var invDepth = 8 - depth;
            return (byte) ((channel >> invDepth) << invDepth);
        }

        private static void ToHsl(this ColorRgb888 rgb, out double h, out double s, out double l)
        {
            const double EPSILON = 1.0 / 255;

            var r = EPSILON * rgb.R;
            var g = EPSILON * rgb.G;
            var b = EPSILON * rgb.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

            if (Math.Abs(chroma) < EPSILON)
            {
                h1 = 0;
            }
            else if (Math.Abs(max - r) < EPSILON)
            {
                h1 = ((g - b) / chroma) % 6;
            }
            else if (Math.Abs(max - g) < EPSILON)
            {
                h1 = 2 + (b - r) / chroma;
            }
            else //if (max == b)
            {
                h1 = 4 + (r - g) / chroma;
            }

            var lightness = 0.5 * (max - min);
            var saturation = Math.Abs(chroma) < EPSILON
                                 ? 0
                                 : chroma / (1 - Math.Abs(2 * lightness - 1));

            h = 60 * h1;
            s = saturation;
            l = lightness;
        }
    }
}
