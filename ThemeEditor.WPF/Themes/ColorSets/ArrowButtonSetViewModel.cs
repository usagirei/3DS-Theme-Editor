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

#endif

        public ArrowButtonSetViewModel(ArrowButtonSet model, string tag) : base(model, tag) {}
    }
}
