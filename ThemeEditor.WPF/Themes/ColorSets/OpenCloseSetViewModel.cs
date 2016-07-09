// --------------------------------------------------
// 3DS Theme Editor - OpenCloseSetViewModel.cs
// --------------------------------------------------

using System;
using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class OpenCloseSetViewModel : ViewModelBase
    {


        private new OpenCloseSet Model => (OpenCloseSet) base.Model;

#if DEBUG

        public Color Shadow
        {
            get { return Model.Shadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.Shadow;
                var newValue = value.ToColorArgb8888();
                if (oldValue == newValue)
                    return;
                Model.Shadow = newValue;
                RaiseViewModelChanged(nameof(Shadow), oldValue, value);
            }
        }

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

#endif

        [Order(7)]
        [Range(-3, 3)]
        [DisplayName("Theme_Sets_OpenClose_ShadowPos", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_ShadowPos_Desc", typeof(ThemeResources))]
        public double ShadowPosition
        {
            get { return Model.TextShadowPos; }
            set
            {
                var oldValue = Model.TextShadowPos;
                var newValue = (float)value;
                if (Math.Abs(oldValue - newValue) < 0.001f)
                    return;
                Model.TextShadowPos = newValue;
                RaiseViewModelChanged(nameof(ShadowPosition), oldValue, value);
            }
        }

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

        [Order(3)]
        [DisplayName("Theme_Sets_OpenClose_Light", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_Light_Desc", typeof(ThemeResources))]
        public Color Light
        {
            get { return Model.Light.ToMediaColor(); }
            set
            {
                var oldValue = Model.Light;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Light = newValue;
                RaiseViewModelChanged(nameof(Light), oldValue, value);
            }
        }





        [Order(6)]
        [DisplayName("Theme_Sets_OpenClose_TextShadow", typeof(ThemeResources))]
        [Description("Theme_Sets_OpenClose_TextShadow_Desc", typeof(ThemeResources))]
        public Color TextShadow
        {
            get { return Model.TextShadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextShadow;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextShadow = newValue;
                RaiseViewModelChanged(nameof(TextShadow), oldValue, value);
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



        public OpenCloseSetViewModel(OpenCloseSet model, string tag) : base(model, tag) { }
    }
}
