using System;

namespace ThemeEditor.WPF.Localization
{
    [AttributeUsage(AttributeTargets.All)]
    public class RangeAttribute : Attribute
    {
        public double Minimum { get;  }
        public double Maximum { get; }
    
        public RangeAttribute(double min, double max)
        {
            Minimum = min;
            Maximum = max;
        }
    }
}