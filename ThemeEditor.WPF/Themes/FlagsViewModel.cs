// --------------------------------------------------
// 3DS Theme Editor - FlagsViewModel.cs
// --------------------------------------------------

using ThemeEditor.Common.Themes;
using ThemeEditor.WPF.Localization;
using ThemeEditor.WPF.Localization.Enums;

namespace ThemeEditor.WPF.Themes
{
    public class FlagsViewModel : ViewModelBase
    {
        [Order("Theme_Flags_Arrow_Button_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Arrow_Button_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Arrow_Button_Color_Desc", typeof(ThemeResources))]
        public bool ArrowButtonColor
        {
            get { return Model.ArrowButtonColor; }
            set
            {
                var oldValue = Model.ArrowButtonColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.ArrowButtonColor = newValue;
                RaiseViewModelChanged(nameof(ArrowButtonColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Arrow_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Arrow_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Arrow_Color_Desc", typeof(ThemeResources))]
        public bool ArrowColor
        {
            get { return Model.ArrowColor; }
            set
            {
                var oldValue = Model.ArrowColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.ArrowColor = newValue;
                RaiseViewModelChanged(nameof(ArrowColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Background_Music_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Background_Music", typeof(ThemeResources))]
        [Description("Theme_Flags_Background_Music_Desc", typeof(ThemeResources))]
        public bool BackgroundMusic
        {
            get { return Model.BackgroundMusic; }
            set
            {
                var oldValue = Model.BackgroundMusic;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.BackgroundMusic = newValue;
                RaiseViewModelChanged(nameof(BackgroundMusic), oldValue, value);
            }
        }

        [Order("Theme_Flags_Bottom_Background_Inner_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Bottom_Background_Inner_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Bottom_Background_Inner_Color_Desc", typeof(ThemeResources))]
        public bool BottomBackgroundInnerColor
        {
            get { return Model.BottomBackgroundInnerColor; }
            set
            {
                var oldValue = Model.BottomBackgroundInnerColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.BottomBackgroundInnerColor = newValue;
                RaiseViewModelChanged(nameof(BottomBackgroundInnerColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Bottom_Background_Outer_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Bottom_Background_Outer_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Bottom_Background_Outer_Color_Desc", typeof(ThemeResources))]
        public bool BottomBackgroundOuterColor
        {
            get { return Model.BottomBackgroundOuterColor; }
            set
            {
                var oldValue = Model.BottomBackgroundOuterColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.BottomBackgroundOuterColor = newValue;
                RaiseViewModelChanged(nameof(BottomBackgroundOuterColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Bottom_Corner_Button_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Bottom_Corner_Button_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Bottom_Corner_Button_Color_Desc", typeof(ThemeResources))]
        public bool BottomCornerButtonColor
        {
            get { return Model.BottomCornerButtonColor; }
            set
            {
                var oldValue = Model.BottomCornerButtonColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.BottomCornerButtonColor = newValue;
                RaiseViewModelChanged(nameof(BottomCornerButtonColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Bottom_Draw_Type_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Bottom_Draw_Type", typeof(ThemeResources))]
        [Description("Theme_Flags_Bottom_Draw_Type_Desc", typeof(ThemeResources))]
        public BottomDrawType BottomDrawType
        {
            get { return (BottomDrawType) (uint) Model.BottomDrawType; }
            set
            {
                var oldValue = (uint) Model.BottomDrawType;
                var newValue = (uint) value;
                if (oldValue == newValue)
                    return;
                Model.BottomDrawType = (Common.Themes.Enums.BottomDrawType) newValue;
                RaiseViewModelChanged(nameof(BottomDrawType), oldValue, value);
            }
        }

        [Order("Theme_Flags_Bottom_Frame_Type_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Bottom_Frame_Type", typeof(ThemeResources))]
        [Description("Theme_Flags_Bottom_Frame_Type_Desc", typeof(ThemeResources))]
        public BottomFrameType BottomFrameType
        {
            get { return (BottomFrameType) (uint) Model.BottomFrameType; }
            set
            {
                var oldValue = (uint) Model.BottomFrameType;
                var newValue = (uint) value;
                if (oldValue == newValue)
                    return;
                Model.BottomFrameType = (Common.Themes.Enums.BottomFrameType) newValue;
                RaiseViewModelChanged(nameof(BottomFrameType), oldValue, value);
            }
        }

        [Order("Theme_Flags_Cursor_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Cursor_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Cursor_Color_Desc", typeof(ThemeResources))]
        public bool CursorColor
        {
            get { return Model.CursorColor; }
            set
            {
                var oldValue = Model.CursorColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.CursorColor = newValue;
                RaiseViewModelChanged(nameof(CursorColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Demo_Text_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Demo_Text_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Demo_Text_Color_Desc", typeof(ThemeResources))]
        public bool DemoTextColor
        {
            get { return Model.DemoTextColor; }
            set
            {
                var oldValue = Model.DemoTextColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.DemoTextColor = newValue;
                RaiseViewModelChanged(nameof(DemoTextColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_File_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_File_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_File_Color_Desc", typeof(ThemeResources))]
        public bool FileColor
        {
            get { return Model.FileColor; }
            set
            {
                var oldValue = Model.FileColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.FileColor = newValue;
                RaiseViewModelChanged(nameof(FileColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_File_Texture_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_File_Texture", typeof(ThemeResources))]
        [Description("Theme_Flags_File_Texture_Desc", typeof(ThemeResources))]
        public bool FileTexture
        {
            get { return Model.FileTexture; }
            set
            {
                var oldValue = Model.FileTexture;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.FileTexture = newValue;
                RaiseViewModelChanged(nameof(FileTexture), oldValue, value);
            }
        }

        [Order("Theme_Flags_Folder_Arrow_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Folder_Arrow_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Folder_Arrow_Color_Desc", typeof(ThemeResources))]
        public bool FolderArrowColor
        {
            get { return Model.FolderArrowColor; }
            set
            {
                var oldValue = Model.FolderArrowColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.FolderArrowColor = newValue;
                RaiseViewModelChanged(nameof(FolderArrowColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Folder_Background_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Folder_Background_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Folder_Background_Color_Desc", typeof(ThemeResources))]
        public bool FolderBackgroundColor
        {
            get { return Model.FolderBackgroundColor; }
            set
            {
                var oldValue = Model.FolderBackgroundColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.FolderBackgroundColor = newValue;
                RaiseViewModelChanged(nameof(FolderBackgroundColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Folder_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Folder_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Folder_Color_Desc", typeof(ThemeResources))]
        public bool FolderColor
        {
            get { return Model.FolderColor; }
            set
            {
                var oldValue = Model.FolderColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.FolderColor = newValue;
                RaiseViewModelChanged(nameof(FolderColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Folder_Texture_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Folder_Texture", typeof(ThemeResources))]
        [Description("Theme_Flags_Folder_Texture_Desc", typeof(ThemeResources))]
        public bool FolderTexture
        {
            get { return Model.FolderTexture; }
            set
            {
                var oldValue = Model.FolderTexture;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.FolderTexture = newValue;
                RaiseViewModelChanged(nameof(FolderTexture), oldValue, value);
            }
        }

        [Order("Theme_Flags_Game_Text_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Game_Text_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Game_Text_Color_Desc", typeof(ThemeResources))]
        public GameTextDrawType GameTextDrawType
        {
            get { return (GameTextDrawType) (uint) Model.GameTextDrawType; }
            set
            {
                var oldValue = (uint) Model.GameTextDrawType;
                var newValue = (uint) value;
                if (oldValue == newValue)
                    return;
                Model.GameTextDrawType = (Common.Themes.Enums.GameTextDrawType) newValue;
                RaiseViewModelChanged(nameof(GameTextDrawType), oldValue, value);
            }
        }

        private new Flags Model => (Flags) base.Model;

        [Order("Theme_Flags_Open_Close_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Open_Close_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Open_Close_Color_Desc", typeof(ThemeResources))]
        public bool OpenCloseColor
        {
            get { return Model.OpenCloseColor; }
            set
            {
                var oldValue = Model.OpenCloseColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.OpenCloseColor = newValue;
                RaiseViewModelChanged(nameof(OpenCloseColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Sound_Effect_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Sound_Effect", typeof(ThemeResources))]
        [Description("Theme_Flags_Sound_Effect_Desc", typeof(ThemeResources))]
        public bool SoundEffect
        {
            get { return Model.SoundEffect; }
            set
            {
                var oldValue = Model.SoundEffect;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.SoundEffect = newValue;
                RaiseViewModelChanged(nameof(SoundEffect), oldValue, value);
            }
        }

        [Order("Theme_Flags_Top_Background_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Top_Background_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Top_Background_Color_Desc", typeof(ThemeResources))]
        public bool TopBackgroundColor
        {
            get
            {
                var enb = (uint) Model.TopDrawType == (uint) TopDrawType.SolidColor
                          || (uint) Model.TopDrawType == (uint) TopDrawType.SolidColorTexture;
                return enb;
            }
        }

        [Order("Theme_Flags_Top_Corner_Button_Color_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Top_Corner_Button_Color", typeof(ThemeResources))]
        [Description("Theme_Flags_Top_Corner_Button_Color_Desc", typeof(ThemeResources))]
        public bool TopCornerButtonColor
        {
            get { return Model.TopCornerButtonColor; }
            set
            {
                var oldValue = Model.TopCornerButtonColor;
                var newValue = value;
                if (oldValue == newValue)
                    return;
                Model.TopCornerButtonColor = newValue;
                RaiseViewModelChanged(nameof(TopCornerButtonColor), oldValue, value);
            }
        }

        [Order("Theme_Flags_Top_Draw_Type_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Top_Draw_Type", typeof(ThemeResources))]
        [Description("Theme_Flags_Top_Draw_Type_Desc", typeof(ThemeResources))]
        public TopDrawType TopDrawType
        {
            get { return (TopDrawType) (uint) Model.TopDrawType; }
            set
            {
                var oldBgc = TopBackgroundColor;
                var oldValue = (uint) Model.TopDrawType;
                var newValue = (uint) value;
                if (oldValue == newValue)
                    return;
                Model.TopDrawType = (Common.Themes.Enums.TopDrawType) newValue;
                RaiseViewModelChanged(nameof(TopDrawType), oldValue, value);
                RaiseViewModelChanged(nameof(TopBackgroundColor), oldBgc, TopBackgroundColor);
            }
        }

        [Order("Theme_Flags_Top_Frame_Type_Order", typeof(ThemeResources))]
        [DisplayName("Theme_Flags_Top_Frame_Type", typeof(ThemeResources))]
        [Description("Theme_Flags_Top_Frame_Type_Desc", typeof(ThemeResources))]
        public TopFrameType TopFrameType
        {
            get { return (TopFrameType) (int) Model.TopFrameType; }
            set
            {
                var oldValue = (uint) Model.TopFrameType;
                var newValue = (uint) value;
                if (oldValue == newValue)
                    return;
                Model.TopFrameType = (Common.Themes.Enums.TopFrameType) newValue;
                RaiseViewModelChanged(nameof(TopFrameType), oldValue, value);
            }
        }

        public FlagsViewModel(Flags model, string tag) : base(model, tag) {}
    }
}
