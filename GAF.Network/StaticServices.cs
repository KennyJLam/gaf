using System;
using System.Collections.Generic;
using System.Net;
using GAF.Network;

namespace GAF.Network
{
	public class StaticServices : IServiceDiscovery
	{
		private List<IPEndPoint> _endpoints;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.EvaluationClient.StaticServices"/> class.
		/// Uses local with the specified port as the end point.
		/// </summary>
		/// <param name="port">Port.</param>
		public StaticServices (int port)
		{
			var endpoints = new List<IPEndPoint> ();

			IPHostEntry ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
			endpoints.Add (new IPEndPoint (ipHostInfo.AddressList [0], port));

			_endpoints = endpoints;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.EvaluationClient.StaticServices"/> class.
		/// Uses the specified end points.
		/// </summary>
		/// <param name="endpoints">Endpoints.</param>
		public StaticServices (List<IPEndPoint> endpoints)
		{			
			_endpoints = endpoints;
		}

		public bool DeRegisterService (string serviceId)
		{
			throw new NotImplementedException ();
		}

		public List<IPEndPoint> GetActiveServices (string serviceName)
		{
			return _endpoints;
		}

		public bool RegisterService (string serviceId, IPEndPoint serviceEndPoint, IPEndPoint checkEndPoint)
		{
			throw new NotImplementedException ();
		}
	}
}

