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
        [DisplayName("Theme_Sets_Game_TextMain", typeof(ThemeResources))]
        [Description("Theme_Sets_Game_TextMain_Desc", typeof(ThemeResources))]
        public Color Text
        {
            get { return Model.TextMain.ToMediaColor(); }
            set
            {
                var oldValue = Model.TextMain;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.TextMain = newValue;
                RaiseViewModelChanged(nameof(Text), oldValue, value);
            }
        }

        [Order(3)]
        [DisplayName("Theme_Sets_Game_Light", typeof(ThemeResources))]
        [Description("Theme_Sets_Game_Light_Desc", typeof(ThemeResources))]
        public Color Light
        {
            get { return Model.Light.ToMediaColor(); }
            set
            {
                var oldValue = Model.Light;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Light = newValue;
                RaiseViewModelChanged(nameof(Light), oldValue, value);
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

        public GameTextSetViewModel(GameTextSet model, string tag) : base(model, tag) { }
    }
}
