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

        public TexturesViewModel(Textures model) : base(model)
        {
            Top = new TextureViewModel(model.Top);
            TopAlt = new TextureViewModel(model.TopExt);
            Bottom = new TextureViewModel(model.Bottom);
            FolderClosed = new TextureViewModel(model.FolderClosed);
            FolderOpen = new TextureViewModel(model.FolderOpen);
            FileLarge = new TextureViewModel(model.FileLarge);
            FileSmall = new TextureViewModel(model.FileSmall);

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
