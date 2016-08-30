using System;
using System.Net;
using System.Linq;

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

			}

		}

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

