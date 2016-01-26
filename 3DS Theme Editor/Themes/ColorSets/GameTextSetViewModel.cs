// --------------------------------------------------
// 3DS Theme Editor - GameTextSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class GameTextSetViewModel : ViewModelBase
    {
        [Order(0)]
        [DisplayName("Theme_Sets_Game_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_Game_Main_Desc", typeof(ThemeResources))]
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

        private new GameTextSet Model => (GameTextSet) base.Model;

        [Order(1)]
        [DisplayName("Theme_Sets_Game_Text", typeof(ThemeResources))]
        [Description("Theme_Sets_Game_Text_Desc", typeof(ThemeResources))]
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

        public GameTextSetViewModel(GameTextSet model) : base(model) {}
    }
}
