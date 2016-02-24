using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using ThemeEditor.Common.Graphics;

namespace ThemeEditor.WPF.Templating
{
    class ColorBlendConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty FadeToColorProperty = DependencyProperty.Register(
                                                                                                    "FadeToColor",
            typeof(Color),
            typeof(ColorBlendConverter),
            new PropertyMetadata(default(Color)));

        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
                                                                                                "Opacity",
            typeof(float),
            typeof(ColorBlendConverter),
            new PropertyMetadata(default(float)));

        public Color FadeToColor
        {
            get { return (Color) GetValue(FadeToColorProperty); }
            set { SetValue(FadeToColorProperty, value); }
        }

        public float Opacity
        {
            get { return (float) GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color))
                return null;
            var col = (Color) value;
            
            if (parameter is float)
                return col.Blend(FadeToColor, (float) parameter);
            if (parameter is Color)
                return col.Blend((Color) parameter, Opacity);
            if (parameter == null)
                return col.Blend(FadeToColor, Opacity);
            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
