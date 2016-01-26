// --------------------------------------------------
// 3DS Theme Editor - BottomBackgroundSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class BottomBackgroundInnerSetViewModel : ViewModelBase
    {
        [Order(2)]
        [DisplayName("Theme_Sets_BottomInner_Border", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomInner_Border_Desc", typeof(ThemeResources))]
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

        [Order(0)]
        [DisplayName("Theme_Sets_BottomInner_Highlight", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomInner_Highlight_Desc", typeof(ThemeResources))]
        public Color Highlight
        {
            get { return Model.Highlight.ToMediaColor(); }
            set
            {
                var oldValue = Model.Highlight;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Highlight = newValue;
                RaiseViewModelChanged(nameof(Highlight), oldValue, value);
            }
        }

        [Order(1)]
        [DisplayName("Theme_Sets_BottomInner_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomInner_Main_Desc", typeof(ThemeResources))]
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

        private new BottomBackgroundInnerSet Model => (BottomBackgroundInnerSet) base.Model;

        [Order(3)]
        [DisplayName("Theme_Sets_BottomInner_Shadow", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomInner_Shadow_Desc", typeof(ThemeResources))]
        public Color Shadow
        {
            get { return Model.Shadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.Shadow;
                var newValue = value.ToColorRgb8888();
                if (oldValue == newValue)
                    return;
                Model.Shadow = newValue;
                RaiseViewModelChanged(nameof(Shadow), oldValue, value);
            }
        }

        public BottomBackgroundInnerSetViewModel(BottomBackgroundInnerSet model, string tag) : base(model, tag) { }
    }
}
