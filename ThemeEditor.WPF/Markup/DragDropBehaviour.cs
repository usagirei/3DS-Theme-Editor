// --------------------------------------------------
// 3DS Theme Editor - DropBehaviour.cs
// --------------------------------------------------

using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace ThemeEditor.WPF.Markup
{
    public static class DragDropBehaviour
    {
        public static readonly DependencyProperty DragDropCommandProperty
            = DependencyProperty.RegisterAttached("DragDropCommand",
                typeof(ICommand),
                typeof(DragDropBehaviour),
                new PropertyMetadata(default(ICommand), OnDropChanged));

        public static readonly DependencyProperty DragEnterCommandProperty
            = DependencyProperty.RegisterAttached("DragEnterCommand",
                typeof(ICommand),
                typeof(DragDropBehaviour),
                new PropertyMetadata(default(ICommand), OnDragEnterChanged));

        public static readonly DependencyProperty DragLeaveCommandProperty
            = DependencyProperty.RegisterAttached("DragLeaveCommand",
                typeof(ICommand),
                typeof(DragDropBehaviour),
                new PropertyMetadata(default(ICommand), OnDragLeaveChanged));

        public static readonly DependencyProperty DragOverCommandProperty
      = DependencyProperty.RegisterAttached("DragOverCommand",
          typeof(ICommand),
          typeof(DragDropBehaviour),
          new PropertyMetadata(default(ICommand), OnDragOverChanged));



        public static ICommand GetDragLeaveCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(DragLeaveCommandProperty);
        }
        public static ICommand GetDragOverCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(DragOverCommandProperty);
        }

        public static ICommand GetDragDropCommand(DependencyObject element)
        {
            return (ICommand) element.GetValue(DragDropCommandProperty);
        }

        public static ICommand GetDragEnterCommand(DependencyObject element)
        {
            return (ICommand) element.GetValue(DragEnterCommandProperty);
        }

        public static void SetDragLeaveCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(DragLeaveCommandProperty, value);
        }

        public static void SetDragOverCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(DragOverCommandProperty, value);
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
            {
                command.Execute(args);
            }
            else
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
            }
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
            {
                command.Execute(args);
            }
            else
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
            }
        }

        private static void OnDragOverChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var uiElement = elem as UIElement;
            if (uiElement == null)
                return;

            uiElement.PreviewDragOver += OnDragOver;
        }

        private static void OnDragOver(object sender, DragEventArgs args)
        {
            var command = GetDragOverCommand((DependencyObject) sender);
            if (command?.CanExecute(args) ?? false)
            {
                command.Execute(args);
            }
            else
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
            }
        }

        private static void OnDropChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var uiElement = elem as UIElement;
            if (uiElement == null)
                return;

            uiElement.PreviewDrop += OnDrop;
        }

        private static void OnDragLeave(object sender, DragEventArgs args)
        {
            var command = GetDragLeaveCommand((DependencyObject)sender);
            if (command?.CanExecute(args) ?? false)
            {
                command.Execute(args);
            }
            else
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
            }
        }

        private static void OnDragLeaveChanged(DependencyObject elem, DependencyPropertyChangedEventArgs args)
        {
            var uiElement = elem as UIElement;
            if (uiElement == null)
                return;

            uiElement.PreviewDragLeave += OnDragLeave;
        }
    }
}
