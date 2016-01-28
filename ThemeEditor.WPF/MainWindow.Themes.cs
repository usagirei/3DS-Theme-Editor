// --------------------------------------------------
// 3DS Theme Editor - MainWindow.Themes.cs
// --------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

using ThemeEditor.Common.Compression;
using ThemeEditor.Common.Themes;
using ThemeEditor.WPF.Localization;
using ThemeEditor.WPF.Themes;

namespace ThemeEditor.WPF
{
    partial class MainWindow
    {
        private const string BGM_FILE_NAME = "BGM.BCSTM";
        public const string WINDOW_TITLE = "Usagi 3DS Theme Editor";
        public const string WINDOW_TITLE_FORMAT = WINDOW_TITLE + " - {0}";

        private string _themePath;

        public GestureCommandWrapper LoadThemeCommandWrapper { get; private set; }
        public GestureCommandWrapper NewThemeCommandWrapper { get; private set; }

        public ICommand ReloadBGMCommand { get; private set; }

        public GestureCommandWrapper SaveAsThemeCommandWrapper { get; private set; }
        public GestureCommandWrapper SaveThemeCommandWrapper { get; private set; }

        public string ThemePath
        {
            get { return _themePath; }
            set
            {
                if (value == _themePath)
                    return;
                _themePath = value;
                OnPropertyChanged(nameof(ThemePath));
            }
        }

        private bool CanExecute_LoadedFromFile()
        {
            return ThemePath != null;
        }

        private Task<LoadBGMResults> LoadBGM_Execute()
        {
            var busyLoadingBGM = MainResources.Busy_LoadingBGM;
            var task = new Task<LoadBGMResults>(() =>
            {
                BusyText = busyLoadingBGM;
                var result = new LoadBGMResults
                {
                    Loaded = false
                };

                var themeDir = Path.GetDirectoryName(ThemePath);
                var bgmFile = Path.Combine(themeDir, BGM_FILE_NAME);
                var vgmStream = new[]
                {
                    Environment.CurrentDirectory,
                    "ThirdParty",
                    "vgmstream",
                    "test.exe"
                }.Aggregate(Path.Combine);

                if (!File.Exists(bgmFile) || !File.Exists(vgmStream))
                    return result;

                using (var ms = new MemoryStream())
                {
                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = vgmStream,
                            Arguments = $"-P \"{bgmFile}\""
                        };

                        using (var process = Process.Start(psi))
                            process.StandardOutput.BaseStream.CopyTo(ms);

                        result.Loaded = true;
                        result.BGMData = ms.ToArray();
                    }
                    catch
                    {
                        // Ignore
                    }
                }

                return result;
            });
            task.Start();
            return task;
        }

        private void LoadBGM_PostExecute(LoadBGMResults result)
        {
            if (result.Loaded)
            {
                AudioPlayer.Instance.SetAudioData(result.BGMData);
                if (!ViewModel.Flags.BackgroundMusic)
                {
                    MessageBox.Show(MainResources.Error_BackgroundMusicLoaded,
                        WINDOW_TITLE,
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    ViewModel.Flags.BackgroundMusic = true;
                }
            }
            else
            {
                AudioPlayer.Instance.ClearAudioData();
                if (ViewModel.Flags.BackgroundMusic)
                {
                    MessageBox.Show(MainResources.Error_NoBackgroundMusicOnLoad,
                        WINDOW_TITLE,
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    ViewModel.Flags.BackgroundMusic = false;
                }
            }
            IsBusy = false;
        }

        private Task<LoadThemeResults> LoadNullTheme_Execute()
        {
            var busyLoadingTheme = MainResources.Busy_LoadingTheme;
            var task = new Task<LoadThemeResults>(() =>
            {
                var result = new LoadThemeResults
                {
                    Loaded = false,
                    Path = null,
                };

                BusyText = busyLoadingTheme;
                var res = Extensions.GetResources("body_lz\\.bin");
                using (var fs = (Stream) res.Values.First())
                using (var ms = new MemoryStream())
                {
                    try
                    {
                        LZ11.Decompress(fs, fs.Length, ms);
                        ms.Position = 0;
                        result.Theme = Theme.Read(ms);
                        result.Loaded = true;
                    }
                    catch
                    {
                        return result;
                    }
                }

                return result;
            });
            task.Start();
            return task;
        }

        private Task<LoadThemeResults> LoadTheme_Execute(string path)
        {
            var busyPickingFile = MainResources.Busy_PickingFile;
            var busyLoadingTheme = MainResources.Busy_LoadingTheme;
            var task = new Task<LoadThemeResults>(() =>
            {
                var result = new LoadThemeResults
                {
                    Loaded = false,
                    Path = path,
                };

                if (string.IsNullOrEmpty(result.Path))
                {
                    BusyText = busyPickingFile;
                    var opfl = new OpenFileDialog
                    {
                        Filter = "3DS Theme File|body_LZ.bin",
                        Multiselect = false
                    };
                    var dlg = opfl.ShowDialog();
                    if (dlg.HasValue && dlg.Value)
                        result.Path = opfl.FileName;
                }

                if (string.IsNullOrEmpty(result.Path))
                    return result;

                BusyText = busyLoadingTheme;
                using (var fs = File.OpenRead(result.Path))
                using (var ms = new MemoryStream())
                {
                    try
                    {
                        LZ11.Decompress(fs, fs.Length, ms);
                        ms.Position = 0;
                        result.Theme = Theme.Read(ms);
                        result.Loaded = true;
                    }
                    catch
                    {
                        return result;
                    }
                }

                return result;
            });
            task.Start();
            return task;
        }

        private void LoadTheme_PostExecute(LoadThemeResults result)
        {
            if (result.Loaded)
            {
                ViewModel = new ThemeViewModel(result.Theme);
                ThemePath = result.Path;

                if (ReloadBGMCommand.CanExecute(null))
                    ReloadBGMCommand.Execute(null);
            }
            IsBusy = false;
        }

        private BitmapSource RenderPreview()
        {
            bool wasPreviewing = chk_AnimatePreview.IsChecked.HasValue && chk_AnimatePreview.IsChecked.Value;

            chk_AnimatePreview.IsChecked = false;

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

            RenderTargetBitmap bmp = new RenderTargetBitmap(412, 480, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

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

            var rtb
                = new RenderTargetBitmap((int) (bounds.Width * dpiX / 96.0),
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

        private Task<SaveThemeResults> SaveTheme_Execute(string path)
        {
            var viewModel = ViewModel;
            var busyPickingFile = MainResources.Busy_PickingFile;
            var busySavingTheme = MainResources.Busy_SavingTheme;
            var task = new Task<SaveThemeResults>(() =>
            {
                BusyText = busyPickingFile;

                var result = new SaveThemeResults
                {
                    Saved = false,
                    Path = path
                };

                if (string.IsNullOrEmpty(result.Path))
                {
                    var svfl = new SaveFileDialog
                    {
                        Filter = "3DS Theme File|body_LZ.bin",
                    };
                    var dlg = svfl.ShowDialog();
                    if (dlg.HasValue && !dlg.Value)
                        return result;
                    result.Path = svfl.FileName;
                }

                BusyText = busySavingTheme;
                using (var decBuffer = new MemoryStream())
                using (var comBuffer = new MemoryStream())
                {
                    try
                    {
                        viewModel.Save(decBuffer);
                        decBuffer.Position = 0;
                        LZ11.Compress(decBuffer, decBuffer.Length, comBuffer, true);
                        comBuffer.Position = 0;
                        using (var fs = File.Open(result.Path, FileMode.Create))
                            comBuffer.CopyTo(fs);
                        result.Saved = true;
                    }
                    catch
                    {
                        // Ignore
                    }
                }

                return result;
            },
                TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private void SaveTheme_PostExecute(SaveThemeResults result)
        {
            if (result.Saved)
            {
                ThemePath = result.Path;
                var themeDir = Path.GetDirectoryName(result.Path);

                var bmp = RenderPreview();

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));

                var path = Path.Combine(themeDir, "preview.png");
                using (Stream stream = File.Create(path))
                    encoder.Save(stream);

                MessageBox.Show(MainResources.Error_ThemeSaved,
                    WINDOW_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                if (ViewModel.Flags.BackgroundMusic)
                {
                    if (!File.Exists(Path.Combine(themeDir, BGM_FILE_NAME)))
                    {
                        MessageBox.Show(MainResources.Error_NoBackgroundMusicOnSave,
                            WINDOW_TITLE,
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
            }

            IsBusy = false;
        }

        partial void SetupThemeCommands()
        {
            var loadCommand = new RelayCommandAsync<string, LoadThemeResults>(LoadTheme_Execute,
                null,
                str => PreExecute_SetBusy(),
                LoadTheme_PostExecute);

            var saveAsCommand = new RelayCommandAsync<string, SaveThemeResults>(SaveTheme_Execute,
                str => CanExecute_ViewModelLoaded(),
                str => PreExecute_SetBusy(),
                SaveTheme_PostExecute);

            var saveCommand = new RelayCommandAsync<SaveThemeResults>(() => SaveTheme_Execute(ThemePath),
                CanExecute_ViewModelLoaded,
                PreExecute_SetBusy,
                SaveTheme_PostExecute);

            var newCommand = new RelayCommandAsync<LoadThemeResults>(LoadNullTheme_Execute,
                null,
                PreExecute_SetBusy,
                LoadTheme_PostExecute);

            LoadThemeCommandWrapper
                = new GestureCommandWrapper(loadCommand, new KeyGesture(Key.O, ModifierKeys.Control));
            SaveThemeCommandWrapper
                = new GestureCommandWrapper(saveCommand, new KeyGesture(Key.S, ModifierKeys.Control));
            SaveAsThemeCommandWrapper
                = new GestureCommandWrapper(saveAsCommand, new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));

            NewThemeCommandWrapper = new GestureCommandWrapper(newCommand, new KeyGesture(Key.N, ModifierKeys.Control));

            ReloadBGMCommand = new RelayCommandAsync<LoadBGMResults>(LoadBGM_Execute,
                CanExecute_LoadedFromFile,
                PreExecute_SetBusy,
                LoadBGM_PostExecute);
        }

        private class LoadThemeResults
        {
            public bool Loaded;
            public string Path;
            public Theme Theme;
        }

        private class LoadBGMResults
        {
            public byte[] BGMData;
            public bool Loaded;
        }

        private class SaveThemeResults
        {
            public string Path;
            public bool Saved;
        }
    }
}
