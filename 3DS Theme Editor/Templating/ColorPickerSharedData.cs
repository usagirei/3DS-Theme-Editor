// --------------------------------------------------
// 3DS Theme Editor - ColorPickerSharedData.cs
// --------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Xceed.Wpf.Toolkit;

namespace ThemeEditor.WPF
{
    public class ColorPickerSharedData : DependencyObject, INotifyPropertyChanged
    {
        private const double EPSILON = 0.01;
        private const string FORMAT34 = @"(?<Format>\w{1,})\((?<Data>(?:(?:[\d.]*?,)){2,3}([\d.]*?))\)";
        private const string HEX6 = @"(?<Format>#)(?<Data>[A-Fa-f0-9]{6})";
        private const string HEX8 = @"(?<Format>#)(?<Data>[A-Fa-f0-9]{8})";

        private static readonly Regex ColorRegex = new Regex($"{HEX8}|{HEX6}|{FORMAT34}", RegexOptions.Compiled);
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
            CopyCommand = new RelayCommand<ColorPicker>(ColorPickerCopy_Execute);
            PasteCommand = new RelayCommand<ColorPicker>(ColorPickerPaste_Execute, ColorPickerPaste_CanExecute);
        }

        private ColorPickerSharedData() {}

        public static bool CanParseColor(string colorString)
        {
            string[] validDataTypes = {"rgb", "rgba", "hsl", "hsla", "hsv", "hsva", "#"};
            var ismatch = ColorRegex.IsMatch(colorString);
            if (!ismatch)
                return false;

            var m = ColorRegex.Match(colorString);
            var dataType = m.Groups["Format"].Value;
            return validDataTypes.Contains(dataType);
        }

        public static Color ColorFromHsl(double hue, double sat, double bright, double alpha = 1.0)
        {
            //bright = Math.Min(1,bright);
            hue %= 360.0f;
            sat = sat.Clamp(0, 1);
            bright = bright.Clamp(0, 1);
            var chroma = (1 - Math.Abs((2 * bright) - 1)) * sat;
            var hueIndex = hue / 60.0 % 6.0;
            var factor = chroma * (1 - Math.Abs((hueIndex % 2) - 1));
            double red = 0, green = 0, blue = 0;
            switch ((int) Math.Floor(hueIndex))
            {
                case 0:
                    red = chroma;
                    green = factor;
                    break;
                case 1:
                    red = factor;
                    green = chroma;
                    break;
                case 2:
                    green = chroma;
                    blue = factor;
                    break;
                case 3:
                    green = factor;
                    blue = chroma;
                    break;
                case 4:
                    red = factor;
                    blue = chroma;
                    break;
                case 5:
                    red = chroma;
                    blue = factor;
                    break;
            }
            var m = bright - (chroma / 2.0);
            var alphaByte = Convert.ToByte(alpha * 255);
            var redByte = Convert.ToByte((red + m) * 255);
            var greenByte = Convert.ToByte((green + m) * 255);
            var blueByte = Convert.ToByte((blue + m) * 255);
            return Color.FromArgb(alphaByte, redByte, greenByte, blueByte);
        }

        public static Color ColorFromHsv(double hue, double sat, double value, double alpha = 1.0)
        {
            var chroma = value * sat;
            var hueIndex = hue / 60.0 % 6.0;
            var factor = chroma * (1 - Math.Abs((hueIndex % 2) - 1));
            double red = 0, green = 0, blue = 0;
            switch ((int) Math.Floor(hueIndex))
            {
                case 0:
                    red = chroma;
                    green = factor;
                    break;
                case 1:
                    red = factor;
                    green = chroma;
                    break;
                case 2:
                    green = chroma;
                    blue = factor;
                    break;
                case 3:
                    green = factor;
                    blue = chroma;
                    break;
                case 4:
                    red = factor;
                    blue = chroma;
                    break;
                case 5:
                    red = chroma;
                    blue = factor;
                    break;
            }
            var m = value - chroma;
            var alphaByte = Convert.ToByte(alpha * 255);
            var redByte = Convert.ToByte((red + m) * 255);
            var greenByte = Convert.ToByte((green + m) * 255);
            var blueByte = Convert.ToByte((blue + m) * 255);
            return Color.FromArgb(alphaByte, redByte, greenByte, blueByte);
        }

        public static void ColorToHsl(Color col, out double h, out double s, out double l)
        {
            var r = col.R / 255.0;
            var g = col.G / 255.0;
            var b = col.B / 255.0;

            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));

            l = (max + min) / 2.0f;

            if (Math.Abs(max - min) < EPSILON)
            {
                h = 0;
                s = 0;
            }
            else
            {
                var delta = (max - min);
                if (Math.Abs(max - r) < EPSILON)
                {
                    if (g >= b)
                    {
                        h = 60 * (g - b) / delta;
                    }
                    else
                    {
                        h = 60 * (g - b) / delta + 360;
                    }
                }
                else if (Math.Abs(max - g) < EPSILON)
                {
                    h = 60 * (b - r) / delta + 120;
                }
                else
                {
                    h = 60 * (r - g) / delta + 240;
                }

                s = delta / (1 - Math.Abs(2 * l - 1)); // Weird Colors at l < 0.5 
                //s = delta / (max + min);
                //s = delta / max;
            }
        }

        public static void ColorToHsv(Color col, out double h, out double s, out double v)
        {
            var r = col.R / 255.0;
            var g = col.G / 255.0;
            var b = col.B / 255.0;

            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));

            v = max;

            if (Math.Abs(max - min) < EPSILON)
            {
                h = 0;
                s = 0;
            }
            else
            {
                if (Math.Abs(max - r) < EPSILON)
                {
                    if (g >= b)
                    {
                        h = 60 * (g - b) / (max - min);
                    }
                    else
                    {
                        h = 60 * (g - b) / (max - min) + 360;
                    }
                }
                else if (Math.Abs(max - g) < EPSILON)
                {
                    h = 60 * (b - r) / (max - min) + 120;
                }
                else
                {
                    h = 60 * (r - g) / (max - min) + 240;
                }
                s = (max - min) / max;
            }
        }

        public static bool TryParseColor(string colorString, out Color color)
        {
            try
            {
                var errorColor = Color.FromRgb(0, 0, 0);

                if (!CanParseColor(colorString))
                {
                    color = errorColor;
                    return false;
                }
                var match = ColorRegex.Match(colorString);
                var format = match.Groups["Format"].Value;
                var data = match.Groups["Data"].Value;
                byte[] dataBytes;
                double[] doubleValues;
                switch (format.ToLower())
                {
                    case "#":
                    {
                        dataBytes = Enumerable.Range(0, data.Length)
                                              .Where(x => x % 2 == 0)
                                              .Select(x => Convert.ToByte(data.Substring(x, 2), 16))
                                              .ToArray();
                        if (dataBytes.Length == 4)
                            errorColor = Color.FromArgb(dataBytes[0], dataBytes[1], dataBytes[2], dataBytes[3]);
                        if (dataBytes.Length == 3)
                            errorColor = Color.FromRgb(dataBytes[0], dataBytes[1], dataBytes[2]);
                        break;
                    }

                    case "rgb":
                    case "rgba":
                    {
                        dataBytes = data.Split(',')
                                        .Select(s => Convert.ToByte(s))
                                        .ToArray();
                        if (dataBytes.Length == 3)
                            errorColor = Color.FromRgb(dataBytes[0], dataBytes[1], dataBytes[2]);
                        if (dataBytes.Length == 4)
                            errorColor = Color.FromArgb(dataBytes[3], dataBytes[0], dataBytes[1], dataBytes[2]);
                        break;
                    }

                    case "hsv":
                    case "hsva":
                    {
                        doubleValues = data.Split(',')
                                           .Select(s => Convert.ToDouble(s, CultureInfo.InvariantCulture))
                                           .ToArray();
                        if (doubleValues.Length == 3)
                            errorColor = ColorFromHsv(doubleValues[0], doubleValues[1], doubleValues[2]);
                        if (doubleValues.Length == 4)
                            errorColor = ColorFromHsv
                                (doubleValues[0], doubleValues[1], doubleValues[2], doubleValues[3]);
                        break;
                    }

                    case "hsl":
                    case "hsla":
                    {
                        doubleValues = data.Split(',')
                                           .Select(s => Convert.ToDouble(s, CultureInfo.InvariantCulture))
                                           .ToArray();
                        if (doubleValues.Length == 3)
                            errorColor = ColorFromHsl(doubleValues[0], doubleValues[1], doubleValues[2]);
                        if (doubleValues.Length == 4)
                            errorColor = ColorFromHsl
                                (doubleValues[0], doubleValues[1], doubleValues[2], doubleValues[3]);
                        break;
                    }
                }
                color = errorColor;
                return true;
            }
            catch
            {
                color = Color.FromRgb(0, 0, 0);
                return false;
            }
        }

        private static void ColorPickerCopy_Execute(ColorPicker colorPicker)
        {
            Clipboard.SetText(colorPicker.SelectedColorText);
        }

        private static bool ColorPickerPaste_CanExecute(ColorPicker obj)
        {
            var strColor = Clipboard.GetText();
            return CanParseColor(strColor);
        }

        private static void ColorPickerPaste_Execute(ColorPicker colorPicker)
        {
            var strColor = Clipboard.GetText();
            Color parsedColor;
            if (TryParseColor(strColor, out parsedColor))
                colorPicker.SelectedColor = parsedColor;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
