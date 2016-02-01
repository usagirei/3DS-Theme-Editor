using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using NAudio.Wave;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF.Experimental.CWAV
{
    public class CwavFile : INotifyPropertyChanged
    {
        public static readonly byte[] EmptyData = new byte[0];
        private byte[] _cwavData;

        private string _tag;

        private byte[] _wavData;

        public byte[] CwavData
        {
            get { return _cwavData; }
            set
            {
                _cwavData = value;
                _wavData = null;

                // Make Sure the Imported CWAV has a correct Size
                // This may result in chopped audio
                // However the file itself is broken in the first place for having the incorrect size header
                EnsureSize();

                OnPropertyChanged(nameof(CwavData));
                OnPropertyChanged(nameof(WavData));
                OnPropertyChanged(nameof(Size));
            }
        }

        private void EnsureSize()
        {
            if (Size == CwavData.Length)
                return;
            var bytes = BitConverter.GetBytes(_cwavData.Length);
            Buffer.BlockCopy(bytes, 0, _cwavData, 0x0C, 4);
            OnPropertyChanged(nameof(Size));
        }

        public int Size => CwavData?.Length > 0x10 ? BitConverter.ToInt32(_cwavData, 0x0C) : 0;

        public string Tag
        {
            get { return _tag; }
            set
            {
                if (value == _tag)
                    return;
                _tag = value;
                OnPropertyChanged(nameof(Tag));
            }
        }

        public byte[] WavData => _wavData ?? (_wavData = DecodeCwav(CwavData));

        public CwavFile(byte[] data)
        {
            CwavData = data;
        }

        public static byte[] DecodeCwav(byte[] cwavData)
        {
            if (cwavData.Length == 0)
                return EmptyData;
            if (!ThirdPartyTools.VgmStream.Present)
                return EmptyData;

            using (var tf = new TempFile("bcwav"))
            using (var ms = new MemoryStream())
            {
                using (var fs = File.Open(tf.FilePath, FileMode.Create))
                {
                    fs.Write(cwavData, 0, cwavData.Length);
                    fs.Flush();
                }
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = ThirdPartyTools.VgmStream.Path,
                        Arguments = $"-P \"{tf.FilePath}\""
                    };

                    using (var process = Process.Start(psi))
                        process.StandardOutput.BaseStream.CopyTo(ms);

                    return ms.ToArray();
                }
                catch
                {
                    return EmptyData;
                }
            }
        }

        public static CwavFile Empty() => new CwavFile(EmptyData);

        public static byte[] EncodeCwav(byte[] wavData)
        {
            if (!ThirdPartyTools.CtrWaveConveter.Present)
                return EmptyData;

            using (var sourceStream = new MemoryStream(wavData))
                //
            using (var tempBcwav = new TempFile("bcwav"))
            using (var tempWav = new TempFile("wav"))
                //
            {
                using (var tempWavStream = File.Open(tempWav.FilePath, FileMode.Create))
                {
                    if (Settings.Default.EXP_ResampleWav)
                    {
                        var nwf = new WaveFormat(22050, 8, 1); //22Khz Mono PCM8
                        var wr = new WaveFileReader(sourceStream);
                        var conv = new WaveFormatConversionStream(nwf, wr);
                        conv.CopyTo(tempWavStream);
                    }
                    else
                    {
                        sourceStream.CopyTo(tempWavStream);
                    }
                    tempWavStream.Flush(true);
                }

                try
                {
                    var psi = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = ThirdPartyTools.CtrWaveConveter.Path,
                        Arguments = $"-o \"{tempBcwav.FilePath}\" \"{tempWav.FilePath}\""
                    };

                    var process = Process.Start(psi);
                    process.WaitForExit();

                    using (var tempBcwavStream
                        = File.Open(tempBcwav.FilePath, FileMode.Open))
                    {
                        byte[] converted = new byte[tempBcwavStream.Length];
                        tempBcwavStream.Read(converted, 0, converted.Length);
                        return converted;
                    }
                }
                catch
                {
                    return EmptyData;
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
