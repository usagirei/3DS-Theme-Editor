// --------------------------------------------------
// 3DS Theme Editor - Template.cs
// --------------------------------------------------

using System;
using System.Windows;

namespace ThemeEditor.WPF.Templating
{
    public class Template : DependencyObject
    {
        public static readonly DependencyProperty DataTemplateProperty
            = DependencyProperty.Register(nameof(DataTemplate),
                typeof(DataTemplate),
                typeof(Template));

        public static readonly DependencyProperty PropertyNameProperty
            = DependencyProperty.Register(nameof(PropertyName),
                typeof(string),
                typeof(Template),
                new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PropertyTypeProperty
            = DependencyProperty.Register(nameof(PropertyType),
                typeof(Type),
                typeof(Template));

        public DataTemplate DataTemplate
        {
            get { return (DataTemplate) GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        public string PropertyName
        {
            get { return (string) GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public Type PropertyType
        {
            get { return (Type) GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }
    }
}
