// --------------------------------------------------
// 3DS Theme Editor - MainWindow.xaml.cs
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using ThemeEditor.Common.Graphics;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static Dictionary<TargetImage, List<ImageSize>> _validImageSizes;

        public static readonly DependencyProperty ViewModelProperty
            = DependencyProperty.Register(nameof(ViewModel),
                typeof(ThemeViewModel),
                typeof(MainWindow),
                new PropertyMetadata(default(ThemeViewModel), OnThemePropertyChangedCallback));

        private static void OnThemePropertyChangedCallback(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            ((ThemeViewModel)args.OldValue)?.Dispose();
        }

        private string _busyText;

        private bool _isBusy;

        public ICommand AboutCommand { get; }

        public string BusyText
        {
            get { return _busyText; }
            set
            {
                _busyText = value;
                OnPropertyChanged(nameof(BusyText));
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == _isBusy)
                    return;
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ThemeViewModel ViewModel
        {
            get { return (ThemeViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        static MainWindow()
        {
            InitValidSizes();
        }

        public MainWindow()
        {
            InitializeComponent();

            AboutCommand =
                new RelayCommand(
                    () => new AboutWindow
                    {
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    }.ShowDialog());

            SetupThemeCommands();
            SetupImageCommands();

            DataContext = this;
        }

        private static void InitValidSizes()
        {
            _validImageSizes = new Dictionary<TargetImage, List<ImageSize>>
            {
                {
                    TargetImage.Top, new List<ImageSize>
                    {
                        new ImageSize(1024, 256, RawTexture.DataFormat.Bgr565),
                        new ImageSize(512, 256, RawTexture.DataFormat.Bgr565),
                        new ImageSize(64, 64, RawTexture.DataFormat.A8),
                    }
                },
                {
                    TargetImage.Bottom, new List<ImageSize>
                    {
                        new ImageSize(1024, 256, RawTexture.DataFormat.Bgr565),
                        new ImageSize(512, 256, RawTexture.DataFormat.Bgr565),
                    }
                },
                {
                    TargetImage.TopAlt, new List<ImageSize>
                    {
                        new ImageSize(64, 64, RawTexture.DataFormat.A8),
                    }
                },
                {
                    TargetImage.FileSmall, new List<ImageSize>
                    {
                        new ImageSize(32, 64, RawTexture.DataFormat.Bgr888),
                    }
                },
                {
                    TargetImage.FileLarge, new List<ImageSize>
                    {
                        new ImageSize(64, 128, RawTexture.DataFormat.Bgr888),
                    }
                },
                {
                    TargetImage.FolderClosed, new List<ImageSize>
                    {
                        new ImageSize(128, 64, RawTexture.DataFormat.Bgr888),
                    }
                },
                {
                    TargetImage.FolderOpen, new List<ImageSize>
                    {
                        new ImageSize(128, 64, RawTexture.DataFormat.Bgr888),
                    }
                },
            };
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanExecute_ViewModelLoaded()
        {
            return ViewModel != null;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {}

        private void PreExecute_SetBusy()
        {
            IsBusy = true;
            BusyText = null;
        }

        partial void SetupImageCommands();

        partial void SetupThemeCommands();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
