using System.IO;
using System.Text;

namespace ThemeEditor.Common.SMDH
{
    public struct SMDHMatchMakerID
    {
        public static SMDHMatchMakerID Zero = new SMDHMatchMakerID(0, 0);
        public SMDHMatchMakerID(int id, long bitId) : this()
        {
            MatchMakerID = id;
            MatchMakerBitID = bitId;
        }

        public long MatchMakerBitID;
        public int MatchMakerID;

        public static SMDHMatchMakerID Read(Stream s)
        {
            SMDHMatchMakerID mmid = new SMDHMatchMakerID();
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                mmid.MatchMakerID = br.ReadInt32();
                mmid.MatchMakerBitID = br.ReadInt64();
            }
            return mmid;
        }

        public static void Write(SMDHMatchMakerID mmid, Stream s)
        {
            using (var bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                bw.Write(mmid.MatchMakerID);
                bw.Write(mmid.MatchMakerBitID);
            }
        }

        public override string ToString()
        {
            return $"{MatchMakerID:X8}-{MatchMakerBitID:X16}";
        }
    }
}