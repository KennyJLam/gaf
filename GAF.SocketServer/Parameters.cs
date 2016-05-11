using System;
using System.Linq;
using System.Net;

namespace GAF.SocketServer
{
	public class Parameters
	{
		private const int defaultPort = 11000;

		public Parameters (String[] args)
		{
			if (args != null) {

				var paramPort = args.SingleOrDefault (arg => arg.StartsWith ("-port:", StringComparison.InvariantCultureIgnoreCase));
				var paramIP = args.SingleOrDefault (arg => arg.StartsWith ("-ip:", StringComparison.InvariantCultureIgnoreCase));

				//Port
				if (!string.IsNullOrEmpty (paramPort)) {
					paramPort = paramPort.Replace ("-port:", "");
					var newPort = 0;
					if (int.TryParse (paramPort, out newPort)) {
						this.Port = newPort;
					}
				} else {
					this.Port = defaultPort;
				}

				//IP Address
				if (!string.IsNullOrEmpty (paramIP)) {

					paramIP = paramIP.Replace ("-ip:", "");
					IPAddress addr;
					if (IPAddress.TryParse (paramIP, out addr)) {
						this.IPAddress = addr;
					}

				} else {
					//nothing specified so use local hostname
					IPHostEntry ipHostInfo = Dns.GetHostEntry (Dns.GetHostName ());
					this.IPAddress = ipHostInfo.AddressList [0];
				}
			}





		}

		public bool Asynchronous { get; set; }

		public int Port { get; set; }

		public IPAddress IPAddress { get; set; }

	}
}

