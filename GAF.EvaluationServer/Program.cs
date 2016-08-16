using System;
using System.Collections.Generic;
using System.Net;
using GAF.Consul;
using GAF.ConsumerFunctions.TravellingSalesman;
using GAF.EvaluationServer.Configuration;
using GAF.Network;

namespace GAF.EvaluationServer
{
	public class Program
	{
		public static int Main (String [] args)
		{
			try {

				//retrieve the parameters from the commant line.
				var settings = new Parameters (args);
				if (settings.DisplayHelp) {
					Console.WriteLine (Resources.Usage);
					return 0;
				}

				var sdIp = ConfigurationManager.Server.ServiceDiscovery.IpAddress;
				var sdPortS = ConfigurationManager.Server.ServiceDiscovery.Port;
				int sdPort = 0;

				if (int.TryParse (sdPortS, out sdPort))
				{
					var sdNodeEndPoint = CreateEndpoint (sdIp, Convert.ToInt32 (sdPort));

					//register service with service discovery

					var serviceDiscoveryAssemblyName = ConfigurationManager.Server.ServiceDiscovery.AssemblyName;
					if (!string.IsNullOrWhiteSpace (serviceDiscoveryAssemblyName))
					{
						try {
							//assembly name specified so load the assembly
							var serviceDiscovery = new ServiceDiscoveryAssembly (serviceDiscoveryAssemblyName, sdNodeEndPoint);

							Console.WriteLine ("Registering GAF Evaluation Server with Service Discovery at address {0}.", sdNodeEndPoint.ToString ());
							var serviceId = string.Format ("GAF-Server:{0}:{1}", settings.EndPoint.Address, settings.EndPoint.Port);
							serviceDiscovery.RegisterService (serviceId, settings.EndPoint, settings.EndPoint);

						} catch (Exception ex) {

							while (ex.InnerException != null) {
								ex = ex.InnerException;
							}
							Console.WriteLine ("Service Discovery Registration Failed [{0}].", ex.Message);

						}
					}
				}

				//nice welcome message
				Console.WriteLine ("GAF Evaluation Server Listening on {0}:{1}.",
					settings.EndPoint.Address,
					settings.EndPoint.Port);

				var fitnessAssemblyName = ConfigurationManager.Server.Fitness.AssemblyName;
				Network.EvaluationServer evaluationServer = new GAF.Network.EvaluationServer (fitnessAssemblyName);
				evaluationServer.OnEvaluationComplete += OnEvaluationComplete;

				//start the server
				evaluationServer.Start (settings.EndPoint);


			} catch (Exception ex) {

				while (ex.InnerException != null) {
					ex = ex.InnerException;
				}
				Console.WriteLine (ex.Message);
			}

			return 0;
		}

		public static void OnEvaluationComplete (object sender, GAF.Network.RemoteEvaluationEventArgs e)
		{
			//this event fires each time an evaluation is undertaken by the server
			//Console.WriteLine ("Evaluated solution {0}, Fitness={1}", e.Solution.Id, e.Solution.Fitness);
		}

		public static IPEndPoint CreateEndpoint (string ipAddress, int port)
		{
			IPEndPoint ipEndPoint = null;

			if (!string.IsNullOrWhiteSpace (ipAddress) && port > 0) {

				IPAddress addr = null;
				if (IPAddress.TryParse (ipAddress, out addr)) {

					ipEndPoint = new IPEndPoint (addr, port);

				}
			}

			return ipEndPoint;
		}
	}
}
