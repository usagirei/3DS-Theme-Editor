// --------------------------------------------------
// 3DS Theme Editor - ViewModelRules.cs
// --------------------------------------------------

using System;
using System.Collections.Generic;

namespace ThemeEditor.WPF.Themes
{
    public class ViewModelRules : IDisposable
    {
        public delegate void RuleValidationDelegate<T, TU>(T viewModel, TU oldValue, TU newValue)
            where T : ViewModelBase;

        private readonly Dictionary<Type, Dictionary<string, Delegate>> _ruleSet;

        private readonly Stack<string> _validationStack = new Stack<string>(10);

        public bool Paused { get; set; }
        public string Tag { get; }

        public ViewModelRules(string baseTag)
        {
            Tag = baseTag;
            _ruleSet = new Dictionary<Type, Dictionary<string, Delegate>>();
            ViewModelBase.ViewModelChanged += ViewModelBaseOnViewModelChanged;
        }

        public void AddRule<T, TU>(string propertyName, RuleValidationDelegate<T, TU> validate)
            where T : ViewModelBase
        {
            Dictionary<string, Delegate> typeRules;
            if (!_ruleSet.TryGetValue(typeof(T), out typeRules))
            {
                typeRules = new Dictionary<string, Delegate>();
                _ruleSet[typeof(T)] = typeRules;
            }
            typeRules[propertyName] = validate;
        }

        public void Apply<T>(T obj)
        {
            Dictionary<string, Delegate> typeRules;
            if (!_ruleSet.TryGetValue(typeof(T), out typeRules))
                return;
            foreach (var pair in typeRules)
            {
                var prop = typeof(T).GetProperty(pair.Key);
                var val = prop.GetValue(obj);
                pair.Value.DynamicInvoke(obj, val, val);
            }
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            Paused = false;
        }

        private void ViewModelBaseOnViewModelChanged(ViewModelBase.ViewModelChangedArgs args)
        {
            if (Tag != args.ViewModel.Tag)
                return;
            // Prevent Self-Revalidation
            if (Paused || (_validationStack.Count > 0 && _validationStack.Peek() == args.Property))
                return;

            Dictionary<string, Delegate> typeRules;
            if (!_ruleSet.TryGetValue(args.ViewModel.GetType(), out typeRules))
                return;
            Delegate validateFunc;
            if (!typeRules.TryGetValue(args.Property, out validateFunc))
                return;
            _validationStack.Push(args.Property);
            validateFunc.DynamicInvoke(args.ViewModel, args.OldValue, args.NewValue);
            _validationStack.Pop();
        }

        public void Dispose()
        {
            ViewModelBase.ViewModelChanged -= ViewModelBaseOnViewModelChanged;
        }
    }
}
