using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using GAF.EvaluationServer.Configuration;
using GAF.Network;
using GAF.Network.Serialization;

namespace GAF.EvaluationServer
{
	public class Program
	{
		public static int Main (String [] args)
		{
			Log.WriteHeader ();

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

				if (int.TryParse (sdPortS, out sdPort)) {
					var sdNodeEndPoint = CreateEndpoint (sdIp, Convert.ToInt32 (sdPort));

					//register service with service discovery
					var serviceDiscoveryAssemblyName = ConfigurationManager.Server.ServiceDiscovery.AssemblyName;
					var serviceDiscoveryType = ConfigurationManager.Server.ServiceDiscovery.Type;

					if (!string.IsNullOrWhiteSpace (serviceDiscoveryAssemblyName)) {
						try {
							//assembly name specified so load the assembly
							var serviceDiscovery = new ServiceDiscoveryClient (serviceDiscoveryAssemblyName, serviceDiscoveryType, sdNodeEndPoint);

							Log.Info (string.Format ("Registering GAF Evaluation Server with Service Discovery at address {0}.", sdNodeEndPoint.ToString ()));

							var serviceId = string.Format ("GAF-Server:{0}:{1}", settings.EndPoint.Address, settings.EndPoint.Port);
							serviceDiscovery.RegisterService (serviceId, settings.EndPoint, settings.EndPoint);

						} catch (Exception ex) {

							while (ex.InnerException != null) {
								ex = ex.InnerException;
							}
							Log.Warning (string.Format ("Service Discovery Registration Failed [{0}].", ex.Message));
						}
					} else {
						Log.Warning ("Service Discovery not defined... skipped.");
					}
				}

				var fitnessAssemblyName = ConfigurationManager.Server.Fitness.AssemblyName;

				Network.EvaluationServer evaluationServer = new GAF.Network.EvaluationServer (fitnessAssemblyName);
				evaluationServer.OnEvaluationComplete += OnEvaluationComplete;
				//evaluationServer.OnLogging += (object sender, GAF.Network.LoggingEventArgs e) => {
				//	Log (e.Message);
				//};

				//start the server
				evaluationServer.Start (settings.EndPoint);


			} catch (Exception ex) {
				Log.Error (ex);
			}

			//program should never return so if we get here it must be an exception
			return -1;
		}

		//private static void Log (string message)
		//{
		//	var dt = DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
		//	Console.WriteLine ("    {0} [INFO] {1}", dt, message);
		//}

		//private static void Log (Network.LoggingEventArgs logArgs)
		//{
		//	string level = string.Empty;
		//	var dt = DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
		//	var message = string.Format ("    {0} [{1}] {2}", dt, level, logArgs.Message);

		//	switch (logArgs.LoggingType) {
		//	case LoggingType.Info:
		//		level = "INFO";
		//		break;
		//	case LoggingType.Warning:
		//		level = "WARN";
		//		break;
		//	case LoggingType.Error:
		//		level = "ERR";
		//		break;
		//	default:
		//		level = "INFO";
		//		break;
		//	}

		//	Trace.TraceInformation ("Hello TraceInformation");
		//	Trace.TraceWarning ("Hello TraceWarning");
		//	Trace.TraceError ("Hello TraceError");
		//}

		//private static void Log (Exception ex)
		//{
		//	while (ex.InnerException != null) {
		//		ex = ex.InnerException;
		//	}
		//	var level = "WARN";
		//	var dt = DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
		//	Console.WriteLine ("    {0} {1} {2}", dt, level, ex.Message);
		//}

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
