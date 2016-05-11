using System;
using System.Linq;
using GAF.Net;


namespace GAF.SocketServer
{

	internal class Program
	{
		private static FitnessFunction _fitnessFunction;

		public static int Main (String[] args)
		{
			try {
				//retrieves the ipAddress and port from the commant line.
				var settings = new Parameters (args);

				//get fitness function from a dll
				var cf = new ConsumerFunctions ("GAF.ConsumerFunctions.dll");
				_fitnessFunction = cf.FitnessFunction;

				//nice welcome message
				Console.WriteLine ("GAF Socket Server Listening on {0}:{1}.", 
					settings.IPAddress, 
					settings.Port);

				//create the evaluation server and subscribe to its only even
				var evaluationServer = new EvaluationServer (cf.FitnessFunction);
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

		public static void OnEvaluationComplete (object sender, RemoteEvaluationEventArgs e)
		{
		
			//this event fires each time an evaluation is undertaken by the server
			Console.WriteLine ("Processed Chromosome:{0} Fitness:{1}", e.Solution.Id, e.Solution.Fitness);

		}

	}
}