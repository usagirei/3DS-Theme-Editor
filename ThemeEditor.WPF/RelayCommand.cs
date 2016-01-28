// --------------------------------------------------
// 3DS Theme Editor - RelayCommand.cs
// --------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThemeEditor.WPF
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute = null;

        private readonly Action<T> _execute = null;

        /// <summary>
        ///     Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute) : this(execute, null) {}

        /// <summary>
        ///     Creates a new command with conditional execution.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute = null;

        private readonly Action _execute = null;

        /// <summary>
        ///     Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute) : this(execute, null) {}

        /// <summary>
        ///     Creates a new command with conditional execution.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    public class RelayCommandAsync<TU> : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Func<Task<TU>> _execute;

        private readonly Action<TU> _postExecute;
        private readonly Action _preExecute;

        private long _isExecuting;

        public RelayCommandAsync(
            Func<Task<TU>> execute,
            Func<bool> canExecute = null,
            Action preExecute = null,
            Action<TU> postExecute = null
            )
        {
            _preExecute = preExecute;
            _postExecute = postExecute;
            _execute = execute;
            _canExecute = canExecute ?? (() => true);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (Interlocked.Read(ref _isExecuting) != 0)
                return false;

            return _canExecute();
        }

        public async void Execute(object parameter)
        {
            Interlocked.Exchange(ref _isExecuting, 1);
            RaiseCanExecuteChanged();

            _preExecute?.Invoke();
            TU retVal;
            try
            {
                retVal = await _execute.Invoke();
            }
            finally
            {
                Interlocked.Exchange(ref _isExecuting, 0);
                RaiseCanExecuteChanged();
            }
            _postExecute?.Invoke(retVal);
        }
    }

    public class RelayCommandAsync<T, TU> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Func<T, Task<TU>> _execute;

        private readonly Action<TU> _postExecute;
        private readonly Action<T> _preExecute;

        private long _isExecuting;

        public RelayCommandAsync(
            Func<T, Task<TU>> execute,
            Func<T, bool> canExecute = null,
            Action<T> preExecute = null,
            Action<TU> postExecute = null)
        {
            _preExecute = preExecute;
            _postExecute = postExecute;
            _execute = execute;
            _canExecute = canExecute ?? (o => true);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (Interlocked.Read(ref _isExecuting) != 0)
                return false;

            return _canExecute((T) parameter);
        }

        public async void Execute(object parameter)
        {
            Interlocked.Exchange(ref _isExecuting, 1);
            RaiseCanExecuteChanged();

            _preExecute?.Invoke((T) parameter);
            TU retVal;
            try
            {
                retVal = await _execute((T) parameter);
            }
            finally
            {
                Interlocked.Exchange(ref _isExecuting, 0);
                RaiseCanExecuteChanged();
            }
            _postExecute?.Invoke(retVal);
        }
    }
}
