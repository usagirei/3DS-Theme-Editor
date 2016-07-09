using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemeEditor.Common
{
    internal static class Extensions
    {
        public static string ReadFixedSizeString(this BinaryReader br, Encoding encoding, int bytes)
        {
            var strData = br.ReadBytes(bytes);
            var str = encoding.GetString(strData);
            var i = str.IndexOf('\0');
            return str.Remove(i);
        }

        public static void WriteFixedSizeString(this BinaryWriter bw, string str, Encoding encoding, int bytes)
        {
            var buf = new byte[bytes];
            var strData = encoding.GetBytes(str);
            Buffer.BlockCopy(strData, 0, buf, 0, Math.Min(strData.Length, buf.Length));
            bw.Write(buf);
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) < 0
                ? min
                : (value.CompareTo(max) > 0
                    ? max
                    : value);
        }
    }
}
