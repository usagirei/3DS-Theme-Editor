using System.IO;
using System.Text;

namespace ThemeEditor.Common.SMDH
{
    public class SMDHTitle
    {
        public string LongDesc;
        public string Publisher;
        public string ShortDesc;

        public SMDHTitle()
        {
            LongDesc = string.Empty;
            ShortDesc = string.Empty;
            Publisher = string.Empty;
        }

        private SMDHTitle(bool inter) {}

        public static SMDHTitle Read(Stream s)
        {
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
                return new SMDHTitle(true)
                {
                    ShortDesc = br.ReadFixedSizeString(Encoding.Unicode, 0x80),
                    LongDesc = br.ReadFixedSizeString(Encoding.Unicode, 0x100),
                    Publisher = br.ReadFixedSizeString(Encoding.Unicode, 0x80),
                };
        }

        public static void Write(SMDHTitle title, Stream s)
        {
            using (var bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                bw.WriteFixedSizeString(title.ShortDesc, Encoding.Unicode, 0x80);
                bw.WriteFixedSizeString(title.LongDesc, Encoding.Unicode, 0x100);
                bw.WriteFixedSizeString(title.Publisher, Encoding.Unicode, 0x80);
            }
        }

        public override string ToString()
        {
            return $"{LongDesc} ({ShortDesc}) - {Publisher}";
        }
    }
}
