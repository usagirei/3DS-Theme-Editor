using System;
using System.Globalization;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class EscapeStringUnderscoreCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            return str?.Replace("_", "__") ?? value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}