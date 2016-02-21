using System;

namespace ThemeEditor.Common.SMDH
{
    [Flags]
    public enum RegionLockFlags {
        Japan = 0x01,
        NorthAmerica = 0x02,
        Europe = 0x04,
        Australia = 0x08,
        China = 0x10,
        Korea = 0x20,
        Taiwan = 0x40,

        None = 0x00,
        RegionFree = 0x7fffffff,
    }
}