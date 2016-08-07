using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace GAF.Network
{

	//TODO: Need to have a reference to the Fitness function within the GA somehow or passed in here?
	//see the OnEvaluationBegin handler in this class...

	/// <summary>
	/// This class is a wrapper for the GAF.GeneticAlgorithm class and provides networking funtionality.
	/// </summary>
	public class NetworkWrapper
	{
		private const string ServiceName = "gaf-evaluation-server";
		private IServiceDiscovery _serviceDiscoveryClient;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Net.GeneticAlgorithm"/> class.
		/// </summary>
		/// <param name="geneticAlgorithm">Genetic algorithm.</param>
		public NetworkWrapper (GAF.GeneticAlgorithm geneticAlgorithm, IServiceDiscovery serviceDiscoveryClient)
		{
			if (geneticAlgorithm == null)
				throw new ArgumentNullException (nameof (geneticAlgorithm));

			if (geneticAlgorithm.Population == null)
				throw new NullReferenceException ("The specified GeneticAlgorithm.Population object is null.");

			if (geneticAlgorithm.Population.Solutions == null || geneticAlgorithm.Population.Solutions.Count == 0)
				throw new NullReferenceException ("The specified GeneticAlgorithm.PopulationSolutions object is null or empty.");

			if (serviceDiscoveryClient == null) {
				throw new NullReferenceException ("The specified IServiceDiscovery object is null");
			}
			_serviceDiscoveryClient = serviceDiscoveryClient;

			//store the referenc to the GA and hook up to the evaluation begin class
			this.GeneticAlgorithm = geneticAlgorithm;
			this.GeneticAlgorithm.Population.OnEvaluationBegin += OnEvaluationBegin;

			//get the endpoints from consul
			this.EndPoints = serviceDiscoveryClient.GetActiveServices(ServiceName);

		}

		private void OnEvaluationBegin (object sender, EvaluationEventArgs args)
		{
			try {
				this.EndPoints = _serviceDiscoveryClient.GetActiveServices(ServiceName);
				//TODO: Need to get the ConsumerFunctions dll name from somewhere or move this ouside of the event
				var remoteEval = new EvaluationClient (this.EndPoints, "GAF.ConsumerFunctions.TravellingSalesman.dll");
				remoteEval.OnEvaluationException += (object s, ExceptionEventArgs e) =>
					Console.WriteLine (e.Message);

				var evaluations = remoteEval.Evaluate (args.SolutionsToEvaluate);

				if (evaluations > 0) {
					args.Evaluations = evaluations;
				} else {
					throw new ApplicationException ("No evaluations undertaken, check that a server exists.");
				}

			} catch (Exception ex) {

				while (ex.InnerException != null) {
					ex = ex.InnerException;
				}
				throw;

			} finally {

				args.Cancel = true;
			}
		}

		/// <summary>
		/// Gets a reference to the 'wrapped' GeneticAlgorithm object.
		/// </summary>
		/// <value>The genetic algorithm.</value>
		public GAF.GeneticAlgorithm GeneticAlgorithm { private set; get; }

		/// <summary>
		/// Gets the end points as retrieved via service discovery.
		/// </summary>
		/// <value>The end points.</value>
		public List<IPEndPoint> EndPoints { private set; get; }

		/// <summary>
		/// Gets the consul node end point. Use this if the local Consul Node is listening
		/// on a different IP/Port to the hosts IP and port 8500.
		/// </summary>
		/// <value>The consul node end point.</value>
		public IPEndPoint ConsulNodeEndPoint { private set; get; }

		/// <summary>
		/// Creates an endpoint from the IPAddress and Port number as a colon delimetered string.
		/// Parameter must be in the format IPAddress:PortNumber e.g. 192.168.1.64:11000.
		/// </summary>
		/// <returns>The endpoint.</returns>
		/// <param name="endpointAddress">Address with port.</param>
		public static IPEndPoint CreateEndpoint (string endpointAddress)
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
	}
}

