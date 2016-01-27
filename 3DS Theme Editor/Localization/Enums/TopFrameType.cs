// --------------------------------------------------
// 3DS Theme Editor - TopFrameType.cs
// --------------------------------------------------

namespace ThemeEditor.WPF.Localization.Enums
{
    public enum TopFrameType : uint
    {
        [Order(0)] [DisplayName("Enum_FrameType_Single", typeof(ThemeResources))] Single = 1,
        [Order(1)] [DisplayName("Enum_FrameType_FastScroll", typeof(ThemeResources))] FastScroll = 0,
        [Order(2)] [DisplayName("Enum_FrameType_SlowScroll", typeof(ThemeResources))] SlowScroll = 3,
        [Visible(false)] Invalid = 2
    }
}
