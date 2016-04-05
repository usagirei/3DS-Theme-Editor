using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF
{
    partial class MainWindow
    {
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

    }
}
