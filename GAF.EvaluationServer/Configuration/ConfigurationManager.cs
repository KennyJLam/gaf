using System;
using System.Configuration;
using System.Linq;
using System.Web;
//using J8.Umbraco.Diagnostics;


namespace WebUI.Configuration
{
    public static class ConfigurationManager
    {
        private const string SectionName = "customSection";
        private const string SettingsNodeName = "settings";
		private const string RedirectsNodeName = "redirects";



        public static T GetSetting<T>(string key)
        {
            if(typeof (T) == typeof(string))
            {
                return (T)(object)GetSettingAsString(key);
            }
            if (typeof(T) == typeof(int))
            {
                return (T)(object)GetSettingAsInteger(key);
            }
            if (typeof(T) == typeof(double))
            {
                return (T)(object)GetSettingAsDouble(key);
            }
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)GetSettingAsBool(key);
            }

            throw new ConfigurationErrorsException("A conversion for the specified C# Type, could not be found.");
        }
			
		public static string GetRedirect(string oldUrl)
		{
			var element = GetRedirectElement (oldUrl);

			//redirect is allowed to return null
			if (element == null) {
				return null;
			} else {
				return element.NewUrl;
			}
		}

        #region Private Methods

		private static string GetSettingAsString(string key)
		{
			var element = GetSettingElement(key);
			if (element == null)
			{
				throw new ConfigurationErrorsException(string.Format(
					"<setting key='{0}'... is set incorrectly.", key));
			}

			return element.Value;
		}

		private static int GetSettingAsInteger(string key)
		{
			var setting = GetSettingAsString(key);
			var result = -1;

			if (string.IsNullOrWhiteSpace(setting))
			{

				if (!int.TryParse(setting, out result))
				{
					throw new ConfigurationErrorsException(string.Format(
						"<setting key='{0}'... is set incorrectly.", key));
				}
			}

			return result;
		}

		private static double GetSettingAsDouble(string key)
		{
			var setting = GetSettingAsString(key);
			var result = -1.0;

			if (string.IsNullOrWhiteSpace(setting))
			{

				if (!double.TryParse(setting, out result))
				{
					throw new ConfigurationErrorsException(string.Format(
						"<setting key='{0}'... is set incorrectly.", key));
				}
			}

			return result;
		}

		private static bool GetSettingAsBool(string key)
		{
			var setting = GetSettingAsString(key);
			var result = false;

			if (string.IsNullOrWhiteSpace(setting))
			{

				if (!bool.TryParse(setting, out result))
				{
					throw new ConfigurationErrorsException(string.Format(
						"<setting key='{0}'... is set incorrectly.", key));
				}
			}

			return result;
		}

        private static SettingCollection GetSettingCollection()
        {
            var section = GetConfigSection();
			var settings = section.Settings;

			if (settings == null)
            {
                throw new Exception(
                    string.Format("The configuration node '{0}' cannot be found in the configuration file.",
						SettingsNodeName));
            }

			return settings;
        }
			
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

		private static RedirectCollection GetRedirectCollection()
		{
			var section = GetConfigSection();
			var redirects = section.Redirects;

			if (redirects == null)
			{
				throw new Exception(
					string.Format("The configuration node '{0}' cannot be found in the configuration file.",
						RedirectsNodeName));
			}

			return redirects;
		}

		private static RedirectElement GetRedirectElement(string oldUrl)
		{
			RedirectElement result = null;
			var elements = GetRedirectCollection();

			foreach (var element in elements)
			{
				var typedElement = element as RedirectElement;
				if (typedElement != null &&
					typedElement.OldUrl.Equals(oldUrl, StringComparison.InvariantCultureIgnoreCase))
				{
					result = typedElement;
					break;
				}
			}

			return result;
		}

        private static CustomSection GetConfigSection()
        {
            var kcConfig = System.Configuration.ConfigurationManager.GetSection(SectionName) as CustomSection;
            if (kcConfig == null)
            {
                throw new ApplicationException(
                    string.Format("Configuration section '{0}' cannot be found in the configuration file.", SectionName));
            }

            return kcConfig;
        }

        #endregion

    }
}



