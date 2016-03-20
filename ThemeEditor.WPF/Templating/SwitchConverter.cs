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
            get { return (List<SwitchConverterCase>) GetValue(CasesProperty); }
            set { SetValue(CasesProperty, value); }
        }

        public SwitchConverter ()
        {
            Cases = new List<SwitchConverterCase>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
                return null;
            return Cases.FirstOrDefault(@case => @case.Case == (int) value)?.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SwitchConverterCase
    {
        public int Case { get; set; }
        public object Value { get; set; }

        public SwitchConverterCase() { }

        public SwitchConverterCase(int @case, object value)
        {
            Case = @case;
            Value = value;
        }
    }
}
