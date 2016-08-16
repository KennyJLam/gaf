using System;
using System.Configuration;
using System.Linq;
using System.Web;
//using J8.Umbraco.Diagnostics;


namespace GAF.EvaluationServer.Configuration
{
    public static class ConfigurationManager
    {
        private const string SectionName = "server";
        private const string SettingsNodeName = "settings";
		private const string FitnessNodeName = "fitness";
		private const string ServiceDiscoveryNodeName = "serviceDiscovery";

		public static string GetSetting(string key)
		{
			var element = GetSettingElement(key);
			if (element == null)
			{
				throw new ConfigurationErrorsException(string.Format(
					"<setting key='{0}'... is set incorrectly or is missing.", key));
			}

			return element.Value;
		}

		public static ServerSection Server {
			get {
				var kcConfig = System.Configuration.ConfigurationManager.GetSection (SectionName) as ServerSection;
				if (kcConfig == null) {
					throw new ApplicationException (
						string.Format ("Configuration section '{0}' cannot be found in the configuration file.", SectionName));
				}

				return kcConfig;
			}
		}

		//public static FitnessElement Fitness {
		//	get {
		//		var e = Server.Fitness;

		//		if (e == null) {
		//			throw new Exception (
		//				string.Format ("The configuration node '{0}' cannot be found in the configuration file.",
		//					FitnessNodeName));
		//		}

		//		return e;
		//	}
		//}

		//public static ServiceDiscoveryElement ServiceDiscovery {
		//	get {
		//		var e = Server.ServiceDiscovery;

		//		if (e == null) {
		//			throw new Exception (
		//				string.Format ("The configuration node '{0}' cannot be found in the configuration file.",
		//					FitnessNodeName));
		//		}

		//		return e;
		//	}
		//}

		private static SettingElement GetSettingElement(string key)
        {
            SettingElement result = null;
            var elements = GetSettingCollection();

            foreach (var element in elements)
            {
                var typedElement = element as SettingElement;
                if (typedElement != null &&
                    typedElement.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = typedElement;
                    break;
                }
            }

            return result;
        }

		private static SettingCollection GetSettingCollection ()
		{
			var settings = Server.Settings;

			if (settings == null) {
				throw new Exception (
					string.Format ("The configuration node '{0}' cannot be found in the configuration file.",
						SettingsNodeName));
			}

			return settings;
		}


    }
}



