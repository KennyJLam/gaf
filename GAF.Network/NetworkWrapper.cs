/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;


namespace GAF.Network
{

	/// <summary>
	/// This class is a wrapper for the GAF.GeneticAlgorithm class and provides networking funtionality.
	/// </summary>
	public class NetworkWrapper
	{
		private const string ServiceName = "gaf-evaluation-server";
		private string _fitnessAssemblyName;
		private EvaluationClient _evaluationClient;
		private IServiceDiscovery _serviceDiscoveryClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.NetworkWrapper"/> class.
		/// </summary>
		/// <param name="geneticAlgorithm">Genetic algorithm.</param>
		/// <param name="serviceDiscoveryClient">Service discovery client.</param>
		/// <param name="fitnessAssemblyName">Fitness assembly name.</param>
		public NetworkWrapper (GAF.GeneticAlgorithm geneticAlgorithm, IServiceDiscovery serviceDiscoveryClient, string fitnessAssemblyName)
			: this (geneticAlgorithm, serviceDiscoveryClient, fitnessAssemblyName, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.NetworkWrapper"/> class.
		/// </summary>
		/// <param name="geneticAlgorithm">Genetic algorithm.</param>
		/// <param name="serviceDiscoveryClient">Service discovery client.</param>
		/// <param name="fitnessAssemblyName">Fitness assembly name.</param>
		/// <param name="reInitialise">If set to <c>true</c> to re-initialise the server.</param>
		/// <remarks>
		/// Re-initialising the server will, if the fitness function is not defined by the server, 
		/// re-transmit the the fitness function to the server.
		/// </remarks>
		public NetworkWrapper (GAF.GeneticAlgorithm geneticAlgorithm, IServiceDiscovery serviceDiscoveryClient, string fitnessAssemblyName, bool initialise)
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

			if (string.IsNullOrEmpty (fitnessAssemblyName)) {
				throw new NullReferenceException ("The specified fitness assembly name is null or empty");
			}
			_serviceDiscoveryClient = serviceDiscoveryClient;
			_fitnessAssemblyName = fitnessAssemblyName;

			//store the referenc to the GA and hook up to the evaluation begin class
			this.GeneticAlgorithm = geneticAlgorithm;
			this.GeneticAlgorithm.Population.OnEvaluationBegin += OnEvaluationBegin;

			//get the endpoints from consul
			Log.Info ("Getting remote endpoints from Service Discovery.");
			this.EndPoints = _serviceDiscoveryClient.GetActiveServices (ServiceName);

			if (this.EndPoints.Count == 0) {
				throw new ServiceDiscoveryException ("No server endpoints detected. Check that servers are running and registered with the appropriate IServiceDiscovery service.");
			}

			foreach (var endpoint in EndPoints) {
				Log.Info (string.Format ("Detected Endpoint: {0}:{1}", endpoint.Address, endpoint.Port));
			}

			_evaluationClient = new EvaluationClient (this.EndPoints, _fitnessAssemblyName);

			//_evaluationClient.OnEvaluationException += (object s, ExceptionEventArgs e) => {

			//	//this is the result of an error on one of the tasks and could be due to a socket exception e.g. server failure.
			//	//this should be logged and ignored as the offending socket client has already been removed from the pool
			//	//and the service discovery will put it back when it is all OK
			//	Log.Error (e.Message);

			//};

			if (initialise) {
				_evaluationClient.Initialise ();
			}

		}

		/// <summary>
		/// Re-initialises the server. If the fitness function is not defined by the server, 
		/// the fitness function will be re-transmitted to the server. 
		/// </summary>
		public void ReInitialise ()
		{
			_evaluationClient.Initialise ();
		}

		private void OnEvaluationBegin (object sender, EvaluationEventArgs args)
		{
			//this event is called each time a population is evaluated therefore
			// this will be called after each operator has been invoked
			try {

				var stopwatch = new Stopwatch ();
				stopwatch.Start ();

				if (args.SolutionsToEvaluate.Count > 0) {

					//TODO: do this every few generations rather than every generation
					//refresh the endpoints available and update the evaluationClient accordingly.
					EndPoints = _serviceDiscoveryClient.GetActiveServices (ServiceName);
					if (this.EndPoints.Count == 0) {
						throw new ServiceDiscoveryException ("No server endpoints detected. Check that servers are running and registered with the appropriate IServiceDiscovery service.");
					}

					_evaluationClient.UpdateEndpoints (EndPoints);

					foreach (var endpoint in EndPoints) {
						Log.Info (string.Format ("Detected Endpoint: {0}:{1}", endpoint.Address, endpoint.Port));
					}

					var evaluations = _evaluationClient.Evaluate (args.SolutionsToEvaluate);

					if (evaluations > 0) {
						args.Evaluations = evaluations;
					} else {
						throw new ApplicationException ("No evaluations undertaken, check that a server exists.");
					}

					stopwatch.Stop ();
					Log.Debug (string.Format ("Evaluation time = {0} ms.", stopwatch.ElapsedMilliseconds));
				}
			} catch (Exception ex) {

				while (ex.InnerException != null) {
					ex = ex.InnerException;
				}

				Log.Error (ex);

				//throw;

			} finally {
				//prevent the normall evaluation process from taking place
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
		/// Creates an endpoint from the IP address and Port number as a colon delimetered string.
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

