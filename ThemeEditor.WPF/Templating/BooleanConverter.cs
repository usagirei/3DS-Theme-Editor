using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class BooleanConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty FalseValueProperty
            = DependencyProperty.Register(nameof(FalseValue),
                typeof(object),
                typeof(BooleanConverter),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TrueValueProperty
            = DependencyProperty.Register(nameof(TrueValue),
                typeof(object),
                typeof(BooleanConverter),
                new PropertyMetadata(default(object)));

        public object FalseValue
        {
            get { return GetValue(FalseValueProperty); }
            set { SetValue(FalseValueProperty, value); }
        }

        public object TrueValue
        {
            get { return GetValue(TrueValueProperty); }
            set { SetValue(TrueValueProperty, value); }
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool) value
                           ? TrueValue
                           : FalseValue;
            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
