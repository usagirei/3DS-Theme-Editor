using System;

namespace ThemeEditor.Common.SMDH
{
    [Flags]
    public enum SMDHSettingFlags
    {
        None = 0x00,
        Visible = 0x0001,
        AutoBoot = 0x002,
        Uses3D = 0x0004,
        LicenseAgreement = 0x0008,
        AutoSave = 0x0010,
        ExtendedBanner = 0x0020,
        AgeRating = 0x0040,
        SaveData = 0x0080,
        RecordStats = 0x0100,
        DisableSaveBackup = 0x0200,
    }
}