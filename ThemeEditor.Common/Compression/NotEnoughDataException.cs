// --------------------------------------------------
// ThemeEditor.Common - NotEnoughDataException.cs
// --------------------------------------------------

using System.IO;

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
    ///     An exception that is thrown by the decompression functions when there
    ///     is not enough data available in order to properly decompress the input.
    /// </summary>
    public class NotEnoughDataException : IOException
    {
        /// <summary>
        ///     Gets the number of bytes that was supposed to be written.
        /// </summary>
        public long DesiredLength { get; }

        /// <summary>
        ///     Gets the actual number of written bytes.
        /// </summary>
        public long WrittenLength { get; }

        /// <summary>
        ///     Creates a new NotEnoughDataException.
        /// </summary>
        /// <param name="currentOutSize">The actual number of written bytes.</param>
        /// <param name="totalOutSize">The desired number of written bytes.</param>
        public NotEnoughDataException(long currentOutSize, long totalOutSize)
            : base("Not enough data availble; 0x" + currentOutSize.ToString("X")
                   + " of " + (totalOutSize < 0 ? "???" : ("0x" + totalOutSize.ToString("X")))
                   + " bytes written.")
        {
            WrittenLength = currentOutSize;
            DesiredLength = totalOutSize;
        }
    }
}
