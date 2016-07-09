using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ThemeEditor.WPF.Templating
{
    public class ThicknessConverter : DependencyObject, IValueConverter
    {
        [Flags]
        public enum ThicknessSide
        {
            Top = 1,
            Left = 2,
            Right = 4,
            Bottom = 8
        }

        public static readonly DependencyProperty MultiplierProperty = DependencyProperty.Register(
            nameof(Multiplier),
            typeof (double),
            typeof (ThicknessConverter),
            new PropertyMetadata(1.0));

        public double Multiplier
        {
            get { return (double) GetValue(MultiplierProperty); }
            set { SetValue(MultiplierProperty, value); }
        }

        public static readonly DependencyProperty SidesProperty = DependencyProperty.Register(
            nameof(Sides),
            typeof(ThicknessSide),
            typeof(ThicknessConverter),
            new PropertyMetadata(default(ThicknessSide)));

        public ThicknessSide Sides
        {
            get { return (ThicknessSide)GetValue(SidesProperty); }
            set { SetValue(SidesProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness t = new Thickness();
            if (!(value is double))
                return t;

            double v = (double) value;
            v *= Multiplier;
            if (Sides.HasFlag(ThicknessSide.Top))
                t.Top = v;
            if (Sides.HasFlag(ThicknessSide.Left))
                t.Left = v;
            if (Sides.HasFlag(ThicknessSide.Right))
                t.Right = v;
            if (Sides.HasFlag(ThicknessSide.Bottom))
                t.Bottom = v;
            return t;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}