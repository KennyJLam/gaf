using System.Configuration;

namespace GAF.EvaluationServer.Configuration
{

    [ConfigurationCollection(typeof(SettingElement), AddItemName = "setting",
    CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SettingElement)(element)).Key;
        }
    }
}
