using System.IO;

namespace ThemeEditor.Common.SMDH
{
    public struct EulaVersion
    {
        public static EulaVersion Zero = new EulaVersion(0,0);
        public byte Minor;
        public byte Major;

        public static EulaVersion Read(Stream s)
        {
            var eula = new EulaVersion
            {
                Minor = (byte) s.ReadByte(),
                Major = (byte) s.ReadByte()
            };
            return eula;
        }

        public EulaVersion(byte minor, byte major) : this()
        {
            Minor = minor;
            Major = major;
        }

        public static void Write(EulaVersion eula, Stream s)
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