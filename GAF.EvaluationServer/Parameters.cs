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

				var paramPort = args.SingleOrDefault (arg => arg.StartsWith ("-port:", StringComparison.InvariantCultureIgnoreCase));
				var paramIP = args.SingleOrDefault (arg => arg.StartsWith ("-ip:", StringComparison.InvariantCultureIgnoreCase));
				var paramFitnessAssembly = args.SingleOrDefault (arg => arg.StartsWith ("-f:", StringComparison.InvariantCultureIgnoreCase));
				var paramConsul = args.SingleOrDefault (arg => arg.StartsWith ("-consul:", StringComparison.InvariantCultureIgnoreCase));

				//Port
				if (!string.IsNullOrEmpty (paramPort)) {
					paramPort = paramPort.Replace ("-port:", "");
					var newPort = 0;
					if (int.TryParse (paramPort, out newPort)) {
						this.Port = newPort;
					}
				} else {
					this.Port = defaultServerPort;
				}

				//IP Address
				if (!string.IsNullOrEmpty (paramIP)) {

					paramIP = paramIP.Replace ("-ip:", "");
					IPAddress addr;
					if (IPAddress.TryParse (paramIP, out addr)) {
						this.IPAddress = addr;
					}

				} else {
					//nothing specified so use local address
					IPHostEntry ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
					this.IPAddress = ipHostInfo.AddressList [0];
				}

				//Consumer Functions
				if (!string.IsNullOrEmpty (paramFitnessAssembly)) {
					this.FitnessAssemblyName = paramFitnessAssembly.Replace ("-f:", "");
				}

				//Consul Node Address
				if (!string.IsNullOrEmpty (paramConsul)) {
					paramConsul = paramConsul.Replace ("-consul:", "");
					this.ConsulNodeEndPoint = CreateEndpoint (paramConsul);
				} else {
					//nothing specified so use local address
					IPHostEntry ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
					this.ConsulNodeEndPoint = new IPEndPoint(ipHostInfo.AddressList [0], defaultConsulPort);
				}

			}

		}

		public int Port { get; set; }

		public IPAddress IPAddress { get; set; }

		public string FitnessAssemblyName { set; get;}

		public IPEndPoint ConsulNodeEndPoint { set; get; }


		#region Helper Methods

		private IPEndPoint CreateEndpoint (string endpointAddress)
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

