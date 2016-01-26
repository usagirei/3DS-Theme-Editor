// --------------------------------------------------
// 3DS Theme Editor - TopSolidSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class TopSolidSetViewModel : ViewModelBase
    {
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

        private new TopBackgroundSet Model => (TopBackgroundSet) base.Model;

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

        public TopSolidSetViewModel(TopBackgroundSet model) : base(model) {}
    }
}
