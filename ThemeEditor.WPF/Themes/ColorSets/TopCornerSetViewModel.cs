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

        private new TopCornerSet Model => (TopCornerSet)base.Model;

#if DEBUG

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

        public Color Shadow
        {
            get { return Model.Shadow.ToMediaColor(); }
            set
            {
                var oldValue = Model.Shadow;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Shadow = newValue;
                RaiseViewModelChanged(nameof(Shadow), oldValue, value);
            }
        }

#endif

        [Order(2)]
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

        

        [Order(3)]
        [DisplayName("Theme_Sets_TopCorner_Text", typeof(ThemeResources))]
        [Description("Theme_Sets_TopCorner_Text_Desc", typeof(ThemeResources))]
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


        public TopCornerSetViewModel(TopCornerSet model, string tag) : base(model, tag) { }
    }
}
