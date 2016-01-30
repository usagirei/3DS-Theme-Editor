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

        public static readonly DependencyProperty ComparisonModeProperty = DependencyProperty.Register(
                                                                                                       nameof(Mode),
            typeof(ComparisonMode),
            typeof(ComparisonConverter),
            new PropertyMetadata(default(ComparisonMode)));

        public static readonly IValueConverter Instance = new ComparisonConverter();

        public ComparisonMode Mode
        {
            get { return (ComparisonMode) GetValue(ComparisonModeProperty); }
            set { SetValue(ComparisonModeProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var srcValue = value as IComparable;
            var tgtValue = parameter as IComparable;
            if (srcValue == null || tgtValue == null)
                return false;

            var compared = srcValue.CompareTo(tgtValue);
            switch (Mode)
            {
                case ComparisonMode.Equals:
                    return compared == 0;
                case ComparisonMode.LessThan:
                    return compared < 0;
                case ComparisonMode.GreaterThan:
                    return compared > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}