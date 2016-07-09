using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class NullConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty NullValueProperty = DependencyProperty.Register(
            nameof(NullValue),
            typeof(object),
            typeof(NullConverter),
            new PropertyMetadata(default(object)));

        public object NullValue
        {
            get { return (object)GetValue(NullValueProperty); }
            set { SetValue(NullValueProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? NullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}