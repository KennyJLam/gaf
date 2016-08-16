using System.Configuration;

namespace GAF.EvaluationServer.Configuration
{
    public class CustomSection : ConfigurationSection
    {
        ///// <summary>
        ///// 
        ///// </summary>
        [ConfigurationProperty("fitness", IsKey = false, IsRequired = false)]
        public FitnessElement Fitness
        {
			get { return (FitnessElement)base["fitness"]; }
        }

		/// <summary>
		/// Gets the servicediscovery element.
		/// </summary>
		/// <value>The service discovery.</value>
		[ConfigurationProperty ("serviceDiscovery", IsKey = false, IsRequired = false)]
		public ServiceDiscoveryElement ServiceDiscovery {
			get { return (ServiceDiscoveryElement)base ["serviceDiscovery"]; }
		}

        //[ConfigurationProperty("dataAccessLayer")]
        //public DataAccessLayer DataAccessLayer
        //{
        //    get { return (DataAccessLayer)base["dataAccessLayer"]; }

        //}

//        /// <summary>
//        /// A collection of the configured themes
//        /// </summary>
//        [ConfigurationProperty("sites")]
//        public SiteCollection Sites
//        {
//            get { return ((SiteCollection)(base["sites"])); }
//        }

		[ConfigurationProperty("settings")]
		public SettingCollection Settings
		{
			get { return ((SettingCollection)(base["settings"])); }
		}

    }
}
