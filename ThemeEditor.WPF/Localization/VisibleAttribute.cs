// --------------------------------------------------
// 3DS Theme Editor - VisibleAttribute.cs
// --------------------------------------------------

using System;

namespace ThemeEditor.WPF.Localization
{
    [AttributeUsage(AttributeTargets.All)]
    public class VisibleAttribute : Attribute
    {
        public bool Visible { get; }

        public VisibleAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
