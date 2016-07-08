// --------------------------------------------------
// 3DS Theme Editor - BottomCornerSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class BottomCornerSetViewModel : ViewModelBase
    {
        [Order(0)]
        [DisplayName("Theme_Sets_BottomCorner_Border", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomCorner_Border_Desc", typeof(ThemeResources))]
        public Color Border
        {
            get { return Model.BaseDark.ToMediaColor(); }
            set
            {
                var oldValue = Model.BaseDark;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.BaseDark = newValue;
                RaiseViewModelChanged(nameof(Border), oldValue, value);
            }
        }

        [Order(2)]
        [DisplayName("Theme_Sets_BottomCorner_Highlight", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomCorner_Highlight_Desc", typeof(ThemeResources))]
        public Color Highlight
        {
            get { return Model.BaseLight.ToMediaColor(); }
            set
            {
                var oldValue = Model.BaseLight;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.BaseLight = newValue;
                RaiseViewModelChanged(nameof(Highlight), oldValue, value);
            }
        }

        [Order(4)]
        [DisplayName("Theme_Sets_BottomCorner_IconBottom", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomCorner_IconBottom_Desc", typeof(ThemeResources))]
        public Color IconBottom
        {
            get { return Model.IconMain.ToMediaColor(); }
            set
            {
                var oldValue = Model.IconMain;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.IconMain = newValue;
                RaiseViewModelChanged(nameof(IconBottom), oldValue, value);
            }
        }

        [Order(5)]
        [DisplayName("Theme_Sets_BottomCorner_IconTop", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomCorner_IconTop_Desc", typeof(ThemeResources))]
        public Color IconTop
        {
            get { return Model.IconLight.ToMediaColor(); }
            set
            {
                var oldValue = Model.IconLight;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.IconLight = newValue;
                RaiseViewModelChanged(nameof(IconTop), oldValue, value);
            }
        }

        [Order(1)]
        [DisplayName("Theme_Sets_BottomCorner_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomCorner_Main_Desc", typeof(ThemeResources))]
        public Color Main
        {
            get { return Model.BaseMain.ToMediaColor(); }
            set
            {
                var oldValue = Model.BaseMain;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.BaseMain = newValue;
                RaiseViewModelChanged(nameof(Main), oldValue, value);
            }
        }

        private new BottomCorner Model => (BottomCorner) base.Model;

        public BottomCornerSetViewModel(BottomCorner model, string tag) : base(model, tag) { }
    }
}
