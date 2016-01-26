// --------------------------------------------------
// ThemeEditor.Common - InputTooLargeException.cs
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
    ///     An exception indicating that the file cannot be compressed, because the decompressed size
    ///     cannot be represented in the current compression format.
    /// </summary>
    public class InputTooLargeException : Exception
    {
        /// <summary>
        ///     Creates a new exception that indicates that the input is too big to be compressed.
        /// </summary>
        public InputTooLargeException()
            : base("The compression ratio is not high enough to fit the input "
                   + "in a single compressed file.") {}
    }
}
