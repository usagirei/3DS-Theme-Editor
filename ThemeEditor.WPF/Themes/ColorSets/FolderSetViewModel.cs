// --------------------------------------------------
// 3DS Theme Editor - FolderSetViewModel.cs
// --------------------------------------------------

using System.Windows.Media;

using ThemeEditor.Common.Themes.ColorSets;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes.ColorSets
{
    public sealed class FolderSetViewModel : ViewModelBase
    {
        private new FolderSet Model => (FolderSet) base.Model;


        [Order(2)]
        [DisplayName("Theme_Sets_Folder_Light", typeof(ThemeResources))]
        [Description("Theme_Sets_Folder_Light_Desc", typeof(ThemeResources))]
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

        [Order(3)]
        [DisplayName("Theme_Sets_Folder_Shadow", typeof(ThemeResources))]
        [Description("Theme_Sets_Folder_Shadow_Desc", typeof(ThemeResources))]
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

        [Order(0)]
        [DisplayName("Theme_Sets_Folder_Main", typeof(ThemeResources))]
        [Description("Theme_Sets_Folder_Main_Desc", typeof(ThemeResources))]
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

        [Order(1)]
        [DisplayName("Theme_Sets_Folder_Shading", typeof(ThemeResources))]
        [Description("Theme_Sets_Folder_Shading_Desc", typeof(ThemeResources))]
        public Color Dark
        {
            get { return Model.Dark.ToMediaColor(); }
            set
            {
                var oldValue = Model.Dark;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Dark = newValue;
                RaiseViewModelChanged(nameof(Dark), oldValue, value);
            }
        }

        public FolderSetViewModel(FolderSet model, string tag) : base(model, tag) { }
    }
}
