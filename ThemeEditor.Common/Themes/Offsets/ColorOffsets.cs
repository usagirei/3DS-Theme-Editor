// --------------------------------------------------
// ThemeEditor.Common - ColorOffsets.cs
// --------------------------------------------------

namespace ThemeEditor.Common.Themes.Offsets
{
    internal struct ColorOffsets
    {
        public uint Cursor;
        public uint Folder;
        public uint File;
        public uint ArrowButton;
        public uint Arrow;
        public uint Open;
        public uint Close;
        public uint GameText;
        public uint BottomSolid;
        public uint BottomOuter;
        public uint FolderBackground;
        public uint FolderArrow;
        public uint BottomCornerButton;
        public uint TopCornerButton;
        public uint DemoText;
        public uint TopBackground;

        public void Offset(int offset)
        {
            Cursor = (uint) (Cursor + offset);
            Folder = (uint) (Folder + offset);
            File = (uint) (File + offset);
            ArrowButton = (uint) (ArrowButton + offset);
            Arrow = (uint) (Arrow + offset);
            Open = (uint) (Open + offset);
            Close = (uint) (Close + offset);
            GameText = (uint) (GameText + offset);
            BottomSolid = (uint) (BottomSolid + offset);
            BottomOuter = (uint) (BottomOuter + offset);
            FolderBackground = (uint) (FolderBackground + offset);
            FolderArrow = (uint) (FolderArrow + offset);
            BottomCornerButton = (uint) (BottomCornerButton + offset);
            TopCornerButton = (uint) (TopCornerButton + offset);
            DemoText = (uint) (DemoText + offset);
            TopBackground = (uint) (TopBackground + offset);
        }
    }
}
