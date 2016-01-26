// --------------------------------------------------
// 3DS Theme Editor - TopDrawType.cs
// --------------------------------------------------

namespace ThemeEditor.WPF.Localization.Enums
{
    public enum TopDrawType : uint
    {
        [Order(0)] [DisplayName("Enum_DrawType_None", typeof(ThemeResources))] None = 0,
        [Order(1)] [DisplayName("Enum_DrawType_Texture", typeof(ThemeResources))] Texture = 3,
        [Order(2)] [DisplayName("Enum_DrawType_SolidColor", typeof(ThemeResources))] SolidColor = 1,
        [Order(3)] [DisplayName("Enum_DrawType_SolidColorTexture", typeof(ThemeResources))] SolidColorTexture = 2,
    }
}
