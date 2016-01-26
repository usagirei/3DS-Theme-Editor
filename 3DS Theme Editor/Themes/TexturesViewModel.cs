// --------------------------------------------------
// 3DS Theme Editor - TexturesViewModel.cs
// --------------------------------------------------

using ThemeEditor.Common.Themes;

namespace ThemeEditor.WPF.Themes
{
    public sealed class TexturesViewModel : ViewModelBase
    {
        public TextureViewModel Bottom { get; }
        public TextureViewModel FileLarge { get; }
        public TextureViewModel FileSmall { get; }
        public TextureViewModel FolderClosed { get; }
        public TextureViewModel FolderOpen { get; }
        public TextureViewModel Top { get; }
        public TextureViewModel TopAlt { get; }

        public override void Dispose()
        {
            Bottom.Dispose();
            FileLarge.Dispose();
            FileSmall.Dispose();
            FolderClosed.Dispose();
            FolderOpen.Dispose();
            Top.Dispose();
            TopAlt.Dispose();

            base.Dispose();
        }

        public TexturesViewModel(Textures model, string tag) : base(model, tag)
        {
            Top = new TextureViewModel(model.Top, tag);
            TopAlt = new TextureViewModel(model.TopExt, tag);
            Bottom = new TextureViewModel(model.Bottom, tag);
            FolderClosed = new TextureViewModel(model.FolderClosed, tag);
            FolderOpen = new TextureViewModel(model.FolderOpen, tag);
            FileLarge = new TextureViewModel(model.FileLarge, tag);
            FileSmall = new TextureViewModel(model.FileSmall, tag);

            Top.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(Top), null, null);
            };
            TopAlt.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(TopAlt), null, null);
            };
            Bottom.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(Bottom), null, null);
            };
            FolderClosed.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(FolderClosed), null, null);
            };
            FolderOpen.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(FolderOpen), null, null);
            };
            FileLarge.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(FileLarge), null, null);
            };
            FileSmall.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TextureViewModel.Bitmap))
                    RaiseViewModelChanged(nameof(FileSmall), null, null);
            };
        }
    }
}
