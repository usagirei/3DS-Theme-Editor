// --------------------------------------------------
// 3DS Theme Editor - OpenCloseSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class OpenCloseSetViewModel : ViewModelBase
    {
        [Order(3)]
        [DisplayName("Theme_Sets_OpenClose_Glow", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_Glow_Desc", typeof(ThemeResources))]
        public Color Glow
        {
            get { return Model.Light.ToMediaColor(); }
            set
            {
                var oldValue = Model.Light;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Light = newValue;
                RaiseViewModelChanged(nameof(Glow), oldValue, value);
            }
        }

        private new OpenCloseSet Model => (OpenCloseSet) base.Model;

        [Order(1)]
        [DisplayName("Theme_Sets_OpenClose_Pressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_Pressed_Desc", typeof(ThemeResources))]
        public Color Pressed
        {
            get { return Model.Dark.ToMediaColor(); }
            set
            {
                var oldValue = Model.Dark;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Dark = newValue;
                RaiseViewModelChanged(nameof(Pressed), oldValue, value);
            }
        }

        [Order(6)]
        [DisplayName("Theme_Sets_OpenClose_TextGlow", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextGlow_Desc", typeof(ThemeResources))]
        public Color TextGlow
        {
            get { return Model.TextShadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextShadow;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextShadow = newValue;
                RaiseViewModelChanged(nameof(TextGlow), oldValue, value);
            }
        }

        [Order(8)]
        [DisplayName("Theme_Sets_OpenClose_TextPressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextPressed_Desc", typeof(ThemeResources))]
        public Color TextPressed
        {
            get { return Model.TextSelected.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextSelected;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextSelected = newValue;
                RaiseViewModelChanged(nameof(TextPressed), oldValue, value);
            }
        }

        [Order(7)]
        [DisplayName("Theme_Sets_OpenClose_TextUnpressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextUnpressed_Desc", typeof(ThemeResources))]
        public Color TextUnpressed

        {
            get { return Model.TextMain.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextMain;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextMain = newValue;
                RaiseViewModelChanged(nameof(TextUnpressed), oldValue, value);
            }
        }

        [Order(2)]
        [DisplayName("Theme_Sets_OpenClose_Unpressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_Unpressed_Desc", typeof(ThemeResources))]
        public Color Unpressed
        {
            get { return Model.Main.ToMediaColor(); }
            set
            {
                var oldValue = Model.Main;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Main = newValue;
                RaiseViewModelChanged(nameof(Unpressed), oldValue, value);
            }
        }

        public OpenCloseSetViewModel(OpenCloseSet model, string tag) : base(model, tag) { }
    }
}
