using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for BottomFolderPreview.xaml
    /// </summary>
    public partial class BottomFolderPreview : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ShowPreviewProperty = DependencyProperty.Register(
                                                                                                    nameof(ShowPreview),
            typeof(bool),
            typeof(BottomFolderPreview),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register(nameof(Theme),
                typeof(ThemeViewModel),
                typeof(BottomFolderPreview),
                new PropertyMetadata(default(ThemeViewModel)));

        private int _cursorColumn;

        private int _cursorRow;
        private int _fileColumn;

        private int _fileRow;
        private int _folderColumn;

        private int _folderRow;

        public int CursorColumn
        {
            get { return _cursorColumn; }
            set
            {
                if (value == _cursorColumn)
                    return;
                _cursorColumn = value;
                OnPropertyChanged(nameof(CursorColumn));
            }
        }

        public int CursorRow
        {
            get { return _cursorRow; }
            set
            {
                if (value == _cursorRow)
                    return;
                _cursorRow = value;
                OnPropertyChanged(nameof(CursorRow));
            }
        }

        public int FileColumn
        {
            get { return _fileColumn; }
            set
            {
                if (value == _fileColumn)
                    return;
                _fileColumn = value;
                OnPropertyChanged(nameof(FileColumn));
            }
        }

        public int FileRow
        {
            get { return _fileRow; }
            set
            {
                if (value == _fileRow)
                    return;
                _fileRow = value;
                OnPropertyChanged(nameof(FileRow));
            }
        }

        public int FolderColumn
        {
            get { return _folderColumn; }
            set
            {
                if (value == _folderColumn)
                    return;
                _folderColumn = value;
                OnPropertyChanged(nameof(FolderColumn));
            }
        }

        public int FolderRow
        {
            get { return _folderRow; }
            set
            {
                if (value == _folderRow)
                    return;
                _folderRow = value;
                OnPropertyChanged(nameof(FolderRow));
            }
        }

        public bool ShowPreview
        {
            get { return (bool) GetValue(ShowPreviewProperty); }
            set { SetValue(ShowPreviewProperty, value); }
        }

        public ThemeViewModel Theme
        {
            get { return (ThemeViewModel) GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static BottomFolderPreview()
        {
            Type ownerType = typeof(BottomFolderPreview);
            IsEnabledProperty.OverrideMetadata(ownerType, new FrameworkPropertyMetadata(false));
        }

        public BottomFolderPreview()
        {
            InitializeComponent();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
