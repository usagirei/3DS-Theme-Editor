using System;

namespace ThemeEditor.Common.SMDH
{
    [Flags]
    public enum AgeRatingFlags : byte
    {
        None = 0x00,
        Enable = 0x80,
        RatingPending = 0x40,
        AllAges = 0x20
    }
}