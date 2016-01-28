// --------------------------------------------------
// 3DS Theme Editor - AudioPlayer.cs
// --------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

using NAudio.Wave;

namespace ThemeEditor.WPF
{
    class AudioPlayer : INotifyPropertyChanged
    {
        private LoopStream _loopStream;

        private float _volume = 1f;
        private WaveOut _waveOut;
        private WaveFileReader _waveProvider;

        public bool HasData { get; private set; }
        public static AudioPlayer Instance { get; } = new AudioPlayer();

        public bool IsPlaying => _waveOut?.PlaybackState == PlaybackState.Playing;

        public ICommand PlayCommand { get; }

        public ICommand StopCommand { get; }

        public float Volume
        {
            get { return _volume; }
            set
            {
                if (Math.Abs(_waveOut.Volume - value) < 0.001f)
                    return;
                _waveOut.Volume = _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        public AudioPlayer()
        {
            PlayCommand = new RelayCommand(Play_Execute, CanExecute_HasData);
            StopCommand = new RelayCommand(Stop_Execute, CanExecute_HasData);
        }

        public void ClearAudioData()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
            if (_waveProvider != null)
            {
                _waveProvider.Dispose();
                _waveProvider = null;
            }
            if (_loopStream != null)
            {
                _loopStream.Dispose();
                _loopStream = null;
            }
            HasData = false;
        }

        public void SetAudioData(byte[] wavData)
        {
            ClearAudioData();

            var wavStream = new MemoryStream(wavData);
            _waveProvider = new WaveFileReader(wavStream);
            _waveOut = new WaveOut();
            _loopStream = new LoopStream(_waveProvider);
            _waveOut.Init(_loopStream);
            _waveOut.Volume = _volume;

            HasData = true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanExecute_HasData()
        {
            return HasData;
        }

        private void Play_Execute()
        {
            if (_waveOut.PlaybackState != PlaybackState.Playing)
                _waveOut.Play();
            else
                _waveOut.Pause();
            OnPropertyChanged(nameof(IsPlaying));
        }

        private void Stop_Execute()
        {
            if (_waveOut.PlaybackState == PlaybackState.Stopped)
                return;
            _waveOut.Stop();
            _loopStream.Position = 0;
            OnPropertyChanged(nameof(IsPlaying));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public class LoopStream : WaveStream
        {
            readonly WaveStream _src;

            public override long Length => _src.Length;

            public bool Loop { get; set; }

            public override long Position
            {
                get { return _src.Position; }
                set { _src.Position = value; }
            }

            public override WaveFormat WaveFormat => _src.WaveFormat;

            public LoopStream(WaveStream src)
            {
                _src = src;
                Loop = true;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                var total = 0;

                while (total < count)
                {
                    var read = _src.Read(buffer, offset + total, count - total);
                    if (read == 0)
                    {
                        if (_src.Position == 0 || !Loop)
                            break;
                        _src.Position = 0;
                    }
                    total += read;
                }
                return total;
            }
        }
    }
}
