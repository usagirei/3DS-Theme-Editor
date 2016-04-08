// --------------------------------------------------
// 3DS Theme Editor - MainWindow.Themes.cs
// --------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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

        public ICommand ConvertBGMCommand { get; private set; }

        public GestureCommandWrapper SaveAsThemeCommandWrapper { get; private set; }
        public GestureCommandWrapper SaveThemeCommandWrapper { get; private set; }

        public ICommand SendThemeCHMM2Command { get; private set; }

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

        private Task<LoadThemeResults> LoadNullTheme_Execute() => LoadNullTheme_Execute(true);

        private Task<LoadThemeResults> LoadNullTheme_Execute(bool start)
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
            if (start)
                task.Start();
            return task;
        }

        private Task<LoadThemeResults> LoadTheme_Execute(string path) => LoadTheme_Execute(path, true);

        private Task<LoadThemeResults> LoadTheme_Execute(string path, bool start)
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
                        Filter = "3DS Theme File|*.bin",
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
            if (start)
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

                    ViewModel.Info.LargeIcon.Bitmap = (BitmapSource) large;
                    ViewModel.Info.SmallIcon.Bitmap = (BitmapSource) small;
                }

                ThemePath = result.Path;

                if (ReloadBGMCommand.CanExecute(null))
                    ReloadBGMCommand.Execute(null);
            }
            IsBusy = false;
        }

        private Task<SaveThemeResults> SaveTheme_Execute(string path) => SaveTheme_Execute(path, true);

        private Task<SaveThemeResults> SaveTheme_Execute(string path, bool start)
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
                        Filter = "3DS Theme File|*.bin",
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
            if (start)
                task.Start();
            return task;
        }

        private Task<ZipThemeResults> ZipTheme_Execute(string bgmPath) => ZipTheme_Execute(bgmPath, true);

        private Task<ZipThemeResults> ZipTheme_Execute(string bgmPath, bool start)
        {
            var viewModel = ViewModel;
            var busySavingTheme = MainResources.Busy_SavingTheme;
            var bmpPreview = RenderPreview(PreviewKind.Both);

            var task = new Task<ZipThemeResults>(() =>
            {
                BusyText = busySavingTheme;

                var results = new ZipThemeResults
                {
                    Saved = false,
                    BGMPath = bgmPath,
                    Data = null
                };

                var themeName = viewModel.Info.ShortDescription;
                var rnd = new Random();
                if (string.IsNullOrEmpty(themeName))
                {
                    var bytes = new byte[4];
                    rnd.NextBytes(bytes);
                    themeName = "3DSThemeEditor-" + BitConverter.ToString(bytes);
                }
                themeName = Regex.Replace(themeName, "[^ -~]+", "_");
                using (var ms = new MemoryStream())
                {
                    using (var arch = new ZipArchive(ms, ZipArchiveMode.Create))
                    {
                        var bodyLz = arch.CreateEntry($"{themeName}/body_LZ.bin");
                        using (var tgt = bodyLz.Open())
                        using (var srcDec = new MemoryStream())
                        using (var srcCom = new MemoryStream())
                        {
                            viewModel.Save(srcDec);

                            viewModel.Save(srcDec);
                            srcDec.Position = 0;
                            LZ11.Compress(srcDec, srcDec.Length, srcCom, true);
                            srcCom.Position = 0;

                            srcCom.CopyTo(tgt);
                        }

                        var info = arch.CreateEntry($"{themeName}/info.smdh");
                        using (var tgt = info.Open())
                            viewModel.Info.Save(tgt);

                        var preview = arch.CreateEntry($"{themeName}/preview.png");
                        using (var tgt = preview.Open())
                        using (var src = new MemoryStream())
                        {
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bmpPreview));

                            encoder.Save(src);
                            src.Position = 0;

                            src.CopyTo(tgt);
                        }

                        if (viewModel.Flags.BackgroundMusic)
                        {
                            var bgm = arch.CreateEntry($"{themeName}/BGM.BCSTM");
                            using (var src = File.OpenRead(results.BGMPath))
                            using (var tgt = bgm.Open())
                                src.CopyTo(tgt);
                        }
                    }
                    results.Data = ms.ToArray();
                    results.Saved = true;
                    File.WriteAllBytes(@"C:\Temp\theme.zip", results.Data);
                }

                return results;
            },
                TaskCreationOptions.LongRunning);
            if (start)
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

            var sendChmm2Command = new RelayCommandAsync<string, SendThemeResults>(SendTheme_Execute,
                str => CanExecute_ViewModelLoaded(),
                str => PreExecute_SetBusy(),
                SendTheme_PostExecute);

            LoadThemeCommandWrapper
                = new GestureCommandWrapper(loadCommand, new KeyGesture(Key.O, ModifierKeys.Control));
            SaveThemeCommandWrapper
                = new GestureCommandWrapper(saveCommand, new KeyGesture(Key.S, ModifierKeys.Control));
            SaveAsThemeCommandWrapper
                = new GestureCommandWrapper(saveAsCommand,
                    new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));

            SendThemeCHMM2Command = sendChmm2Command;

            NewThemeCommandWrapper = new GestureCommandWrapper(newCommand, new KeyGesture(Key.N, ModifierKeys.Control));

            ReloadBGMCommand = new RelayCommandAsync<LoadBGMResults>(LoadBGM_Execute,
                CanExecute_LoadedFromFile,
                PreExecute_SetBusy,
                LoadBGM_PostExecute);
        }

        private void SendTheme_PostExecute(SendThemeResults obj)
        {
            IsBusy = false;
            if (obj.Sent)
            {
                MessageBox.Show("Theme Sent Successfuly",
                    WINDOW_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private Task<SendThemeResults> SendTheme_Execute(string ip) => SendTheme_Execute(ip, true);

        private Task<SendThemeResults> SendTheme_Execute(string ip, bool start)
        {
            var sendingTheme = "Sending Theme...";
            var connecting = "Estabilishing Conection...";
            var connectError = "Conection Error";
            var zipThemeTask = ZipTheme_Execute(ThemePath, false);
            var task = new Task<SendThemeResults>(() =>
            {
                var result = new SendThemeResults
                {
                    IP = ip,
                    Sent = false,
                };
                IPAddress addr;
                if (!IPAddress.TryParse(ip, out addr))
                    return result;

                IPEndPoint endPoint = new IPEndPoint(addr, 5000);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                zipThemeTask.Start();
                result.Zip = zipThemeTask.Result;

                BusyText = connecting;

                try
                {
                    socket.Connect(endPoint);
                }
                catch (SocketException ex)
                {
                    BusyText = ex.Message;
                    return result;
                }

                BusyText = sendingTheme;

                byte[] socketBuffer = new byte[255];
                socket.Receive(socketBuffer, 11, SocketFlags.None);
                if (Encoding.ASCII.GetString(socketBuffer, 0, 11) != "YATA SENDER")
                {
                    BusyText = connectError;
                    socket.Close();
                    return result;
                }

                var toWrite = result.Zip.Data.Length;
                var offset = 0;
                var fLen = (float) result.Zip.Data.Length;

                while (true)
                {
                    var step = Math.Min(toWrite, 8192);
                    socket.Send(result.Zip.Data, offset, step, 0);
                    toWrite -= step;
                    offset += step;

                    BusyText = $"Sending: {offset / fLen:f2}%";
                    if (toWrite <= 0)
                        break;
                }

                socket.Receive(socketBuffer, 9, SocketFlags.None);
                if (Encoding.ASCII.GetString(socketBuffer, 0, 9) != "YATA TERM")
                {
                    BusyText = connectError;
                    socket.Close();
                    return result;
                }

                result.Sent = true;
                return result;
            });
            if (start)
                task.Start();
            return task;
        }

        private class LoadThemeResults
        {
            public SMDH Info;
            public bool Loaded;
            public string Path;
            public Theme Theme;
        }

        private class LoadBGMResults
        {
            public byte[] BGMData;
            public bool Loaded;
        }

        private class ZipThemeResults
        {
            public string BGMPath;
            public byte[] Data;
            public bool Saved;
        }

        private class SendThemeResults
        {
            public string IP;
            public bool Sent;
            public ZipThemeResults Zip;
        }

        private class SaveThemeResults
        {
            public string Path;
            public bool Saved;
        }
    }
}