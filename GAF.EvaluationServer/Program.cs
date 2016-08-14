using System;
using System.Collections.Generic;
using GAF.Consul;
using GAF.ConsumerFunctions.TravellingSalesman;
using GAF.Network;

namespace GAF.EvaluationServer
{
	public class Program
	{
		public static int Main (String [] args)
		{
			try {

				//retrieves the parameters from the commant line.
				var settings = new Parameters (args);

				//register service with consul
				//if (settings.ConsulNodeEndPoint != null) {

				//var endPoint = ConsulClient.CreateEndpoint ("192.168.1.90:8500");
				//var consulClient = new ConsulClient ("192.168.1.91", 8500);
				var consulClient = new ConsulClient (settings.ConsulNodeEndPoint);


				Console.WriteLine ("Registering GAF Evaluation Server with Consul Node at address {0}.",
								   consulClient.NodeEndPoint.ToString ());
				try {
					//consulClient.RegisterService (serviceDefinition);
					var serviceId = string.Format ("GAF-Server:{0}:{1}", settings.IPAddress, settings.Port);
					var endPoint = ConsulClient.CreateEndpoint (string.Format ("{0}:{1}", settings.IPAddress, settings.Port));
					var testEndPoint = ConsulClient.CreateEndpoint (string.Format ("{0}:{1}", settings.IPAddress, settings.Port));

					consulClient.RegisterService (serviceId, endPoint, testEndPoint);
					//consulClient.DeRegisterService ("");

				} catch (Exception ex) {
					while (ex.InnerException != null) {
						ex = ex.InnerException;
					}
					Console.WriteLine ("Consul Registration Failed [{0}].", ex.Message);
				}

				//nice welcome message
				Console.WriteLine ("GAF Evaluation Server Listening on {0}:{1}.",
					settings.IPAddress,
					settings.Port);

				Network.EvaluationServer evaluationServer = new GAF.Network.EvaluationServer (settings.FitnessAssemblyName);
				evaluationServer.OnEvaluationComplete += OnEvaluationComplete;

				//start the server
				evaluationServer.Start (settings.IPAddress, settings.Port);


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
			Console.WriteLine ("Evaluated solution {0}, Fitness={1}", e.Solution.Id, e.Solution.Fitness);
		}

	}
}
