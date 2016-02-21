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
    }
}
