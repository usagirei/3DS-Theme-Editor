// --------------------------------------------------
// 3DS Theme Editor - DemoTextSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class DemoTextSetViewModel : ViewModelBase
    {
        [Order(0)]
        [DisplayName("Theme_Sets_Demo_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_Demo_Main_Desc", typeof(ThemeResources))]
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

        private new DemoTextSet Model => (DemoTextSet) base.Model;

        [Order(1)]
        [DisplayName("Theme_Sets_Demo_Text", typeof(ThemeResources))]
        [Description("Theme_Sets_Demo_Text_Desc", typeof(ThemeResources))]
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

        public DemoTextSetViewModel(DemoTextSet model) : base(model) {}
    }
}
