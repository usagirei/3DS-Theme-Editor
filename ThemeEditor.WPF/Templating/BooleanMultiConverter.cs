using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    class BooleanMultiConverter : DependencyObject, IMultiValueConverter
    {
        public enum BooleanOperation
        {
            And,
            Or
        }

        public static readonly DependencyProperty FalseValueProperty
            = DependencyProperty.Register(nameof(FalseValue),
                typeof(object),
                typeof(BooleanMultiConverter),
                new PropertyMetadata(false));

        public static readonly DependencyProperty OperationProperty
            = DependencyProperty.Register(
                                          nameof(Operation),
                typeof(BooleanOperation),
                typeof(BooleanMultiConverter),
                new PropertyMetadata(default(BooleanOperation)));

        public static readonly DependencyProperty TrueValueProperty
            = DependencyProperty.Register(nameof(TrueValue),
                typeof(object),
                typeof(BooleanMultiConverter),
                new PropertyMetadata(true));

        public object FalseValue
        {
            get { return GetValue(FalseValueProperty); }
            set { SetValue(FalseValueProperty, value); }
        }

        public BooleanOperation Operation
        {
            get { return (BooleanOperation) GetValue(OperationProperty); }
            set { SetValue(OperationProperty, value); }
        }

        public object TrueValue
        {
            get { return GetValue(TrueValueProperty); }
            set { SetValue(TrueValueProperty, value); }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool temp;
            switch (Operation)
            {
                case BooleanOperation.And:
                    foreach (var value in values)
                    {
                        if (!(value is bool))
                            continue;

                        temp = (bool) value;
                        if (!temp)
                            return FalseValue;
                    }
                    return TrueValue;

                case BooleanOperation.Or:
                    foreach (var value in values)
                    {
                        if (!(value is bool))
                            continue;

                        temp = (bool) value;
                        if (temp)
                            return TrueValue;
                    }
                    return FalseValue;
                default:
                    return TrueValue;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }
}
