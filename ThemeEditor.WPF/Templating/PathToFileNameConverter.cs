using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class PathToFileNameConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level",
            typeof (int),
            typeof (PathToFileNameConverter),
            new PropertyMetadata(0));

        public int Level
        {
            get { return (int) GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return null;
            var str = (string) value;
            var nodes = str.Split(Path.DirectorySeparatorChar);
            var up = (nodes.Length - 1) - Level;
            up = Math.Max(0, up);
            return nodes[up];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}