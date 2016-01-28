// --------------------------------------------------
// 3DS Theme Editor - RenderToolCache.cs
// --------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;

namespace ThemeEditor.WPF.RenderTools
{
    public class RenderToolFactory
    {
        private readonly IDictionary<object, Freezable> _toolCache = new Dictionary<object, Freezable>();

        private readonly IDictionary<Type, Func<object, Freezable>> _toolFactories =
            new Dictionary<Type, Func<object, Freezable>>();

        public TResult GetTool<TResult>(object toolKey)
            where TResult : Freezable
        {
            Freezable result;

            if (_toolCache.TryGetValue(toolKey, out result))
                return (TResult) result;
            result = _toolFactories[toolKey.GetType()](toolKey);
            _toolCache.Add(toolKey, result);

            return (TResult) result;
        }

        public void RegisterTool<TKey, TResult>(Func<TKey, TResult> toolFactory)
            where TResult : Freezable
        {
            _toolFactories[typeof(TKey)] = p =>
            {
                var tool = toolFactory((TKey) p);
                tool.Freeze();
                return tool;
            };
        }
    }
}
