// --------------------------------------------------
// 3DS Theme Editor - FolderArrowSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class FolderArrowSetViewModel : ViewModelBase
    {
        [Order(6)]
        [DisplayName("Theme_Sets_FolderArrow_ArrowShadow", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_ArrowShadow_Desc", typeof(ThemeResources))]
        public Color ArrowGlow

        {
            get { return Model.ArrowShadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.ArrowShadow;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.ArrowShadow = newValue;
                RaiseViewModelChanged(nameof(ArrowGlow), oldValue, value);
            }
        }

        [Order(8)]
        [DisplayName("Theme_Sets_FolderArrow_ArrowPressed", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_ArrowPressed_Desc", typeof(ThemeResources))]
        public Color ArrowPressed

        {
            get { return Model.ArrowSelected.ToMediaColor(); }
            set
            {
                var oldValue = Model.ArrowSelected;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.ArrowSelected = newValue;
                RaiseViewModelChanged(nameof(ArrowPressed), oldValue, value);
            }
        }

        [Order(7)]
        [DisplayName("Theme_Sets_FolderArrow_ArrowUnpressed", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_ArrowÚnpressed_Desc", typeof(ThemeResources))]
        public Color ArrowUnpressed

        {
            get { return Model.ArrowMain.ToMediaColor(); }
            set
            {
                var oldValue = Model.ArrowMain;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.ArrowMain = newValue;
                RaiseViewModelChanged(nameof(ArrowUnpressed), oldValue, value);
            }
        }

        [Order(4)]
        [DisplayName("Theme_Sets_FolderArrow_Glow", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_Glow_Desc", typeof(ThemeResources))]
        public Color Glow
        {
            get { return Model.Shadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.Shadow;
                var newValue = value.ToColorArgb8888();
                if (oldValue == newValue)
                    return;
                Model.Shadow = newValue;
                RaiseViewModelChanged(nameof(Glow), oldValue, value);
            }
        }

        [Order(3)]
        [DisplayName("Theme_Sets_FolderArrow_Highlight", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_Highlight_Desc", typeof(ThemeResources))]
        public Color Highlight
        {
            get { return Model.Light.ToMediaColor(); }
            set
            {
                var oldValue = Model.Light;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Light = newValue;
                RaiseViewModelChanged(nameof(Highlight), oldValue, value);
            }
        }

        [Order(2)]
        [DisplayName("Theme_Sets_FolderArrow_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_Main_Desc", typeof(ThemeResources))]
        public Color Main
        {
            get { return Model.Main.ToMediaColor(); }
            set
            {
                var oldValue = Model.Main;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Main = newValue;
                RaiseViewModelChanged(nameof(Main), oldValue, value);
            }
        }

        private new FolderArrowSet Model => (FolderArrowSet) base.Model;

        [Order(1)]
        [DisplayName("Theme_Sets_FolderArrow_Shading", typeof(ThemeResources))]
        [Description("Theme_Sets_FolderArrow_Shading_Desc", typeof(ThemeResources))]
        public Color Shading
        {
            get { return Model.Dark.ToMediaColor(); }
            set
            {
                var oldValue = Model.Dark;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Dark = newValue;
                RaiseViewModelChanged(nameof(Shading), oldValue, value);
            }
        }

        public FolderArrowSetViewModel(FolderArrowSet model, string tag) : base(model, tag) { }
    }
}
