// --------------------------------------------------
// 3DS Theme Editor - DisplayNameAttribute.cs
// --------------------------------------------------

using System;
using System.Reflection;
using System.Resources;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF.Localization
{
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayNameAttribute : Attribute
    {
        private readonly bool _localize;
        private readonly PropertyInfo _managerProp;
        private readonly string _name;

        public string DisplayName
        {
            get
            {
                return _localize
                           ? Manager.GetString(_name) ?? _name
                           : _name;
            }
        }

        private ResourceManager Manager => (ResourceManager) _managerProp.GetValue(null);

        public DisplayNameAttribute(string name, Type resourcesType = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                _localize = false;
                return;
            }
            _managerProp = (resourcesType ?? typeof(Resources)).GetProperty("ResourceManager");

            this._name = name;
            this._localize = resourcesType != null;
        }
    }
}
