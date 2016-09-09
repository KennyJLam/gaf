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


using System.Text;
using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.IO;
using System.Diagnostics;
using GAF.Network.Serialization;

namespace GAF.Network
{
	/// <summary>
	/// Evaluation client.
	/// </summary>
	public class EvaluationClient
	{
		private List<Socket> _clients;
		private List<IPEndPoint> _endPoints;
		private List<bool> _initialiseFlags;
		private List<Task> _tasks;

		private object _syncLock = new object ();
		private FitnessAssembly _fitnessAssembly;
		private string _fitnessAssemblyName;

		/// <summary>
		/// Delegate definition for the EvaluationException event handler.
		/// </summary>
		//public delegate void EvaluationExceptionHandler (object sender, ExceptionEventArgs e);

		/// <summary>
		/// Event definition for the EvaluationException event handler.
		/// </summary>
		//public event EvaluationExceptionHandler OnEvaluationException;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Net.EvaluationClient"/> class.
		/// </summary>
		/// <param name="endPoints">End points.</param>
		public EvaluationClient (List<IPEndPoint> endPoints, string fitnessAssemblyName)
		{
			if (endPoints == null) {
				throw new ArgumentNullException (nameof (endPoints), "The parameter is null.");
			}

			if (string.IsNullOrWhiteSpace (fitnessAssemblyName)) {
				throw new ArgumentException ("The specified fitness assembly name is null or empty.", nameof (fitnessAssemblyName));
			}
			EndPoints = new List<IPEndPoint> ();
			EndPoints.AddRange (endPoints);

			_fitnessAssemblyName = fitnessAssemblyName;
			_fitnessAssembly = new FitnessAssembly (fitnessAssemblyName);

			InitialiseClients ();
		}

		/// <summary>
		/// ReInitialises the remote endpoints and re-transmits the FitnessAssembly.
		/// </summary>
		public void Initialise ()
		{
			for (var index = 0; index < InitialiseFlags.Count; index++) {
				InitialiseFlags [index] = true;
			}
		}

		/// <summary>
		/// Evaluate the specified solutionsToEvaluate.
		/// </summary>
		/// <param name="solutionsToEvaluate">Solutions to evaluate.</param>
		public int Evaluate (List<Chromosome> solutionsToEvaluate)
		{

			if (solutionsToEvaluate == null) {
				throw new ArgumentNullException (nameof (solutionsToEvaluate), "The parameter is null.");
			}

			var solutionCount = solutionsToEvaluate.Count;
			if (solutionCount == 0) {
				throw new ArgumentException ("The parameter is empty.", nameof (solutionsToEvaluate));
			}

			var epCount = EndPoints.Count;

			if (epCount > 0) {

				//create a single queue and add the solutions to the queue
				var queue = new System.Collections.Queue ();
				var syncQueue = System.Collections.Queue.Synchronized (queue);

				foreach (var solution in solutionsToEvaluate) {
					syncQueue.Enqueue (solution);
				}

				//start each task passing in the queue, fitness function and that task id
				//the task id is used within the fitness function to determine a specific endpoint
				for (int i = 0; i < epCount; i++) {
					Tasks [i] = RunTask (syncQueue, RemoteFitnessDelegateFunction, i);
				}

				//wait until all tasks are complete
				Task.WaitAll (Tasks.ToArray ());

				// set the number of evaluations we have done (one per solution)
				return solutionCount;

			} else {
				return 0;
			}

		}

		///// <summary>
		///// Adds the endpoint.
		///// </summary>
		///// <param name="endpoint">Endpoint.</param>
		//public void AddEndpoint (IPEndPoint endpoint)
		//{
		//	EndPoints.Add (endpoint);
		//	InitialiseClients ();
		//}

		///// <summary>
		///// Adds the endpoints.
		///// </summary>
		///// <param name="endpoints">Endpoints.</param>
		//public void AddEndpoints (List<IPEndPoint> endpoints)
		//{
		//	EndPoints.AddRange (endpoints);
		//	InitialiseClients ();
		//}

		///// <summary>
		///// Removes the endpoint.
		///// </summary>
		//public void RemoveEndpoint (int index)
		//{
		//	EndPoints.RemoveAt (index);
		//	Clients.RemoveAt (index);
		//	InitialiseFlags.RemoveAt (index);
		//	Tasks.RemoveAt (index);
		//}

		/// <summary>
		/// Replaces existing endpoints with specified endpoints.
		/// </summary>
		/// <param name="endpoints">Endpoints.</param>
		public void UpdateEndpoints (List<IPEndPoint> endpoints)
		{
			EndPoints.Clear ();
			EndPoints.AddRange (endpoints);
			InitialiseClients ();
		}

		private void InitialiseClients ()
		{
			int epCount = EndPoints.Count;
			InitialiseFlags = new List<bool> ();
			Tasks = new List<Task> ();
			Clients = new List<Socket> ();

			InitialiseFlags.AddRange (new bool [epCount]);
			Tasks.AddRange (new Task [epCount]);
			Clients.AddRange (new Socket [epCount]);

		}

		private Task RunTask (System.Collections.Queue syncQueue, FitnessFunction fitnessFunctionDelegate, int taskId)
		{
			//create a simple task that calls the locally defined Evaluate function
			var tokenSource = new CancellationTokenSource ();
			var token = tokenSource.Token;

			Log.Info (string.Format ("Starting Task {0}", taskId));
			//.Net 4.5 option
			//var task = Task.Run (() => EvaluateTask (syncQueue, fitnessFunctionDelegate, taskId, token), token);

			//.Net 4.0/4.5 option
			var task = new Task (() => EvaluateTask (syncQueue, fitnessFunctionDelegate, taskId, token), token);
			task.Start ();

			Task continuationTask = task.ContinueWith (t => {

				var message = new StringBuilder ();
				foreach (var ex in t.Exception.InnerExceptions) {
					message.Append (ex.Message);
					message.Append ("\r\n");
				}

				Log.Error (message.ToString ());
				Log.Error (string.Format ("Endpoint {0}:{1} failed.", EndPoints [taskId].Address, EndPoints [taskId].Port));

				//TODO: could remove endpoint but service discovery would probably just re-add it until service discovery
				// detected the failure so leave this to service discovery.
				// 
				//     RemoveEndpoint (taskId);


			}, TaskContinuationOptions.OnlyOnFaulted);

			//if we get here, the task has failed, perhaps due to a socket exception/server failure etc.
			//the continuation task now needs to wait until all of the others have completed.
			//continuationTask.Wait ();

			return task;
		}

		private void EvaluateTask (System.Collections.Queue syncQueue, FitnessFunction fitnessFunctionDelegate, int taskId, CancellationToken token)
		{
			Chromosome solution = null;
			//this task will run until the queue is emptied
			try {

				// Establish the remote endpoint using the appropriate endpoint and socket client
				IPEndPoint remoteEndPoint = EndPoints [taskId];
				Clients [taskId] = SocketClient.Connect (remoteEndPoint);


				//send a status packet to see if we have already initialised this connection
				//i.e. passed the fitness function accross

				var statusRequestPacket = new Packet (PacketId.Status);
				var statusPacket = SocketClient.TransmitData (Clients [taskId], statusRequestPacket);

				//check the status packet and decode with the ServerStatus class.
				if (statusPacket == null) {
					throw new GAF.Network.SocketException ("Status Packet was not received or was empty.");
				}

				var serverStatus = new ServerStatus (statusPacket);

				//we only reinitialise if not already initialised and serverside fitness is NOT being used
				if (!serverStatus.ServerDefinedFitness && (!serverStatus.Initialised || InitialiseFlags [taskId])) {

					//ok to init server with fitness etc
					Log.Debug ("Sending the fitness function to the server.");

					//reset init flag
					InitialiseFlags [taskId] = false;

					//serialise the fitness function
					var functionBytes = File.ReadAllBytes (_fitnessAssemblyName);

					//send to server
					var xmitPacket = new Packet (functionBytes, PacketId.Init, Guid.Empty);
					SocketClient.TransmitData (Clients [taskId], xmitPacket);

				}


				//read the queue, each task will be doing this
				while (syncQueue.Count > 0) {

					//reset the 'solution' variable in case we have an exception
					//we do not want this to point to a previous solution as we will
					//use it to re-queue a solution that failed an evaluation
					solution = null;

					//take a solution from the queue
					//this can cause an exception if the queue is emptied
					//between the count (above) and the next statement
					try {
						solution = (Chromosome)syncQueue.Dequeue ();
					} catch {
						break;
					}

					//add the task Id, this is used in the fitness function 
					//to determine a suitable endpoint
					solution.Tag = taskId;
					if (token.IsCancellationRequested) {
						break;
					}

					//evaluate the solution in the normal way however pass the locally defined 
					//'Remote Fitness Function' this will initiate a remote connection to the
					//real fitness function at the server end.
					solution.Evaluate (fitnessFunctionDelegate);

				}

				//all done so send ETX to inform the server
				SocketClient.TransmitETX (Clients [taskId]);
				SocketClient.Close (Clients [taskId]);

			} catch (Exception ex) {
				//things went wrong so requeue for another attempt.
				if (solution != null) {
					Log.Error (string.Format ("{0}, re-queuing solution {1}.", ex.Message, solution.Id));
					syncQueue.Enqueue (solution);
					return;
				}
			} finally {

			}

		}

		private double RemoteFitnessDelegateFunction (Chromosome chromosome)
		{
			double fitness = 0.0;
			Socket client = Clients [(int)chromosome.Tag];

			if (client == null || client.Connected) {

				// Convert the passed chromosome to a byte array
				//actually just the genes are serialised as we dont need to sent the rest
				//var byteData = Serializer.Serialize<Chromosome> (chromosome, _fitnessAssembly.KnownTypes);
				var byteData = Binary.Serialize<List<Gene>> (chromosome.Genes, _fitnessAssembly.KnownTypes);

				var xmitPacket = new Packet (byteData, PacketId.Data, chromosome.Id);

				var recPacket = SocketClient.TransmitData (client, xmitPacket);
				if (recPacket != null) {
					fitness = BitConverter.ToDouble (recPacket.Data, 0);

				} else {
					throw new GAF.Network.SocketException ("Data Packet was not received or was empty.");
				}

				//TODO: Look at re-writing the protocol to handle full bi-directional transfers 
				//rather than just using the returned GUID.
				if (recPacket.Header.PacketId == PacketId.Result &&
					recPacket.Header.ObjectId.ToString () != xmitPacket.Header.ObjectId.ToString ()) {
					throw new GAF.Network.SocketException ("Received PacketID or ObjectId was incorrect.");
				}

			} else {
				throw new GAF.Network.SocketException ("Client not connected.");
			}

			return fitness;
		}


		#region Static Methods

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

		#endregion

		#region Properties

		/// <summary>
		/// Gets the evaluations undertaken since the class was instantiated.
		/// </summary>
		/// <value>The evaluations.</value>
		public int Evaluations { get; private set; }

		#endregion

		#region Private Properties

		/// <summary>
		/// Gets the end points.
		/// </summary>
		/// <value>The end points.</value>
		private List<IPEndPoint> EndPoints {
			get {
				lock (_syncLock) {
					return _endPoints;
				}
			}
			set {
				lock (_syncLock) {
					_endPoints = value;
				}
			}
		}

		private List<Socket> Clients {
			get {
				lock (_syncLock) {
					return _clients;
				}
			}
			set {
				lock (_syncLock) {
					_clients = value;
				}
			}
		}

		private List<bool> InitialiseFlags {
			get {
				lock (_syncLock) {
					return _initialiseFlags;
				}
			}
			set {
				lock (_syncLock) {
					_initialiseFlags = value;
				}
			}
		}

		private List<Task> Tasks {
			get {
				lock (_syncLock) {
					return _tasks;
				}
			}
			set {
				lock (_syncLock) {
					_tasks = value;
				}
			}
		}

		#endregion

	}
}
