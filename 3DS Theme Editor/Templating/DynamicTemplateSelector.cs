// --------------------------------------------------
// 3DS Theme Editor - DynamicTemplateSelector.cs
// --------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace ThemeEditor.WPF.Templating
{
    // Modified from http://www.codeproject.com/Articles/418250/WPF-Based-Dynamic-DataTemplateSelector
    // Added Property Path selector to
    public class DynamicTemplateSelector : DataTemplateSelector
    {
        public static readonly DependencyProperty TemplatesProperty =
            DependencyProperty.RegisterAttached("Templates",
                typeof(TemplateCollection),
                typeof(DynamicTemplateSelector),
                new FrameworkPropertyMetadata(new TemplateCollection(), FrameworkPropertyMetadataOptions.Inherits));

        public static TemplateCollection GetTemplates(UIElement element)
        {
            return (TemplateCollection) element.GetValue(TemplatesProperty);
        }

        public static void SetTemplates(UIElement element, TemplateCollection collection)
        {
            element.SetValue(TemplatesProperty, collection);
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            if (!(container is UIElement))
                return base.SelectTemplate(item, container);

            var templates = GetTemplates((UIElement) container);
            if (templates != null && templates.Count != 0)
            {
                foreach (var template in templates)
                {
                    var toCheck = item;
                    if (!string.IsNullOrEmpty(template.PropertyName))
                    {
                        var prop = item.GetType().GetProperty(template.PropertyName);
                        toCheck = prop.GetValue(item);
                    }

                    if (template.PropertyType.IsInstanceOfType(toCheck))
                        return template.DataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
