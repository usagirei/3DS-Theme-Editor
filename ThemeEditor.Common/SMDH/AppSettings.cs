using System.IO;
using System.Text;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.SMDH
{
    public class AppSettings
    {
        public AgeRating[] AgeRatings;
        public RegionLockFlags RegionLock;
        public MatchMakerID MatchMakerID;
        public SettingFlags Flags;
        public EulaVersion EULA;
        public short Reserved;
        public float OptimalBannerFrame;
        public uint StreetPassID;
      

        private AppSettings(bool inter)
        {
            AgeRatings = new AgeRating[16];
        }

        public AppSettings() : this(true)
        {
            for (int i = 0; i < 16; i++)
                AgeRatings[i] = new AgeRating();
            RegionLock = RegionLockFlags.RegionFree;
            MatchMakerID = MatchMakerID.Zero;
            Flags = SettingFlags.None;
            EULA = EulaVersion.Zero;
            Reserved = 0;
            OptimalBannerFrame = 0;
            StreetPassID = 0;
        }

        public static AppSettings Read(Stream s)
        {
            AppSettings sett = new AppSettings(true);
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                for (int i = 0; i < 16; i++)
                    sett.AgeRatings[i] = AgeRating.Read(s);
                sett.RegionLock = (RegionLockFlags) br.ReadInt32();
                sett.MatchMakerID = MatchMakerID.Read(s);
                sett.Flags = (SettingFlags) br.ReadInt32();
                sett.EULA = EulaVersion.Read(s);
                sett.Reserved = br.ReadInt16();
                sett.OptimalBannerFrame = br.ReadSingle();
                sett.StreetPassID = br.ReadUInt32();
            }
            return sett;
        }

        public static void Write(AppSettings sett, Stream s)
        {
            using (var bw = new BinaryWriter(s, Encoding.ASCII, true))
            {
                for (int i = 0; i < 16; i++)
                    AgeRating.Write(sett.AgeRatings[i], s);
                bw.Write((int) sett.RegionLock);
                MatchMakerID.Write(sett.MatchMakerID, s);
                bw.Write((int) sett.Flags);
                EulaVersion.Write(sett.EULA, s);
                bw.Write(sett.Reserved);
                bw.Write(sett.OptimalBannerFrame);
                bw.Write(sett.StreetPassID);
            }
        }
    }
}