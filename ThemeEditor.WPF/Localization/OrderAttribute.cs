// --------------------------------------------------
// 3DS Theme Editor - OrderAttribute.cs
// --------------------------------------------------

using System;
using System.Reflection;
using System.Resources;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF.Localization
{
    [AttributeUsage(AttributeTargets.All)]
    public class OrderAttribute : Attribute
    {
        private readonly bool _localize;
        private readonly PropertyInfo _managerProp;
        private readonly string _order;

        private ResourceManager Manager => (ResourceManager) _managerProp.GetValue(null);

        public int Order
        {
            get
            {
                var value = _localize
                                ? Manager.GetString(_order) ?? "0"
                                : _order;
                int outValue;
                return int.TryParse(value, out outValue) ? outValue : 0;
            }
        }

        public OrderAttribute(int order)
        {
            _order = Convert.ToString(order);
        }

        public OrderAttribute(string order, Type resourcesType = null)
        {
            if (string.IsNullOrEmpty(order))
            {
                _localize = false;
                _order = "0";
                return;
            }
            _managerProp = (resourcesType ?? typeof(Resources)).GetProperty("ResourceManager");

            this._order = order;
            this._localize = resourcesType != null;
        }
    }
}
