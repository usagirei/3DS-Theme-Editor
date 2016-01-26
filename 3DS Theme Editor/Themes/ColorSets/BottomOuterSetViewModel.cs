// --------------------------------------------------
// 3DS Theme Editor - BottomOuterSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class BottomOuterSetViewModel : ViewModelBase
    {
        [Order(2)]
        [DisplayName("Theme_Sets_BottomOuter_Glow", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomOuter_Glow_Desc", typeof(ThemeResources))]
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
        [DisplayName("Theme_Sets_BottomOuter_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomOuter_Main_Desc", typeof(ThemeResources))]
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

        private new BottomBackgroundOuterSet Model => (BottomBackgroundOuterSet) base.Model;

        [Order(0)]
        [DisplayName("Theme_Sets_BottomOuter_Stripe", typeof(ThemeResources))]
        [Description("Theme_Sets_BottomOuter_Stripe_Desc", typeof(ThemeResources))]
        public Color Stripe
        {
            get { return Model.Stripe.ToMediaColor(); }
            set
            {
                var oldValue = Model.Stripe;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Stripe = newValue;
                RaiseViewModelChanged(nameof(Stripe), oldValue, value);
            }
        }

        public BottomOuterSetViewModel(BottomBackgroundOuterSet model) : base(model) {}
    }
}
