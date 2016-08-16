using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace GAF.Network
{
	public class ServiceDiscoveryAssembly
	{
		IServiceDiscovery _serviceDiscovery;

		public ServiceDiscoveryAssembly (string assemblyPath, IPEndPoint endpoint)
		{
			if (string.IsNullOrWhiteSpace (assemblyPath)) {
				throw new ArgumentException ("The specified path null or empty.", nameof (assemblyPath));
			}

			if (!File.Exists (assemblyPath)) {
				throw new FileNotFoundException (string.Format ("Cannot find Assembly '{0}'", assemblyPath));
			}

			var assembly = Assembly.LoadFile (assemblyPath);
			var type = typeof (IServiceDiscovery);
			var types = assembly.DefinedTypes.Where (type.IsAssignableFrom).ToList ();
			//var types = fitnessDll.GetTypes();

			if (types.Count == 0) {
				throw new ApplicationException (string.Format ("An IServiceDiscovery type connot be found within the specified assembly [{0}].", assemblyPath));
			}

			//get the first type available
			var serviceDiscoveryClass = types [0];

			_serviceDiscovery = (IServiceDiscovery)Activator.CreateInstance (serviceDiscoveryClass, endpoint);

		}

		/// <summary>
		/// Gets the active services.
		/// </summary>
		/// <returns>The active services.</returns>
		/// <param name="serviceName">Service name.</param>
		public List<IPEndPoint> GetActiveServices (string serviceName)
		{
			return _serviceDiscovery.GetActiveServices (serviceName);
		}

		/// <summary>
		/// Registers a service.
		/// </summary>
		/// <returns><c>true</c>, if service was registered, <c>false</c> otherwise.</returns>
		/// <param name="serviceId">Service identifier.</param>
		/// <param name="serviceEndPoint">Service end point.</param>
		/// <param name="checkEndPoint">Check end point.</param>
		public bool RegisterService (string serviceId, IPEndPoint serviceEndPoint, IPEndPoint checkEndPoint)
		{
			return _serviceDiscovery.RegisterService (serviceId, serviceEndPoint, checkEndPoint);
		}

		/// <summary>
		/// Deregisters a service.
		/// </summary>
		/// <returns><c>true</c>, if register service was ded, <c>false</c> otherwise.</returns>
		/// <param name="serviceId">Service identifier.</param>
		public bool DeRegisterService (string serviceId)
		{
			return _serviceDiscovery.DeRegisterService (serviceId);
		}
	}
}

