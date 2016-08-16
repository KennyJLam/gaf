using System;
using System.Net;
using System.Linq;
using GAF.Consul;

namespace GAF.EvaluationServer
{
	public class Parameters
	{
		private const int defaultServerPort = 11000;
		private const int defaultConsulPort = 8500;

		public Parameters (String[] args)
		{
			if (args != null) {

				var paramHelp = args.SingleOrDefault (arg => arg.StartsWith ("-h", StringComparison.InvariantCultureIgnoreCase));
				var paramEndpoint = args.SingleOrDefault (arg => arg.StartsWith ("-endpoint:", StringComparison.InvariantCultureIgnoreCase));
				//var paramFitnessAssembly = args.SingleOrDefault (arg => arg.StartsWith ("-f:", StringComparison.InvariantCultureIgnoreCase));
				//var paramConsulEndpoint = args.SingleOrDefault (arg => arg.StartsWith ("-consul:", StringComparison.InvariantCultureIgnoreCase));

				//Help
				DisplayHelp = !string.IsNullOrEmpty (paramHelp);

				//Endpoint Address
				if (!string.IsNullOrEmpty (paramEndpoint)) {
					paramEndpoint = paramEndpoint.Replace ("-endpoint:", "");
					this.EndPoint = CreateEndpoint (paramEndpoint);
				} else {
					//nothing specified so use local address
					IPHostEntry ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
					this.EndPoint = new IPEndPoint (ipHostInfo.AddressList [0], defaultServerPort);
				}
				////Consumer Functions
				//if (!string.IsNullOrEmpty (paramFitnessAssembly)) {
				//	this.FitnessAssemblyName = paramFitnessAssembly.Replace ("-f:", "");
				//}

				////Consul Node Address
				//if (!string.IsNullOrEmpty (paramConsulEndpoint)) {
				//	paramConsulEndpoint = paramConsulEndpoint.Replace ("-consul:", "");
				//	this.ConsulNodeEndPoint = CreateEndpoint (paramConsulEndpoint);
				//} else {
				//	//nothing specified so use local address
				//	IPHostEntry ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
				//	this.ConsulNodeEndPoint = new IPEndPoint(ipHostInfo.AddressList [0], defaultConsulPort);
				//}

			}

		}

		//public string FitnessAssemblyName { private set; get;}

		//public IPEndPoint ConsulNodeEndPoint { private set; get; }

		public IPEndPoint EndPoint { private set; get; }

		public bool DisplayHelp { private set; get;}

		#region Helper Methods

		public IPEndPoint CreateEndpoint (string endpointAddress)
		{
			IPEndPoint ipEndPoint = null;

			if (endpointAddress.Contains (":")) {

				var epSegments = endpointAddress.Split (":".ToCharArray ());

				IPAddress addr = null;
				int port = 0;

				if (IPAddress.TryParse (epSegments [0], out addr) &&
					int.TryParse (epSegments [1], out port)) {

					ipEndPoint = new IPEndPoint (addr, port);

				}
			}

			return ipEndPoint;
		}

		#endregion
	}
}

