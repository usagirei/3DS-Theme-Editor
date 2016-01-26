// --------------------------------------------------
// 3DS Theme Editor - TopCornerSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class TopCornerSetViewModel : ViewModelBase
    {
        [Order(0)]
        [DisplayName("Theme_Sets_TopCorner_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_TopCorner_Main_Desc", typeof(ThemeResources))]
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

        private new TopCornerSet Model => (TopCornerSet) base.Model;

        [Order(3)]
        [DisplayName("Theme_Sets_TopCorner_Text", typeof(ThemeResources))]
        [Description("Theme_Sets_TopCorner_Text_Desc", typeof(ThemeResources))]
        public Color Text
        {
            get { return Model.Text.ToMediaColor(); }
            set
            {
                var oldValue = Model.Text;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Text = newValue;
                RaiseViewModelChanged(nameof(Text), oldValue, value);
            }
        }

        public TopCornerSetViewModel(TopCornerSet model) : base(model) {}
    }
}
