using System.Configuration;

namespace GAF.EvaluationServer.Configuration
{
    public class FitnessElement : ConfigurationElement
    {
        [ConfigurationProperty("assemblyName", IsKey = true, IsRequired = false)]
        public string AssemblyName
        {
            get { return (string)base["assemblyName"]; }
        }

    }
}
