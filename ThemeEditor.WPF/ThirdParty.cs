using System;
using System.IO;
using System.Linq;

namespace ThemeEditor.WPF
{
    internal static class ThirdPartyTools
    {
        public static ThirdPartyTool CtrWaveConveter { get; }

        public static ThirdPartyTool VgmStream { get; }

        static ThirdPartyTools()
        {
            VgmStream = new ThirdPartyTool
            {
                Path = new[]
                {
                    Environment.CurrentDirectory,
                    "ThirdParty",
                    "vgmstream",
                    "test.exe"
                }.Aggregate(Path.Combine)
            };

            CtrWaveConveter = new ThirdPartyTool
            {
                Path = new[]
                {
                    Environment.CurrentDirectory,
                    "ThirdParty",
                    "ctr_WaveConverter32",
                    "ctr_WaveConverter32.exe"
                }.Aggregate(Path.Combine)
            };
        }

        public class ThirdPartyTool
        {
            public string Path { get; set; }
            public bool Present => File.Exists(Path);
        }
    }
}
