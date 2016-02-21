using System.IO;

namespace ThemeEditor.Common.SMDH
{
    public struct AgeRating
    {
        public byte Value;

        public byte Rating
        {
            get { return (byte) (Value & 0x1F); }
            set { Value = (byte) ((Value & ~0x1F) | (value & 0x1F)); }
        }

        public AgeRatingFlags Flags
        {
            get { return (AgeRatingFlags) (Value & ~0x1F); }
            set { Value = (byte) ((Value & 0x1F) | ((byte) value & ~0x1F)); }
        }

        /*
        public bool Enable
        {
            get { return Flags.HasFlag(AgeRatingFlags.Enable); }
            set
            {
                if (value)
                    Flags |= AgeRatingFlags.Enable;
                else
                    Flags &= ~AgeRatingFlags.Enable;
            }
        }

        public bool AllAges
        {
            get { return Flags.HasFlag(AgeRatingFlags.AllAges); }
            set
            {
                if (value)
                    Flags |= AgeRatingFlags.AllAges;
                else
                    Flags &= ~AgeRatingFlags.AllAges;
            }
        }

        public bool RatingPending
        {
            get { return Flags.HasFlag(AgeRatingFlags.RatingPending); }
            set
            {
                if (value)
                    Flags |= AgeRatingFlags.RatingPending;
                else
                    Flags &= ~AgeRatingFlags.RatingPending;
            }
        }
        */

        public static AgeRating Read(Stream s)
        {
            return new AgeRating
            {
                Value = (byte) s.ReadByte()
            };
        }

        public static void Write(AgeRating rating, Stream s)
        {
            s.WriteByte(rating.Value);
        }

        public override string ToString()
        {
            return Flags + " - " + Rating;
        }
    }
}
