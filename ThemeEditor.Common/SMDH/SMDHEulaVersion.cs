using System.IO;

namespace ThemeEditor.Common.SMDH
{
    public struct SMDHEulaVersion
    {
        public static SMDHEulaVersion Zero = new SMDHEulaVersion(0,0);
        public byte Minor;
        public byte Major;

        public static SMDHEulaVersion Read(Stream s)
        {
            var eula = new SMDHEulaVersion
            {
                Minor = (byte) s.ReadByte(),
                Major = (byte) s.ReadByte()
            };
            return eula;
        }

        public SMDHEulaVersion(byte minor, byte major) : this()
        {
            Minor = minor;
            Major = major;
        }

        public static void Write(SMDHEulaVersion eula, Stream s)
        {
            s.WriteByte(eula.Minor);
            s.WriteByte(eula.Major);
        }

        public override string ToString()
        {
            return Major + "." + Minor;
        }
    }
}