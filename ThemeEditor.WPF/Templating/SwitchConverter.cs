using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ThemeEditor.WPF.Templating
{
    [ContentProperty(nameof(Cases))]
    public class SwitchConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty CasesProperty = DependencyProperty.Register(
                                                                                              nameof(Cases),
            typeof(List<SwitchConverterCase>),
            typeof(SwitchConverter),
            new PropertyMetadata(default(List<SwitchConverterCase>)));

        public List<SwitchConverterCase> Cases
        {
            get { return (List<SwitchConverterCase>)GetValue(CasesProperty); }
            set { SetValue(CasesProperty, value); }
        }

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            nameof(DefaultValue),
            typeof (object),
            typeof (SwitchConverter),
            new PropertyMetadata((object)null));

        public object DefaultValue
        {
            get { return GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public SwitchConverter()
        {
            Cases = new List<SwitchConverterCase>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rv = Cases.FirstOrDefault(@case => @case.Case.Equals(value))?.Value;
            return rv ?? DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ContentProperty(nameof(Value))]
    public class SwitchConverterCase
    {
        public object Case { get; set; }
        public object Value { get; set; }

        public SwitchConverterCase() { }

        public SwitchConverterCase(int @case, object value)
        {
            Case = @case;
            Value = value;
        }
    }
}
