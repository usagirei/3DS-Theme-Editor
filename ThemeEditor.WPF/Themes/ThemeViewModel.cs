// --------------------------------------------------
// 3DS Theme Editor - ThemeViewModel.cs
// --------------------------------------------------

using System;
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

        public byte[] CWavBytes
        {
            get { return Model.CWAV; }
            set { Model.CWAV = value; }
        }

        public ThemeViewModel(Theme model) : base(model, Guid.NewGuid().ToString())
        {
            Flags = new FlagsViewModel(model.Flags, Tag);
            Colors = new ColorsViewModel(model.Colors, Tag);
            Textures = new TexturesViewModel(model.Textures, Tag);
            
            SetupRules();
        }

        public void Save(Stream stream)
        {
            Theme.Write(Model, stream);
        }

        public override void Dispose()
        {   

            Rules.Dispose();
            Flags.Dispose();
            Colors.Dispose();
            Textures.Dispose();

            base.Dispose();
        }

        partial void SetupRules();
    }
}
