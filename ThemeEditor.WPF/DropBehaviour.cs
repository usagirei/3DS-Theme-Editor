// --------------------------------------------------
// 3DS Theme Editor - DropBehaviour.cs
// --------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace ThemeEditor.WPF
{
    public static class DragDropBehaviour
    {
        public static readonly DependencyProperty DragDropCommandProperty
            = DependencyProperty.RegisterAttached("DragDropCommand",
                typeof(ICommand),
                typeof(DragDropBehaviour),
                new PropertyMetadata(default(ICommand), OnPreviewDropChanged));

        public static readonly DependencyProperty DragEnterCommandProperty
            = DependencyProperty.RegisterAttached("DragEnterCommand",
                typeof(ICommand),
                typeof(DragDropBehaviour),
                new PropertyMetadata(default(ICommand), OnDragEnterChanged));

        public static ICommand GetDragDropCommand(DependencyObject element)
        {
            return (ICommand) element.GetValue(DragDropCommandProperty);
        }

        public static ICommand GetDragEnterCommand(DependencyObject element)
        {
            return (ICommand) element.GetValue(DragEnterCommandProperty);
        }

        public static void SetDragDropCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(DragDropCommandProperty, value);
        }

        public static void SetDragEnterCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(DragEnterCommandProperty, value);
        }

        private static void OnDragEnter(object sender, DragEventArgs args)
        {
            var command = GetDragEnterCommand((DependencyObject) sender);
            if (command?.CanExecute(args) ?? false)
                command.Execute(args);
            else
                args.Effects = DragDropEffects.None;
            args.Handled = true;
        }

        private static void OnDragEnterChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var uiElement = elem as UIElement;
            if (uiElement == null)
                return;

            uiElement.PreviewDragEnter += OnDragEnter;
        }

        private static void OnDrop(object sender, DragEventArgs args)
        {
            var command = GetDragDropCommand((DependencyObject) sender);
            if (command?.CanExecute(args) ?? false)
                command.Execute(args);
            else
                args.Effects = DragDropEffects.None;
            args.Handled = true;
        }

        private static void OnPreviewDropChanged(
            DependencyObject elem
            ,
            DependencyPropertyChangedEventArgs args)
        {
            var uiElement = elem as UIElement;
            if (uiElement == null)
                return;

            uiElement.PreviewDrop += OnDrop;
        }
    }
}
