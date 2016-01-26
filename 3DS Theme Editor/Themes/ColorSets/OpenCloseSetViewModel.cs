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
            get { return Model.Glow.ToMediaColor(); }
            set
            {
                var oldValue = Model.Glow;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Glow = newValue;
                RaiseViewModelChanged(nameof(Glow), oldValue, value);
            }
        }

        private new OpenCloseSet Model => (OpenCloseSet) base.Model;

        [Order(1)]
        [DisplayName("Theme_Sets_OpenClose_Pressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_Pressed_Desc", typeof(ThemeResources))]
        public Color Pressed
        {
            get { return Model.Pressed.ToMediaColor(); }
            set
            {
                var oldValue = Model.Pressed;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Pressed = newValue;
                RaiseViewModelChanged(nameof(Pressed), oldValue, value);
            }
        }

        [Order(6)]
        [DisplayName("Theme_Sets_OpenClose_TextGlow", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextGlow_Desc", typeof(ThemeResources))]
        public Color TextGlow
        {
            get { return Model.TextGlow.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextGlow;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextGlow = newValue;
                RaiseViewModelChanged(nameof(TextGlow), oldValue, value);
            }
        }

        [Order(8)]
        [DisplayName("Theme_Sets_OpenClose_TextPressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextPressed_Desc", typeof(ThemeResources))]
        public Color TextPressed
        {
            get { return Model.TextPressed.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextPressed;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextPressed = newValue;
                RaiseViewModelChanged(nameof(TextPressed), oldValue, value);
            }
        }

        [Order(7)]
        [DisplayName("Theme_Sets_OpenClose_TextUnpressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextUnpressed_Desc", typeof(ThemeResources))]
        public Color TextUnpressed

        {
            get { return Model.TextUnpressed.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextUnpressed;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextUnpressed = newValue;
                RaiseViewModelChanged(nameof(TextUnpressed), oldValue, value);
            }
        }

        [Order(2)]
        [DisplayName("Theme_Sets_OpenClose_Unpressed", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_Unpressed_Desc", typeof(ThemeResources))]
        public Color Unpressed
        {
            get { return Model.Unpressed.ToMediaColor(); }
            set
            {
                var oldValue = Model.Unpressed;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Unpressed = newValue;
                RaiseViewModelChanged(nameof(Unpressed), oldValue, value);
            }
        }

        public OpenCloseSetViewModel(OpenCloseSet model) : base(model) {}
    }
}
