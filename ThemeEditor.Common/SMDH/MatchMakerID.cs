using System.IO;
using System.Text;

namespace ThemeEditor.Common.SMDH
{
    public struct MatchMakerID
    {
        public static MatchMakerID Zero = new MatchMakerID(0, 0);
        public MatchMakerID(int id, long bitId) : this()
        {
            ID = id;
            BitID = bitId;
        }

        public long BitID;
        public int ID;

        public static MatchMakerID Read(Stream s)
        {
            MatchMakerID mmid = new MatchMakerID();
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                mmid.ID = br.ReadInt32();
                mmid.BitID = br.ReadInt64();
            }
            return mmid;
        }

        public static void Write(MatchMakerID mmid, Stream s)
        {
            using (var bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                bw.Write(mmid.ID);
                bw.Write(mmid.BitID);
            }
        }

        public override string ToString()
        {
            return $"{ID:X8}-{BitID:X16}";
        }
    }
}