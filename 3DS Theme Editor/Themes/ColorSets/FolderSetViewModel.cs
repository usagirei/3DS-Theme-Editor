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
        [Order(1)]
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

        private new FolderSet Model => (FolderSet) base.Model;

        [Order(0)]
        [DisplayName("Theme_Sets_Folder_Shading", typeof(ThemeResources))]
        [Description("Theme_Sets_Folder_Shading_Desc", typeof(ThemeResources))]
        public Color Shading
        {
            get { return Model.Shading.ToMediaColor(); }
            set
            {
                var oldValue = Model.Shading;
                var newValue = value.ToColorRgb888();
                if (oldValue == newValue)
                    return;
                Model.Shading = newValue;
                RaiseViewModelChanged(nameof(Shading), oldValue, value);
            }
        }

        public FolderSetViewModel(FolderSet model) : base(model) {}
    }
}
