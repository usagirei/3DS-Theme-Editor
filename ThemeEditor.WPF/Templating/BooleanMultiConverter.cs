using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    class MultiBooleanToVisibilityConverter : DependencyObject, IMultiValueConverter
    {
        public enum BooleanOperation
        {
            And,
            Or
        }

        public static readonly DependencyProperty OperationProperty = 
        DependencyProperty.Register(
                                                                                                  nameof(Operation),
            typeof(BooleanOperation),
            typeof(MultiBooleanToVisibilityConverter),
            new PropertyMetadata(default(BooleanOperation)));

        public BooleanOperation Operation
        {
            get { return (BooleanOperation) GetValue(OperationProperty); }
            set { SetValue(OperationProperty, value); }
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
                            return Visibility.Hidden;
                    }
                    return Visibility.Visible;

                case BooleanOperation.Or:
                    foreach (var value in values)
                    {
                        if (!(value is bool))
                            continue;

                        temp = (bool) value;
                        if (temp)
                            return Visibility.Visible;
                    }
                    return Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }
}
