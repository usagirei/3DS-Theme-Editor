// --------------------------------------------------
// 3DS Theme Editor - BottomFrameType.cs
// --------------------------------------------------

namespace ThemeEditor.WPF.Localization.Enums
{
    public enum BottomFrameType : uint
    {
        [Order(0)] [DisplayName("Enum_FrameType_Single", typeof(ThemeResources))] Single = 1,
        [Order(1)] [DisplayName("Enum_FrameType_FastScroll", typeof(ThemeResources))] FastScroll = 0,
        [Order(2)] [DisplayName("Enum_FrameType_SlowScroll", typeof(ThemeResources))] SlowScroll = 3,
        [Order(3)] [DisplayName("Enum_FrameType_PageScroll", typeof(ThemeResources))] PageScroll = 2,
        [Order(4)] [DisplayName("Enum_FrameType_BounceScroll", typeof(ThemeResources))] BounceScroll = 4,
    }
}
