// --------------------------------------------------
// 3DS Theme Editor - ColorItemConverter.cs
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

using Xceed.Wpf.Toolkit;

namespace ThemeEditor.WPF.Templating
{
    public class ColorItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var enumerable = value as IEnumerable<Color>;
            if (enumerable == null)
                return null;
            return new ObservableCollection<ColorItem>(
                enumerable.Select(
                                  (color, i) => new ColorItem(color, color.ToString())
                    )
                );
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
