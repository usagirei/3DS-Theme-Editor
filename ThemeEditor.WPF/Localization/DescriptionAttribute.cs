// --------------------------------------------------
// 3DS Theme Editor - DescriptionAttribute.cs
// --------------------------------------------------

using System;
using System.Reflection;
using System.Resources;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF.Localization
{
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionAttribute : Attribute
    {
        private readonly string _desc;

        private readonly bool _localize;
        private readonly PropertyInfo _managerProp;

        public string Description
        {
            get
            {
                return _localize
                           ? Manager.GetString(_desc) ?? _desc
                           : _desc;
            }
        }

        private ResourceManager Manager => (ResourceManager) _managerProp.GetValue(null);

        public DescriptionAttribute(string desc, Type resourcesType = null)
        {
            if (string.IsNullOrEmpty(desc))
            {
                _localize = false;
                return;
            }
            _managerProp = (resourcesType ?? typeof(Resources)).GetProperty("ResourceManager");

            this._desc = desc;
            this._localize = resourcesType != null;
        }
    }
}
