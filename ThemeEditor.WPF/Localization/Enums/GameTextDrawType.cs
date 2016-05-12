// --------------------------------------------------
// 3DS Theme Editor - GameTextDrawType.cs
// --------------------------------------------------

namespace ThemeEditor.WPF.Localization.Enums
{
    public enum GameTextDrawType : uint
    {
        [Order(0)] [DisplayName("Enum_GameText_None", typeof (ThemeResources))] Default = 0,
        [Order(1)] [DisplayName("Enum_GameText_Color", typeof (ThemeResources))] Colored = 1,
        [Order(2)] [DisplayName("Enum_GameText_Hidden", typeof (ThemeResources))] Hidden = 2
    }
}