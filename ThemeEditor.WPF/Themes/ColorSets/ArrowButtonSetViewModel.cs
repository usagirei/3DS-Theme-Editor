// --------------------------------------------------
// 3DS Theme Editor - ArrowButtonSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class ArrowButtonSetViewModel : ViewModelBase
    {
        [Order(2)]
        [DisplayName("Theme_Sets_ArrowButton_Glow", typeof(ThemeResources))]
        [Description("Theme_Sets_ArrowButton_Glow_Desc", typeof(ThemeResources))]
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

        [Order(1)]
        [DisplayName("Theme_Sets_ArrowButton_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_ArrowButton_Main_Desc", typeof(ThemeResources))]
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

        private new ArrowButtonSet Model => (ArrowButtonSet) base.Model;

        [Order(0)]
        [DisplayName("Theme_Sets_ArrowButton_Shading", typeof(ThemeResources))]
        [Description("Theme_Sets_ArrowButton_Shading_Desc", typeof(ThemeResources))]
        public Color Shading
        {
            get { return Model.Shading.ToMediaColor(); }
            set
            {
                var oldValue = Model.Shading;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Shading = newValue;
                RaiseViewModelChanged(nameof(Shading), oldValue, value);
            }
        }

        public ArrowButtonSetViewModel(ArrowButtonSet model, string tag) : base(model, tag) {}
    }
}
