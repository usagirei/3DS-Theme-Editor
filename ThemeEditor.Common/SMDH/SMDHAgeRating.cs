using System.IO;

namespace ThemeEditor.Common.SMDH
{
    public struct SMDHAgeRating
    {
        public byte Value;

        public byte Rating
        {
            get { return (byte) (Value & 0x1F); }
            set { Value = (byte) ((Value & ~0x1F) | (value & 0x1F)); }
        }

        public SMDHAgeRatingFlags Flags
        {
            get { return (SMDHAgeRatingFlags) (Value & ~0x1F); }
            set { Value = (byte) ((Value & 0x1F) | ((byte) value & ~0x1F)); }
        }

        /*
        public bool Enable
        {
            get { return Flags.HasFlag(SMDHAgeRatingFlags.Enable); }
            set
            {
                if (value)
                    Flags |= SMDHAgeRatingFlags.Enable;
                else
                    Flags &= ~SMDHAgeRatingFlags.Enable;
            }
        }

        public bool AllAges
        {
            get { return Flags.HasFlag(SMDHAgeRatingFlags.AllAges); }
            set
            {
                if (value)
                    Flags |= SMDHAgeRatingFlags.AllAges;
                else
                    Flags &= ~SMDHAgeRatingFlags.AllAges;
            }
        }

        public bool RatingPending
        {
            get { return Flags.HasFlag(SMDHAgeRatingFlags.RatingPending); }
            set
            {
                if (value)
                    Flags |= SMDHAgeRatingFlags.RatingPending;
                else
                    Flags &= ~SMDHAgeRatingFlags.RatingPending;
            }
        }
        */

        public static SMDHAgeRating Read(Stream s)
        {
            return new SMDHAgeRating
            {
                Value = (byte) s.ReadByte()
            };
        }

        public static void Write(SMDHAgeRating rating, Stream s)
        {
            s.WriteByte(rating.Value);
        }

        public override string ToString()
        {
            return Flags + " - " + Rating;
        }
    }
}
