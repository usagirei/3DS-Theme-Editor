// --------------------------------------------------
// 3DS Theme Editor - EnumListConverter.cs
// --------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Templating
{
    public class EnumListConverter : Freezable, IValueConverter
    {
        private static Dictionary<Type, List<EnumerationProxy>> _cache = new Dictionary<Type, List<EnumerationProxy>>();

        public static readonly DependencyProperty EnumTypeProperty
            = DependencyProperty.Register(nameof(EnumType),
                typeof(Type),
                typeof(EnumListConverter),
                new PropertyMetadata(default(Type)));

        public Type EnumType
        {
            get { return (Type) GetValue(EnumTypeProperty); }
            set { SetValue(EnumTypeProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new EnumListConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<EnumerationProxy> enumList;
            //ItemSource
            if (targetType == typeof(IEnumerable))
            {
                var enumType = value as Type;
                if (enumType == null)
                    return null;

                if (_cache.TryGetValue(enumType, out enumList))
                    return enumList;
                _cache[enumType]
                    = enumList
                      = enumType.GetFields(BindingFlags.Static | BindingFlags.Public)
                                .Where(f => f.GetCustomAttribute<VisibleAttribute>()?.Visible ?? true)
                                .OrderBy(f => f.GetCustomAttribute<OrderAttribute>()?.Order ?? Int32.MaxValue)
                                .Select(f => new EnumerationProxy(f.GetValue(null), enumType))
                                .ToList();
                return enumList;
            }

            // SelectedIndex
            if (targetType == typeof(Int32))
            {
                var enumType = value.GetType();
                if (!_cache.TryGetValue(enumType, out enumList))
                    return -1;
                var firstOrDefault = enumList.FirstOrDefault(proxy => proxy.Value.Equals(value));
                return enumList.IndexOf(firstOrDefault);
            }
            // SelectedItem
            if (targetType == typeof(object))
            {
                if (_cache.TryGetValue(EnumType, out enumList))
                {
                    var firstOrDefault = enumList.FirstOrDefault(proxy => proxy.Value.Equals(value));
                    return firstOrDefault;
                }
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // SelectedItem
            if (value is EnumerationProxy)
            {
                return ((EnumerationProxy) value).Value;
            }
            // SelectedIndex
            if (value is Int32)
            {
                var intValue = (int) value;
                List<EnumerationProxy> enumList;
                if (_cache.TryGetValue(EnumType, out enumList))
                {
                    value = intValue < 0 ? 0 : intValue >= enumList.Count ? enumList.Count - 1 : value;
                    return enumList[(int) value].Value;
                }
            }
            throw new NotImplementedException();
        }

        public class EnumerationProxy
        {
            public string Name { get; }
            public Type Type { get; }
            public object Value { get; }

            public EnumerationProxy(object value, Type type)
            {
                var name = value.ToString();
                Type = type;
                Value = value;
                Name = type.GetField(name).GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? name;
            }
        }
    }
}
