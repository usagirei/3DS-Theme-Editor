using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class ComparisonConverter : DependencyObject, IValueConverter
    {
        public enum ComparisonMode
        {
            LessThan,
            Equals,
            GreaterThan
        }

        public static readonly IValueConverter Instance = new ComparisonConverter();

        public static readonly DependencyProperty ComparisonModeProperty = DependencyProperty.Register(
            nameof(Mode),
            typeof(ComparisonMode),
            typeof(ComparisonConverter),
            new PropertyMetadata(ComparisonMode.Equals));

        public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(
            nameof(TrueValue),
            typeof(object),
            typeof(ComparisonConverter),
            new PropertyMetadata(true));

        public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register(
            nameof(FalseValue),
            typeof(object),
            typeof(ComparisonConverter),
            new PropertyMetadata(false));

        public ComparisonMode Mode
        {
            get { return (ComparisonMode)GetValue(ComparisonModeProperty); }
            set { SetValue(ComparisonModeProperty, value); }
        }

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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var srcValue = value as IComparable;
            var tgtValue = parameter as IComparable;
            if (srcValue == null || tgtValue == null)
                return FalseValue;

            var compared = srcValue.CompareTo(tgtValue);
            switch (Mode)
            {
                case ComparisonMode.Equals:
                    return compared == 0 ? TrueValue : FalseValue;
                case ComparisonMode.LessThan:
                    return compared < 0 ? TrueValue : FalseValue;
                case ComparisonMode.GreaterThan:
                    return compared > 0 ? TrueValue : FalseValue;
            }
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}