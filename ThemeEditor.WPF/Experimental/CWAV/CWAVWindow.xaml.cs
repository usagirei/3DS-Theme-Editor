using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Win32;

using NAudio.Wave;

namespace ThemeEditor.WPF.Experimental.CWAV
{
    /// <summary>
    ///     Interaction logic for CwavWindow.xaml
    /// </summary>
    public partial class CwavWindow : Window
    {
        private readonly WaveOut _sfxPlayer;
        private WaveFileReader _sfxProvider;
        private Stream _sfxStream;

        public ICommand ImportCommand { get; set; }

        public ICommand PlaySfxCommand { get; }
        public CwavBlock ViewModel { get; set; }

        public ICommand ExportSfxCommand { get; set; }

        public ICommand RemoveSfxCommand { get; set; }

        public ICommand ReplaceSfxCommand { get; set; }

        public CwavWindow()
        {
            InitializeComponent();
            _sfxPlayer = new WaveOut();

            PlaySfxCommand = new RelayCommand<CwavKind>(PlayAudio_Execute, CanExecute_HasAudio);
            ImportCommand = new RelayCommandAsync<CwavKind, ImportResults>(Import_Execute, null, null, Import_PostExecute);
            ExportSfxCommand = new RelayCommandAsync<CwavKind, ExportResults>(Export_Execute,
                CanExecute_HasAudio,
                null,
                Export_PostExecute);
            RemoveSfxCommand = new RelayCommand<CwavKind>(Remove_Execute, CanExecute_HasAudio);
            ReplaceSfxCommand = new RelayCommand<object>(Replace_Execute);
        }

        private void Remove_Execute(CwavKind obj)
        {
            Import_PostExecute(new ImportResults()
            {
                CWAV = CwavFile.Empty(),
                Target = obj,
                Loaded = true
            });
        }

        private void Replace_Execute(object obj)
        {
            var arr = (object[]) obj;
            Import_PostExecute(new ImportResults()
            {
                CWAV = (CwavFile) arr[0],
                Target = (CwavKind) arr[1],
                Loaded =  true
            });
        }

        private void Export_PostExecute(ExportResults results)
        {
            if (!results.Saved)
                return;
            MessageBox.Show("SFX Exported");
        }

        private Task<ExportResults> Export_Execute(CwavKind arg)
        {
            var cWav = ViewModel.Sounds[arg];
            return Task<ExportResults>.Factory.StartNew(() =>
            {
                var results = new ExportResults
                {
                    Saved = false
                };
                var svfd = new SaveFileDialog
                {
                    Filter = ThirdPartyTools.VgmStream.Present
                                 ? "DSP ADPCM Audio|*.bcwav|PCM Audio|*.wav"
                                 : "DSP ADPCM Audio|*.bcwav"
                };
                var dlg = svfd.ShowDialog();
                if (dlg.HasValue && dlg.Value)
                {
                    try
                    {
                        switch (svfd.FilterIndex)
                        {
                            case 1:
                                File.WriteAllBytes(svfd.FileName, cWav.CwavData);
                                results.Saved = true;
                                break;
                            case 2:
                                File.WriteAllBytes(svfd.FileName, cWav.WavData);
                                results.Saved = true;
                                break;
                        }
                    }
                    catch
                    {
                        // Ignore
                    }
                }
                return results;
            });
        }

        public bool CanExecute_HasAudio(CwavKind cwavKind)
        {
            return ViewModel.Sounds[cwavKind].WavData.Length > 0;
        }

        public void PlayAudio_Execute(CwavKind cwavKind)
        {
            var wavData = ViewModel.Sounds[cwavKind].WavData;
            if (wavData.Length == 0)
                return;

            _sfxPlayer.Stop();

            _sfxProvider?.Dispose();
            _sfxStream?.Dispose();

            _sfxStream = new MemoryStream(wavData);
            _sfxProvider = new WaveFileReader(_sfxStream);

            _sfxPlayer.Init(_sfxProvider);
            _sfxPlayer.Play();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            var dataSize = ViewModel.EstimatedSize;
            if (dataSize > 0x2DC00)
            {
                MessageBox.Show("CWAV Data Too Big:\n" +
                                "Maximum Size:\t" + 0x2DC00 + " bytes.\n" +
                                "Current Size:\t" + dataSize + " bytes");
                return;
            }
            DialogResult = true;
        }

        private void CWAVWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _sfxPlayer?.Dispose();
            _sfxProvider?.Dispose();
        }

        private Task<ImportResults> Import_Execute(CwavKind cwavKind)
        {
            return Task<ImportResults>.Factory.StartNew(() =>
            {
                var result = new ImportResults
                {
                    Loaded = false,
                    Target = cwavKind
                };
                OpenFileDialog opfd = new OpenFileDialog
                {
                    Filter = ThirdPartyTools.CtrWaveConveter.Present
                                 ? "Supported Files|*.wav;*.bcwav|DSP ADCPM Audio|*.bcwav|PCM Audio|*.wav"
                                 : "Supported Files|*.bcwav|DSP ADCPM Audio|*.bcwav"
                };
                var dlg = opfd.ShowDialog();
                if (dlg.HasValue && dlg.Value)
                {
                    try
                    {
                        switch (opfd.FilterIndex)
                        {
                            case 1:
                            {
                                if (opfd.FileName.EndsWith(".bcwav", StringComparison.OrdinalIgnoreCase))
                                    goto case 2;
                                if (opfd.FileName.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                                    goto case 3;
                                break;
                            }
                            case 2:
                            {
                                // Read CWAV
                                var cwavData = File.ReadAllBytes(opfd.FileName);
                                result.Loaded = true;
                                result.CWAV = new CwavFile(cwavData);
                                break;
                            }
                            case 3:
                            {
                                // Convert Wav
                                var wavData = File.ReadAllBytes(opfd.FileName);
                                byte[] cwavData = CwavFile.EncodeCwav(wavData);
                                if (cwavData.Length == 3)
                                {
                                    return result;
                                }
                                result.Loaded = true;
                                result.CWAV = new CwavFile(cwavData);
                                break;
                            }
                        }
                    }
                    catch
                    {
                        // Ignore
                    }
                }
                return result;
            });
        }

        private void Import_PostExecute(ImportResults results)
        {
            if (!results.Loaded)
                return;

            ViewModel.Sounds[results.Target].CwavData = results.CWAV.CwavData;
        }

        private class ExportResults
        {
            public string Path;
            public bool Saved;
            public CwavKind Target;
        }

        private class ImportResults
        {
            public CwavFile CWAV;
            public bool Loaded;
            public CwavKind Target;
        }
    }
}
