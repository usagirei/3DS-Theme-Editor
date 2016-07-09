using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ThemeEditor.WPF.Templating
{
    public class MathConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty FunctionProperty = DependencyProperty.Register(
            nameof(Function),
            typeof(string),
            typeof(MathConverter),
            new PropertyMetadata("Sin", PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is MathConverter))
                return;

            MathConverter mc = (MathConverter)obj;

            var reflectedFunc
                = typeof(Math).GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m => m.Name == (string)args.NewValue && m.GetParameters()[0].ParameterType == typeof(double));

            if (reflectedFunc == null)
                throw new ArgumentException("Bad Function Name");

            if (reflectedFunc.GetParameters().Length == 1)
            {
                var v = Delegate.CreateDelegate(typeof(Func<double, double>), reflectedFunc, true);
                mc.SingleParam = (Func<double, double>)v;
                mc.DoubleParam = null;
                return;
            }
            if (reflectedFunc.GetParameters().Length == 2)
            {
                var v = Delegate.CreateDelegate(typeof(Func<double, double, double>), reflectedFunc, true);
                mc.SingleParam = null; ;
                mc.DoubleParam = (Func<double, double, double>)v;
                return;
            }

            if (reflectedFunc == null)
                throw new ArgumentException("Unsupported Function");
        }

        private Func<double, double> SingleParam { get; set; }
        private Func<double, double, double> DoubleParam { get; set; }

        public string Function
        {
            get { return (string)GetValue(FunctionProperty); }
            set { SetValue(FunctionProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                return null;
            double v = (double)value;
            double p = parameter as double? ?? 0;
            return SingleParam?.Invoke(v) ?? DoubleParam?.Invoke(v, p);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}