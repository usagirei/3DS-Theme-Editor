// --------------------------------------------------
// 3DS Theme Editor - ArrowSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class ArrowSetViewModel : ViewModelBase
    {
        [Order(0)]
        [DisplayName("Theme_Sets_Arrow_Border", typeof(ThemeResources))]
        [Description("Theme_Sets_Arrow_Border_Desc", typeof(ThemeResources))]
        public Color Border
        {
            get { return Model.Border.ToMediaColor(); }
            set
            {
                var oldValue = Model.Border;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Border = newValue;
                RaiseViewModelChanged(nameof(Border), oldValue, value);
            }
        }

        private new ArrowSet Model => (ArrowSet) base.Model;

        [Order(2)]
        [DisplayName("Theme_Sets_Arrow_Pressed", typeof(ThemeResources))]
        [Description("Theme_Sets_Arrow_Pressed_Desc", typeof(ThemeResources))]
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

        [Order(1)]
        [DisplayName("Theme_Sets_Arrow_Unpressed", typeof(ThemeResources))]
        [Description("Theme_Sets_Arrow_Unpressed_Desc", typeof(ThemeResources))]
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

        public ArrowSetViewModel(ArrowSet model) : base(model) {}
    }
}
