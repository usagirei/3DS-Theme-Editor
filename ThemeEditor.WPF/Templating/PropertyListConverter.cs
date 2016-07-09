// --------------------------------------------------
// 3DS Theme Editor - PropertyListConverter.cs
// --------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

using ThemeEditor.WPF.Localization;

using DescriptionAttribute = ThemeEditor.WPF.Localization.DescriptionAttribute;
using DisplayNameAttribute = ThemeEditor.WPF.Localization.DisplayNameAttribute;

namespace ThemeEditor.WPF.Templating
{
    public class PropertyListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            Type sourceType = value.GetType();
            var props = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return props
                .OrderBy(o => o.GetCustomAttribute<OrderAttribute>()?.Order ?? int.MaxValue)
                .Where(o => o.GetCustomAttribute<VisibleAttribute>()?.Visible ?? true)
                .Select(info => new PropertyProxy(info, value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public class PropertyProxy : INotifyPropertyChanged, IDisposable
        {
            public PropertyInfo Property;
            public object Target { get; }

            public bool CanRead => Property.CanRead;
            public bool CanWrite => Property.CanWrite;

            public string Description { get; }
            public string Name { get; }

            public int Order { get; }

            public double RangeMin { get; }
            public double RangeMax { get; }

            public object Value
            {
                get
                {
                    if (CanRead)
                        return Property.GetValue(Target, null);
                    return null;
                }
                set
                {
                    if (CanWrite)
                        Property.SetValue(Target, value, null);
                }
            }

            public Type ValueType => Value.GetType();

            public bool Visible { get; set; }

            public PropertyProxy(PropertyInfo pi, object target)
            {
                Name = pi.GetCustomAttribute<DisplayNameAttribute>(false)?.DisplayName ?? pi.Name.ToUpper();
                Description = pi.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? "";
                Order = pi.GetCustomAttribute<OrderAttribute>(false)?.Order ?? int.MaxValue;
                Visible = pi.GetCustomAttribute<VisibleAttribute>(false)?.Visible ?? true;
                RangeMin = pi.GetCustomAttribute<RangeAttribute>(false)?.Minimum ?? 0;
                RangeMax = pi.GetCustomAttribute<RangeAttribute>(false)?.Maximum ?? 1;

                Property = pi;
                Target = target;
                RegisterPropertyChanged();
            }

            protected virtual void OnPropertyChanged(string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void RegisterPropertyChanged()
            {
                var npc = Target as INotifyPropertyChanged;
                if (npc != null)
                {
                    npc.PropertyChanged += TargetOnPropertyChanged;
                }
            }

            private void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == Property.Name)
                    OnPropertyChanged(nameof(Value));
            }

            private void UnregisterPropertyChanged()
            {
                var npc = Target as INotifyPropertyChanged;
                if (npc != null)
                {
                    npc.PropertyChanged -= TargetOnPropertyChanged;
                }
            }

            public void Dispose()
            {
                UnregisterPropertyChanged();
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
