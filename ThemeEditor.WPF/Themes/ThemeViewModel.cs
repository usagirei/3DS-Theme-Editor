// --------------------------------------------------
// 3DS Theme Editor - ThemeViewModel.cs
// --------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ThemeEditor.Common.Graphics;
using ThemeEditor.Common.SMDH;
using ThemeEditor.Common.Themes;
using ThemeEditor.WPF.Markup;

namespace ThemeEditor.WPF.Themes
{
    public sealed partial class ThemeViewModel : ViewModelBase
    {
        public ColorsViewModel Colors { get; }

        public FlagsViewModel Flags { get; }
        private new Theme Model => (Theme) base.Model;

        public TexturesViewModel Textures { get; }

        public ThemeInfoViewModel Info { get; set; }

        public byte[] CWavBytes
        {
            get { return Model.CWAV; }
            set { Model.CWAV = value; }
        }

        public ThemeViewModel(Theme model, SMDH info = null) : base(model, Guid.NewGuid().ToString())
        {
            Flags = new FlagsViewModel(model.Flags, Tag);
            Colors = new ColorsViewModel(model.Colors, Tag);
            Textures = new TexturesViewModel(model.Textures, Tag);

            Info = new ThemeInfoViewModel(info ?? new SMDH(), Tag);

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
