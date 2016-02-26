// --------------------------------------------------
// 3DS Theme Editor - MainWindow.xaml.cs
// --------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

using ThemeEditor.Common.Graphics;
using ThemeEditor.WPF.Experimental.CWAV;
using ThemeEditor.WPF.Localization;
using ThemeEditor.WPF.Properties;
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

        public ICommand CWavManagerCommand { get; set; }

        public ICommand ExportPreviewCommand { get; }

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

            AboutCommand = new RelayCommand(AboutCommand_Execute);

            CWavManagerCommand
                = new RelayCommand(CWavManager_Execute, CanExecute_ViewModelLoaded);

            ExportPreviewCommand = new RelayCommand<PreviewKind>(ExportPreview_Execute);

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
                {
                    TargetImage.SmallIcon, new List<ImageSize>
                    {
                        new ImageSize(24, 24, RawTexture.DataFormat.Bgr565),
                    }
                },
                {
                    TargetImage.LargeIcon, new List<ImageSize>
                    {
                        new ImageSize(48, 48, RawTexture.DataFormat.Bgr565),
                    }
                },
            };
        }

        private static void OnThemePropertyChangedCallback(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            ((ThemeViewModel) args.OldValue)?.Dispose();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AboutCommand_Execute()
        {
            new AboutWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }.ShowDialog();
        }

        private bool CanExecute_ViewModelLoaded()
        {
            return ViewModel != null;
        }

        private void CWavManager_Execute()
        {
            var block = new CwavBlock();
            bool imported = block.TryImport(ViewModel.CWavBytes);
            if (!imported)
                MessageBox.Show("One or more errors have ocurred while parsing the CWAV Data\n" +
                                "Some data may have been recovered, however some issues may be present.");

            var wnd = new CwavWindow(block)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var dlg = wnd.ShowDialog();
            if (dlg.HasValue && dlg.Value)
            {
                var data = wnd.ViewModel.Generate();
                if (data.Length > 0x2DC00)
                    MessageBox.Show("CWAV Data Too Big\nRe-assign the smaller CWAVs and try again.");
                else
                    ViewModel.CWavBytes = data;
            }
        }

        private void ExportPreview_Execute(PreviewKind previewKind)
        {
            var svfl = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
            };
            switch (previewKind)
            {
                case PreviewKind.Top:
                    svfl.FileName = "preview_top";
                    break;
                case PreviewKind.Bottom:
                    svfl.FileName = "preview_bottom";
                    break;
                case PreviewKind.Both:
                    svfl.FileName = "preview";
                    break;
            }
            var dlg = svfl.ShowDialog();
            if (dlg.HasValue && !dlg.Value)
                return;
            var outPath = svfl.FileName;
            var rendered = RenderPreview(previewKind);
            if (rendered == null)
                return;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rendered));
            using (var fs = File.Open(outPath, FileMode.Create))
                encoder.Save(fs);

            MessageBox.Show(MainResources.Error_PreviewSaved, WINDOW_TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.CheckUpdatesOnStartup)
            {
                bool hasUpdates = await Update.CheckUpdateAvailable();
                if (hasUpdates)
                    MessageBox.Show(MainResources.Error_UpdateAvailable,
                        WINDOW_TITLE,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
            }
        }

        private void PreExecute_SetBusy()
        {
            IsBusy = true;
            BusyText = null;
        }

        private BitmapSource RenderPreview(PreviewKind kind)
        {
            bool wasPreviewing = chk_AnimatePreview.IsChecked.HasValue && chk_AnimatePreview.IsChecked.Value;

            chk_AnimatePreview.IsChecked = false;

            BitmapSource bmp;

            switch (kind)
            {
                case PreviewKind.Top:
                {
                    bmp = RenderVisual(pre_TopScreen, 96, 96);
                    break;
                }
                case PreviewKind.Bottom:
                {
                    bmp = RenderVisual(pre_BottomScreen, 96, 96);
                    break;
                }
                case PreviewKind.Both:
                {
                    var topBmp = RenderVisual(pre_TopScreen, 96, 96);
                    var botBmp = RenderVisual(pre_BottomScreen, 96, 96);

                    DrawingVisual drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        var noiseBrush = FindResource("NoiseBackground") as Brush;
                        drawingContext.DrawRectangle(noiseBrush, null, new Rect(0, 0, 412, 480));
                        drawingContext.DrawImage(topBmp, new Rect(0, 0, 412, 240));
                        const int X_OFF = (412 - 320) / 2;
                        drawingContext.DrawImage(botBmp, new Rect(X_OFF, 240, 320, 240));
                    }

                    var rtbmp = new RenderTargetBitmap(412, 480, 96, 96, PixelFormats.Pbgra32);
                    rtbmp.Render(drawingVisual);
                    bmp = rtbmp;

                    break;
                }
                default:
                    return null;
            }

            chk_AnimatePreview.IsChecked = wasPreviewing;
            bmp.Freeze();
            return bmp;
        }

        private BitmapSource RenderVisual(Visual target, double dpiX, double dpiY)
        {
            if (target == null)
            {
                return null;
            }

            var bounds = VisualTreeHelper.GetDescendantBounds(target);

            var rtb = new RenderTargetBitmap((int) (bounds.Width * dpiX / 96.0),
                (int) (bounds.Height * dpiY / 96.0),
                dpiX,
                dpiY,
                PixelFormats.Pbgra32);

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var vb = new VisualBrush(target);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            return rtb;
        }

        partial void SetupImageCommands();

        partial void SetupThemeCommands();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
