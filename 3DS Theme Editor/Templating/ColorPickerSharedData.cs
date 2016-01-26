// --------------------------------------------------
// 3DS Theme Editor - ColorPickerSharedData.cs
// --------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Xceed.Wpf.Toolkit;

namespace ThemeEditor.WPF
{
    public class ColorPickerSharedData : DependencyObject, INotifyPropertyChanged
    {
        private ColorMode _colorMode;

        private ObservableCollection<ColorItem> _recentColors;

        public ColorMode ColorMode
        {
            get { return _colorMode; }
            set
            {
                if (value == _colorMode)
                    return;
                _colorMode = value;
                OnPropertyChanged(nameof(ColorMode));
            }
        }

        public static ICommand CopyCommand { get; }

        public static ColorPickerSharedData Instance { get; } = new ColorPickerSharedData();
        public static ICommand PasteCommand { get; }

        public ObservableCollection<ColorItem> RecentColors
        {
            get { return _recentColors; }
            set
            {
                if (value == _recentColors)
                    return;
                _recentColors = value;
                OnPropertyChanged(nameof(RecentColors));
            }
        }

        static ColorPickerSharedData()
        {
            CopyCommand = new RelayCommand<ColorPicker>(ColorPickerCopy);
            PasteCommand = new RelayCommand<ColorPicker>(ColorPickerPaste);
        }

        private ColorPickerSharedData() {}

        private static void ColorPickerCopy(ColorPicker colorPicker)
        {
            Clipboard.SetText(colorPicker.SelectedColorText);
        }

        private static void ColorPickerPaste(ColorPicker colorPicker)
        {
            var strColor = Clipboard.GetText();
            var fromString = ColorConverter.ConvertFromString(strColor);
            if (fromString != null)
            {
                var col = (Color) fromString;
                colorPicker.SelectedColor = col;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
