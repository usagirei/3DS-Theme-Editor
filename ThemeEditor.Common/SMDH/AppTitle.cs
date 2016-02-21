using System.IO;
using System.Text;

namespace ThemeEditor.Common.SMDH
{
    public class AppTitle
    {
        public string LongDesc;
        public string Publisher;
        public string ShortDesc;

        public AppTitle()
        {
            LongDesc = string.Empty;
            ShortDesc = string.Empty;
            Publisher = string.Empty;
        }

        private AppTitle(bool inter) {}

        public static AppTitle Read(Stream s)
        {
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
                return new AppTitle(true)
                {
                    ShortDesc = br.ReadFixedSizeString(Encoding.Unicode, 0x80),
                    LongDesc = br.ReadFixedSizeString(Encoding.Unicode, 0x100),
                    Publisher = br.ReadFixedSizeString(Encoding.Unicode, 0x80),
                };
        }

        public static void Write(AppTitle title, Stream s)
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
