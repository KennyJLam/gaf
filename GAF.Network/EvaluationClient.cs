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

namespace GAF.Network
{
	/// <summary>
	/// Evaluation client.
	/// </summary>
	public class EvaluationClient
	{
		private Socket [] _clients;
		private List<IPEndPoint> _endPoints;
		private bool [] _reInitialiseFlags;

		private object _syncLock = new object ();
		private FitnessAssembly _fitnessAssembly;
		private string _fitnessAssemblyName;

		//private const int pidInit = 10;

		#region Task Declarations

		/// <summary>
		/// Delegate definition for the EvaluationException event handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void EvaluationExceptionHandler (object sender, ExceptionEventArgs e);

		/// <summary>
		/// Event definition for the EvaluationException event handler.
		/// </summary>
		public event EvaluationExceptionHandler OnEvaluationException;

		#endregion

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

			_endPoints = endPoints;
			_fitnessAssemblyName = fitnessAssemblyName;
			_fitnessAssembly = new FitnessAssembly (fitnessAssemblyName);

			//create a number of tasks
			int epCount = _endPoints.Count;

			_reInitialiseFlags = new bool [epCount];
		}

		/// <summary>
		/// ReInitialises the remote endpoints and re-transmits the FitnessAssembly.
		/// </summary>
		public void ReInitialise ()
		{
			for (var index = 0; index < _reInitialiseFlags.Length; index++) {
				_reInitialiseFlags[index] = true;
			}
		}

		/// <summary>
		/// Evaluate the specified solutionsToEvaluate.
		/// </summary>
		/// <param name="solutionsToEvaluate">Solutions to evaluate.</param>
		public int Evaluate (List<Chromosome> solutionsToEvaluate)
		{

			if (solutionsToEvaluate == null || solutionsToEvaluate.Count == 0) {
				throw new ArgumentNullException (nameof (solutionsToEvaluate), "The parameter is null (or empty).");
			}

			//determine how many endpoints we are to be using
			int epCount = _endPoints.Count;
			Task [] tasks = new Task [epCount];
			_clients = new Socket [epCount];

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
					//tasks [i] = RunTask (syncQueue, args.FitnessFunctionDelegate, i);
					tasks [i] = RunTask (syncQueue, RemoteFitnessDelegateFunction, i);
				}

				//wait until all tasks are complete
				Task.WaitAll (tasks);

				// set the number of evaluations we have done (one per solution)
				return solutionsToEvaluate.Count ();
			} else {
				return 0;
			}

		}

		private Task RunTask (System.Collections.Queue syncQueue, FitnessFunction fitnessFunctionDelegate, int taskId)
		{
			//create a simple task that calls the locally defined Evaluate function
			var tokenSource = new CancellationTokenSource ();
			var token = tokenSource.Token;

			//.Net 4.5 option
			//var task = Task.Run (() => EvaluateTask (syncQueue, fitnessFunctionDelegate, taskId, token), token);

			//.Net 4.0/4.5 option
			var task = new Task (() => EvaluateTask (syncQueue, fitnessFunctionDelegate, taskId, token), token);
			task.Start ();

			task.ContinueWith (t => {

				var exception = t.Exception;

				if (OnEvaluationException != null && t.Exception != null) {
					var message = new StringBuilder ();
					foreach (var ex in t.Exception.InnerExceptions) {
						message.Append (ex.Message);
						message.Append ("\r\n");
					}

					var eventArgs = new ExceptionEventArgs ("RunTask", message.ToString ());
					OnEvaluationException (this, eventArgs);
				}

			}, TaskContinuationOptions.OnlyOnFaulted);

			return task;
		}

		private void EvaluateTask (System.Collections.Queue syncQueue, FitnessFunction fitnessFunctionDelegate, int taskId, CancellationToken token)
		{

			// Establish the remote endpoint using the appropriate endpoint and socket client
			IPEndPoint remoteEndPoint = EndPoints [taskId];
			_clients [taskId] = SocketClient.Connect (remoteEndPoint);

			//send a status packet to see if we have already initialised this connection
			//i.e. passed the fitness function accross
			var statusRequestPacket = new Packet (PacketId.Status);
			var statusPacket = SocketClient.TransmitData (_clients [taskId], statusRequestPacket);

			//check the status packet and decode with the ServerStatus class.
			if (statusPacket == null) {
				throw new GAF.Exceptions.SocketException ("Status Packet was not received or was empty.");
			}

			var serverStatus = new ServerStatus (statusPacket);

			//we only reinitialise if not already initialised and serverside fitness is NOT being used
			if (!serverStatus.ServerDefinedFitness && (!serverStatus.Initialised || _reInitialiseFlags [taskId])) {

				//ok to init server with fitness etc

				//reset init flag
				_reInitialiseFlags [taskId] = false;

				//serialise the fitness function
				var functionBytes = File.ReadAllBytes (_fitnessAssemblyName);

				//send to server
				var xmitPacket = new Packet (functionBytes, PacketId.Init, Guid.Empty);
				SocketClient.TransmitData (_clients [taskId], xmitPacket);

			}

			//read the queue, each task will be doing this
			while (syncQueue.Count > 0) {

				Chromosome solution = null;

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
			SocketClient.TransmitETX (_clients [taskId]);
			SocketClient.Close (_clients [taskId]);

		}

		private double RemoteFitnessDelegateFunction (Chromosome chromosome)
		{
			double fitness = 0.0;
			Socket client = _clients [(int)chromosome.Tag];

			if (client == null || client.Connected) {

				// Convert the passed chromosome to a byte array
				var byteData = Serializer.Serialize<Chromosome> (chromosome, _fitnessAssembly.KnownTypes);
				//var testChrome = Serializer.DeSerialize<Chromosome> (byteData, _fitnessAssembly.KnownTypes);
				var xmitPacket = new Packet (byteData, PacketId.Chromosome, chromosome.Id);

				var recPacket = SocketClient.TransmitData (client, xmitPacket);

				if (recPacket != null) {
					fitness = BitConverter.ToDouble (recPacket.Data, 0);
				} else {
					throw new GAF.Exceptions.SocketException ("Data Packet was not received or was empty.");
				}

				//TODO: Look at re-writing the protocol to handle full bi-directional transfers 
				//rather than just using the returned GUID.
				if (recPacket.Header.PacketId == PacketId.Result &&
					recPacket.Header.ObjectId.ToString () != xmitPacket.Header.ObjectId.ToString ()) {
					throw new GAF.Exceptions.SocketException ("Received PacketID or ObjectId was incorrect.");
				}

			} else {
				throw new GAF.Exceptions.SocketException ("Client not connected.");
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

		/// <summary>
		/// Removes the specified end point.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveEndPointAt (int index)
		{
			lock (_syncLock) {
				_endPoints.RemoveAt (index);
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the evaluations undertaken since the class was instantiated.
		/// </summary>
		/// <value>The evaluations.</value>
		public int Evaluations { get; private set; }

		/// <summary>
		/// Gets the end points.
		/// </summary>
		/// <value>The end points.</value>
		public List<IPEndPoint> EndPoints {
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


		#endregion

	}
}
