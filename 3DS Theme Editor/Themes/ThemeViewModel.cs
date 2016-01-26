// --------------------------------------------------
// 3DS Theme Editor - ThemeViewModel.cs
// --------------------------------------------------

using System.IO;

using ThemeEditor.Common.Themes;

namespace ThemeEditor.WPF.Themes
{
    public sealed partial class ThemeViewModel : ViewModelBase
    {
        public ColorsViewModel Colors { get; }

        public FlagsViewModel Flags { get; }
        private new Theme Model => (Theme) base.Model;

        public TexturesViewModel Textures { get; }

        public ThemeViewModel(Theme model) : base(model)
        {
            Flags = new FlagsViewModel(model.Flags);
            Colors = new ColorsViewModel(model.Colors);
            Textures = new TexturesViewModel(model.Textures);

            SetupRules();
        }

        public void Save(Stream stream)
        {
            Theme.Write(Model, stream);
        }

        partial void SetupRules();
    }
}
