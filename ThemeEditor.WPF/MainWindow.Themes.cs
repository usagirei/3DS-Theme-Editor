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
using ThemeEditor.Common.SMDH;
using ThemeEditor.Common.Themes;
using ThemeEditor.WPF.Localization;
using ThemeEditor.WPF.Markup;
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

               
                if (!ThirdPartyTools.VgmStream.Present)
                    return result;
                var themeDir = Path.GetDirectoryName(ThemePath);
                var bgmFile = Path.Combine(themeDir, BGM_FILE_NAME);
                if (!File.Exists(bgmFile))
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
                            FileName = ThirdPartyTools.VgmStream.Path,
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

                var themeDir = Path.GetDirectoryName(result.Path) ?? ".";

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
                        // Ignore
                    }
                }
                
                var smdhPath = Path.Combine(themeDir, "info.smdh");
                if (result.Loaded && File.Exists(smdhPath))
                {
                    using (var fs = File.OpenRead(smdhPath))
                    {
                        try
                        {
                            result.Info = SMDH.Read(fs);
                        }
                        catch
                        {
                            result.Info = null;
                            
                        }
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
                ViewModel = new ThemeViewModel(result.Theme, result.Info);

                if (result.Info == null)
                {
                    IconExtension icex = new IconExtension(@"/ThemeEditor.WPF;component/Resources/Icons/app_icn.ico", 48);
                    var large = Extensions.CreateResizedImage((ImageSource) icex.ProvideValue(null), 48, 48);
                    icex.Size = 24;
                    var small = Extensions.CreateResizedImage((ImageSource) icex.ProvideValue(null), 24, 24);

                    ViewModel.Info.SmallIcon.Bitmap = (BitmapSource) small;
                    ViewModel.Info.LargeIcon.Bitmap = (BitmapSource) large;
                }

                ThemePath = result.Path;

                if (ReloadBGMCommand.CanExecute(null))
                    ReloadBGMCommand.Execute(null);
            }
            IsBusy = false;
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
                        FileName = "body_lz"
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

                var bmp = RenderPreview(PreviewKind.Both);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));

                var previewPath = Path.Combine(themeDir, "preview.png");
                using (Stream stream = File.Create(previewPath))
                    encoder.Save(stream);

                var infoPath = Path.Combine(themeDir, "info.smdh");
                using (var stream = File.Create(infoPath))
                    ViewModel.Info.Save(stream);

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
            public SMDH Info;
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
