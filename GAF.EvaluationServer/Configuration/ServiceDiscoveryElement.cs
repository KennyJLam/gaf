using System.Configuration;

namespace GAF.EvaluationServer.Configuration
{
    public class ServiceDiscoveryElement : ConfigurationElement
    {
        [ConfigurationProperty("assemblyName", IsKey = true, IsRequired = false)]
        public string AssemblyName
        {
            get { return (string)base["assemblyName"]; }
        }

        [ConfigurationProperty("ipAddress", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
        }

		[ConfigurationProperty ("port", IsKey = false, IsRequired = false)]
		public string Port {
			get { return (string)base ["port"]; }
		}
    }
}
