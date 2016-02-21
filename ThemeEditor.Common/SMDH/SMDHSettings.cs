using System.IO;
using System.Text;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.Common.SMDH
{
    public class SMDHSettings
    {
        public SMDHAgeRating[] AgeRatings;
        public SMDHRegionLockFlags RegionLock;
        public SMDHMatchMakerID MatchMakerID;
        public SMDHSettingFlags Flags;
        public SMDHEulaVersion EULA;
        public short Reserved;
        public float OptimalBannerFrame;
        public uint StreetPassID;
      

        private SMDHSettings(bool inter)
        {
            AgeRatings = new SMDHAgeRating[16];
        }

        public SMDHSettings() : this(true)
        {
            for (int i = 0; i < 16; i++)
                AgeRatings[i] = new SMDHAgeRating();
            RegionLock = SMDHRegionLockFlags.RegionFree;
            MatchMakerID = SMDHMatchMakerID.Zero;
            Flags = SMDHSettingFlags.None;
            EULA = SMDHEulaVersion.Zero;
            Reserved = 0;
            OptimalBannerFrame = 0;
            StreetPassID = 0;
        }

        public static SMDHSettings Read(Stream s)
        {
            SMDHSettings sett = new SMDHSettings(true);
            using (var br = new BinaryReader(s, Encoding.ASCII, true))
            {
                for (int i = 0; i < 16; i++)
                    sett.AgeRatings[i] = SMDHAgeRating.Read(s);
                sett.RegionLock = (SMDHRegionLockFlags) br.ReadInt32();
                sett.MatchMakerID = SMDHMatchMakerID.Read(s);
                sett.Flags = (SMDHSettingFlags) br.ReadInt32();
                sett.EULA = SMDHEulaVersion.Read(s);
                sett.Reserved = br.ReadInt16();
                sett.OptimalBannerFrame = br.ReadSingle();
                sett.StreetPassID = br.ReadUInt32();
            }
            return sett;
        }
    }
}