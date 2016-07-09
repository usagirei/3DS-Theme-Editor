// --------------------------------------------------
// 3DS Theme Editor - TopSolidSetViewModel.cs
// --------------------------------------------------

using System;
using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class TopSolidSetViewModel : ViewModelBase
    {
        private const float EPSILON = 1 / 255f;
        private new TopBackgroundSet Model => (TopBackgroundSet) base.Model;

        [Order(0)]
        [DisplayName("Theme_Sets_Top_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_Top_Main_Desc", typeof(ThemeResources))]
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

        [Order(1)]
        [DisplayName("Theme_Sets_Top_Gradient", typeof(ThemeResources))]
        [Description("Theme_Sets_Top_Gradient_Desc", typeof(ThemeResources))]
        public double Gradient
        {
            get { return Model.Gradient / 255.0; }
            set
            {
                var oldValue = Model.Gradient;
                var newValue = (byte) (value * 255).Clamp(0, 255);
                if (oldValue == newValue)
                    return;
                Model.Gradient = newValue;
                RaiseViewModelChanged(nameof(Gradient), oldValue, value);
            }
        }


        [Order(2)]
        [DisplayName("Theme_Sets_Top_Opacity", typeof(ThemeResources))]
        [Description("Theme_Sets_Top_Opacity_Desc", typeof(ThemeResources))]
        public double TextureOpacity
        {
            get { return Model.TextureOpacity / 255.0; }
            set
            {
                var oldValue = Model.TextureOpacity;
                var newValue = (byte) (value * 255).Clamp(0, 255);
                if (oldValue == newValue)
                    return;
                Model.TextureOpacity = newValue;
                RaiseViewModelChanged(nameof(TextureOpacity), oldValue, value);
            }
        }

        [Order(3)]
        [DisplayName("Theme_Sets_Top_AltOpacity", typeof(ThemeResources))]
        [Description("Theme_Sets_Top_AltOpacity_Desc", typeof(ThemeResources))]
        public double AlternateOpacity
        {
            get { return Model.AlternateOpacity / 255.0; }
            set
            {
                var oldValue = Model.AlternateOpacity;
                var newValue = (byte)(value * 255).Clamp(0, 255);
                if (Math.Abs(oldValue - newValue) < EPSILON)
                    return;
                Model.AlternateOpacity = newValue;
                RaiseViewModelChanged(nameof(AlternateOpacity), oldValue, value);
            }
        }

        [Order(4)]
        [DisplayName("Theme_Sets_Top_FadeToValue", typeof(ThemeResources))]
        [Description("Theme_Sets_Top_FadeToValue_Desc", typeof(ThemeResources))]
        public double GradientColor
        {
            get { return Model.GradientColor / 255.0; }
            set
            {
                var oldValue = Model.GradientColor;
                var newValue = (byte)(value * 255).Clamp(0, 255);
                if (Math.Abs(oldValue - newValue) < EPSILON)
                    return;
                Model.GradientColor = newValue;
                RaiseViewModelChanged(nameof(GradientColor), oldValue, value);
            }
        }

        public TopSolidSetViewModel(TopBackgroundSet model, string tag) : base(model, tag) { }
    }
}
