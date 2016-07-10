using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class BooleanConverter : DependencyObject, IValueConverter, IMultiValueConverter
    {
        public static readonly DependencyProperty FalseValueProperty
            = DependencyProperty.Register(nameof(FalseValue),
                typeof(object),
                typeof(BooleanConverter),
                new PropertyMetadata(false));

        public static readonly DependencyProperty TrueValueProperty
            = DependencyProperty.Register(nameof(TrueValue),
                typeof(object),
                typeof(BooleanConverter),
                new PropertyMetadata(true));

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
                return (bool)value
                    ? TrueValue
                    : FalseValue;
            return null;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool)
                return (bool)values[0]
                    ? values[1]
                    : values[2];
            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}