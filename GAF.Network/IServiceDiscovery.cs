using System;
using System.Collections.Generic;
using System.Net;

namespace GAF.Network
{
	public interface IServiceDiscovery
	{
		List<IPEndPoint> GetActiveServices (string serviceName);
		bool RegisterService (string serviceId, IPEndPoint serviceEndPoint, IPEndPoint checkEndPoint);
		bool DeRegisterService (string serviceId);
	}
}

