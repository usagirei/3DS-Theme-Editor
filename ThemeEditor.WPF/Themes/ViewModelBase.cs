// --------------------------------------------------
// 3DS Theme Editor - ViewModelBase.cs
// --------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

using ThemeEditor.WPF.Localization;

namespace ThemeEditor.WPF.Themes
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public delegate void ViewModelChangedHandler(ViewModelChangedArgs args);

        private object _model;
        private bool _enabled = true;

        [Visible(false)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                var oldValue = _enabled;
                _enabled = value;
                RaiseViewModelChanged(nameof(Enabled), oldValue, value);
            }
        }

        [Visible(false)]
        protected object Model
        {
            get { return _model; }
            set
            {
                var oldValue = _model;
                _model = value;
                RaiseViewModelChanged(nameof(Model), oldValue, value);
            }
        }

        [Visible(false)]
        public string Tag { get; protected set; }

        protected ViewModelBase(object model, string tag)
        {
            if (model == null)
                throw new ArgumentNullException("Model Can't be null", nameof(model));
            Model = model;
            Tag = tag;

            ViewModelChanged += OnViewModelChanged;
        }

        public void InvalidateModel()
        {
            RaiseViewModelChanged(string.Empty, null, null);
        }

        protected virtual void OnViewModelChanged(ViewModelChangedArgs args) {}

        protected void RaiseViewModelChanged(string property, object oldValue, object newValue)
        {
            Debug.WriteLine($"ViewModelBase Property Changed: {property}: '{oldValue}' -> '{newValue}'");
            OnPropertyChanged(property);
            if (ViewModelChanged != null)
                ViewModelChanged.Invoke(new ViewModelChangedArgs(this, property, oldValue, newValue));
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }

        public virtual void Dispose()
        {
            ViewModelChanged -= OnViewModelChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public static event ViewModelChangedHandler ViewModelChanged;

        public class ViewModelChangedArgs : EventArgs
        {
            public object NewValue { get; private set; }
            public object OldValue { get; private set; }
            public string Property { get; private set; }
            public ViewModelBase ViewModel { get; private set; }

            public ViewModelChangedArgs(ViewModelBase model, string property, object oldValue, object newValue)
            {
                ViewModel = model;
                Property = property;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }
    }
}
