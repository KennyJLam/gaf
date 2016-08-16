using System.Configuration;

namespace GAF.EvaluationServer.Configuration
{
    public class SettingElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)base["key"]; }
        }

        [ConfigurationProperty("value", IsKey = false, IsRequired = true)]
        public string Value
        {
            get { return (string)base["value"]; }
        }
    }
}
