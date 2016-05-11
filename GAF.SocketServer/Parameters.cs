using System;
using System.Linq;
using System.Net;

namespace GAF.SocketServer
{
	public class ParameterSettings
	{
		public ParameterSettings (String[] args)
		{
			if (args != null) {

				var paramAsync = args.SingleOrDefault (arg => arg.StartsWith ("-async", StringComparison.InvariantCultureIgnoreCase));
				var paramPort = args.SingleOrDefault (arg => arg.StartsWith ("-port:", StringComparison.InvariantCultureIgnoreCase));
				var paramIP = args.SingleOrDefault (arg => arg.StartsWith ("-ip:", StringComparison.InvariantCultureIgnoreCase));


				if (!string.IsNullOrEmpty (paramAsync)) {
					this.Asynchronous = true;
				}

				if (!string.IsNullOrEmpty (paramPort)) {
					paramPort = paramPort.Replace ("-port:", "");
					var newPort = 0;
					if (int.TryParse (paramPort, out newPort)) {
						this.Port = newPort;
					}
				}
				if (!string.IsNullOrEmpty (paramIP)) {
					try{
						paramIP = paramIP.Replace ("-ip:", "");
						IPAddress addr;
						if(IPAddress.TryParse(paramIP, out addr))
						{
							this.IPAddress = addr;
						}
					}
					catch (Exception ex) {
						Console.WriteLine (ex.Message);
					}
				}
			}





		}

		public bool Asynchronous { get; set; }
		public int Port { get; set; }
		public IPAddress IPAddress { get; set; }

	}
}

