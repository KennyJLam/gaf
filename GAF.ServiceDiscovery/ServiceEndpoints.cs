/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

using System;
using System.Collections.Generic;
using System.Net;
using GAF.Network;

namespace GAF.ServiceDiscovery
{
	public class ServiceEndpoints : IServiceDiscovery
	{
		private List<IPEndPoint> _endpoints;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.EvaluationClient.StaticServices"/> class.
		/// Uses local with the specified port as the end point.
		/// </summary>
		/// <param name="port">Port.</param>
		public ServiceEndpoints (int port)
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
		public ServiceEndpoints (List<IPEndPoint> endpoints)
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

