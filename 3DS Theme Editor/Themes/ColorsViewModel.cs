// --------------------------------------------------
// 3DS Theme Editor - ColorsViewModel.cs
// --------------------------------------------------

using ThemeEditor.Common.Themes;
using ThemeEditor.WPF.Localization;
using ThemeEditor.WPF.Themes.ColorSets;

namespace ThemeEditor.WPF.Themes
{
    public sealed class ColorsViewModel : ViewModelBase
    {
        [Order("Theme_Colors_Arrow_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Arrow", typeof(ThemeResources))]
        [Description("Theme_Colors_Arrow_Desc", typeof(ThemeResources))]
        public ArrowSetViewModel Arrow { get; }

        [Order("Theme_Colors_Arrow_Button_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Arrow_Button", typeof(ThemeResources))]
        [Description("Theme_Colors_Arrow_Button_Desc", typeof(ThemeResources))]
        public ArrowButtonSetViewModel ArrowButton { get; }

        [Order("Theme_Colors_Bottom_Background_Inner_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Bottom_Background_Inner", typeof(ThemeResources))]
        [Description("Theme_Colors_Bottom_Background_Inner_Desc", typeof(ThemeResources))]
        public BottomBackgroundInnerSetViewModel BottomBackgroundInner { get; }

        [Order("Theme_Colors_Bottom_Background_Outer_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Bottom_Background_Outer", typeof(ThemeResources))]
        [Description("Theme_Colors_Bottom_Background_Outer_Desc", typeof(ThemeResources))]
        public BottomOuterSetViewModel BottomBackgroundOuter { get; }

        [Order("Theme_Colors_Bottom_Corner_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Bottom_Corner", typeof(ThemeResources))]
        [Description("Theme_Colors_Bottom_Corner_Desc", typeof(ThemeResources))]
        public BottomCornerSetViewModel BottomCorner { get; }

        [Order("Theme_Colors_Close_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Close", typeof(ThemeResources))]
        [Description("Theme_Colors_Close_Desc", typeof(ThemeResources))]
        public OpenCloseSetViewModel Close { get; }

        [Order("Theme_Colors_Cursor_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Cursor", typeof(ThemeResources))]
        [Description("Theme_Colors_Cursor_Desc", typeof(ThemeResources))]
        public CursorSetViewModel Cursor { get; }

        [Order("Theme_Colors_Demo_Text_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Demo_Text", typeof(ThemeResources))]
        [Description("Theme_Colors_Demo_Text_Desc", typeof(ThemeResources))]
        public DemoTextSetViewModel DemoText { get; }

        [Order("Theme_Colors_File_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_File", typeof(ThemeResources))]
        [Description("Theme_Colors_File_Desc", typeof(ThemeResources))]
        public FileSetViewModel File { get; }

        [Order("Theme_Colors_Folder_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Folder", typeof(ThemeResources))]
        [Description("Theme_Colors_Folder_Desc", typeof(ThemeResources))]
        public FolderSetViewModel Folder { get; }

        [Order("Theme_Colors_Folder_Arrow_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Folder_Arrow", typeof(ThemeResources))]
        [Description("Theme_Colors_Folder_Arrow_Desc", typeof(ThemeResources))]
        public FolderArrowSetViewModel FolderArrow { get; }

        [Order("Theme_Colors_Folder_Background_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Folder_Background", typeof(ThemeResources))]
        [Description("Theme_Colors_Folder_Background_Desc", typeof(ThemeResources))]
        public BottomBackgroundInnerSetViewModel FolderBackground { get; }

        [Order("Theme_Colors_Game_Text_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Game_Text", typeof(ThemeResources))]
        [Description("Theme_Colors_Game_Text_Desc", typeof(ThemeResources))]
        public GameTextSetViewModel GameText { get; }

        [Order("Theme_Colors_Open_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Open", typeof(ThemeResources))]
        [Description("Theme_Colors_Open_Desc", typeof(ThemeResources))]
        public OpenCloseSetViewModel Open { get; }

        [Order("Theme_Colors_Top_Background_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Top_Background", typeof(ThemeResources))]
        [Description("Theme_Colors_Top_Background_Desc", typeof(ThemeResources))]
        public TopSolidSetViewModel TopBackground { get; }

        [Order("Theme_Colors_Top_Corner_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Colors_Top_Corner", typeof(ThemeResources))]
        [Description("Theme_Colors_Top_Corner_Desc", typeof(ThemeResources))]
        public TopCornerSetViewModel TopCorner { get; }

        public ColorsViewModel(Colors colors, string tag) : base(colors, tag)
        {
            Arrow = new ArrowSetViewModel(colors.Arrow, tag);
            ArrowButton = new ArrowButtonSetViewModel(colors.ArrowButton, tag);
            BottomCorner = new BottomCornerSetViewModel(colors.BottomCornerButton, tag);
            BottomBackgroundOuter = new BottomOuterSetViewModel(colors.BottomBackgroundOuter, tag);
            BottomBackgroundInner = new BottomBackgroundInnerSetViewModel(colors.BottomBackgroundInner, tag);
            Close = new OpenCloseSetViewModel(colors.Close, tag);
            Cursor = new CursorSetViewModel(colors.Cursor, tag);
            DemoText = new DemoTextSetViewModel(colors.DemoText, tag);
            File = new FileSetViewModel(colors.File, tag);
            Folder = new FolderSetViewModel(colors.Folder, tag);
            FolderArrow = new FolderArrowSetViewModel(colors.FolderArrow, tag);
            FolderBackground = new BottomBackgroundInnerSetViewModel(colors.FolderBackground, tag);
            GameText = new GameTextSetViewModel(colors.GameText, tag);
            Open = new OpenCloseSetViewModel(colors.Open, tag);
            TopCorner = new TopCornerSetViewModel(colors.TopCornerButton, tag);
            TopBackground = new TopSolidSetViewModel(colors.TopBackground, tag);
        }

        public override void Dispose()
        {
            Arrow.Dispose();
            ArrowButton.Dispose();
            BottomCorner.Dispose();
            BottomBackgroundOuter.Dispose();
            BottomBackgroundInner.Dispose();
            Close.Dispose();
            Cursor.Dispose();
            DemoText.Dispose();
            File.Dispose();
            Folder.Dispose();
            FolderArrow.Dispose();
            FolderBackground.Dispose();
            GameText.Dispose();
            Open.Dispose();
            TopCorner.Dispose();
            TopBackground.Dispose();


            base.Dispose();
        }
    }
}
