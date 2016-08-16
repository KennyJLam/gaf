using System.Configuration;

namespace GAF.EvaluationServer.Configuration
{
    public class FitnessAssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

    }
}
