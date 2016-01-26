// --------------------------------------------------
// ThemeEditor.Common - TooMuchInputException.cs
// --------------------------------------------------

using System;

namespace ThemeEditor.Common.Compression
{
    //  Taken from YATA Project
    //  https://github.com/Reisyukaku/YATA/blob/master/LZ11.cs
    //  That may have been taken from pk3DS
    //  https://github.com/kwsch/pk3DS/blob/master/pk3DS/3DS/LZSS.cs
    //  That was a possible port of the DSDecmp Code
    //  https://github.com/Barubary/dsdecmp/tree/master/CSharp/DSDecmp
    //  Credits for all those guys

    /// <summary>
    ///     An exception indication that the input has more data than required in order
    ///     to decompress it. This may indicate that more sub-files are present in the file.
    /// </summary>
    public class TooMuchInputException : Exception
    {
        /// <summary>
        ///     Gets the number of bytes read by the decompressed to decompress the stream.
        /// </summary>
        public long ReadBytes { get; private set; }

        /// <summary>
        ///     Creates a new exception indicating that the input has more data than necessary for
        ///     decompressing th stream. It may indicate that other data is present after the compressed
        ///     stream.
        /// </summary>
        /// <param name="readBytes">The number of bytes read by the decompressor.</param>
        /// <param name="totLength">The indicated length of the input stream.</param>
        public TooMuchInputException(long readBytes, long totLength)
            : base("The input contains more data than necessary. Only used 0x"
                   + readBytes.ToString("X") + " of 0x" + totLength.ToString("X") + " bytes")
        {
            ReadBytes = readBytes;
        }
    }
}
