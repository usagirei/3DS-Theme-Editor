using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using ThemeEditor.WPF.Properties;

namespace ThemeEditor.WPF
{
    class XmlSettingsProvider : SettingsProvider
    {
        public override string ApplicationName
        {
            get { return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location); }
            set { }
        }

        public string SettingsPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            "Config.xml");

        public static object XmlDeserializeProperty(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static void XmlSerializeProperty(XmlWriter writer, SettingsPropertyValue value)
        {
            var serializer = new XmlSerializer(Settings.Default[value.Name].GetType());
            serializer.Serialize(writer, value.PropertyValue);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(
            SettingsContext context,
            SettingsPropertyCollection collection)
        {
            var values = new SettingsPropertyValueCollection();
            foreach (SettingsProperty property in collection)
            {
                var value = new SettingsPropertyValue(property) {IsDirty = false};
                values.Add(value);
            }
            if (!File.Exists(SettingsPath))
                return values;
            try
            {
                using (var xtr = new XmlTextReader(SettingsPath))
                {
                    var xDoc = new XmlDocument();
                    xDoc.Load(xtr);
                    var settingsNode = xDoc["Settings"];
                    if (settingsNode == null)
                        return values;
                    foreach (XmlNode node in settingsNode.GetElementsByTagName("Setting"))
                    {
                        var name = node.Attributes?["Name"]?.Value;
                        if (string.IsNullOrEmpty(name))
                            continue;
                        var property = values[name];
                        if (property == null)
                            continue;
                        var value = node.InnerXml;
                        var dataType = property.Property.PropertyType;
                        property.PropertyValue = XmlDeserializeProperty(value, dataType);
                    }
                }
            }
            catch (XmlException)
            {
                // Ignore
            }
            return values;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(ApplicationName, config);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            using (var xtw = new XmlTextWriter(SettingsPath, Encoding.UTF8))
            {
                xtw.Formatting = Formatting.Indented;
                xtw.WriteStartDocument();
                xtw.WriteStartElement("Settings");
                foreach (SettingsPropertyValue propertyValue in collection)
                {
                    if (!IsUserScope(propertyValue.Property))
                        continue;
                    xtw.WriteStartElement("Setting");
                    xtw.WriteAttributeString("Name", propertyValue.Name);
                    XmlSerializeProperty(xtw, propertyValue);
                    xtw.WriteEndElement();
                }
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
            }
        }

        private bool IsUserScope(SettingsProperty property)
        {
            return property.Attributes.ContainsKey(typeof(UserScopedSettingAttribute));
        }
    }
}
