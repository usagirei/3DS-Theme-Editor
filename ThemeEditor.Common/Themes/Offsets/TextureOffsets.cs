// --------------------------------------------------
// ThemeEditor.Common - TextureOffsets.cs
// --------------------------------------------------

namespace ThemeEditor.Common.Themes.Offsets
{
    internal struct TextureOffsets
    {
        public uint Top;
        public uint TopExt;
        public uint Bottom;
        public uint FolderClosed;
        public uint FolderOpen;
        public uint FileLarge;
        public uint FileSmall;

        public void Offset(int offset)
        {
            Top = (uint) (Top + offset);
            TopExt = (uint) (TopExt + offset);
            Bottom = (uint) (Bottom + offset);
            FolderClosed = (uint) (FolderClosed + offset);
            FolderOpen = (uint) (FolderOpen + offset);
            FileLarge = (uint) (FileLarge + offset);
            FileSmall = (uint) (FileSmall + offset);
        }
    }
}
