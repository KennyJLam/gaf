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
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.IO;
using System.Diagnostics;
using GAF.Network.Serialization;
using System.Collections;
using GAF.Network.Threading;

namespace GAF.Network
{
	/// <summary>
	/// Evaluation client.
	/// </summary>
	public class EvaluationClient : IDisposable
	{
		//this represents a poolof connected sockets clients
		//private BlockingCollection<Socket> _socketPool;
		private SocketPool _socketPool;
		private FitnessAssembly _fitnessAssembly;
		private string _fitnessAssemblyName;
		private GAF.Network.Threading.ProducerConsumerQueue _pcQueue;

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

			_socketPool = new SocketPool (endPoints);
			_pcQueue = new GAF.Network.Threading.ProducerConsumerQueue (endPoints.Count);

			_fitnessAssemblyName = fitnessAssemblyName;
			_fitnessAssembly = new FitnessAssembly (fitnessAssemblyName);

			InitialiseServers (true);
		}


		/// <summary>
		/// Replaces existing endpoints with specified endpoints.
		/// </summary>
		/// <param name="endpoints">Endpoints.</param>
		public void UpdateEndpoints (List<IPEndPoint> endpoints)
		{
			//TODO: Implement me
			//bear in mind that sockets will be constantly changing
			//can this even work?
		}

		private void InitialiseServers (bool forceInitialisation)
		{
			//send a status packet to see if we have already initialised this connection
			//i.e. passed the fitness function accross
			try {

				var socketPoolItems = _socketPool.Sockets;
				foreach (var socketPoolItem in socketPoolItems) {

					//TODO: Check that item is connected.
					var serverStatus = GetServerStatus (socketPoolItem.Socket);

					//check if it is ok to init server with fitness etc
					if (!serverStatus.ServerDefinedFitness && (!serverStatus.Initialised || forceInitialisation)) {

						Log.Info (string.Format ("Sending the fitness function to server {0}.", socketPoolItem.EndPoint));

						var functionBytes = File.ReadAllBytes (_fitnessAssemblyName);

						//send to server
						var xmitPacket = new Packet (functionBytes, PacketId.Init, Guid.Empty);
						SocketClient.TransmitData (socketPoolItem.Socket, xmitPacket);
					}
				}

			} catch (Exception ex) {
				Log.Error (ex);
			}
		}

		/// <summary>
		/// Evaluate the specified solutionsToEvaluate.
		/// </summary>
		/// <param name="solutionsToEvaluate">Solutions to evaluate.</param>
		public async Task<int> Evaluate (List<Chromosome> solutionsToEvaluate)
		{
			//this method is called after each operator, the solutionsToEvaluate list
			//contains all the required for a complete generation.
			try {

				if (solutionsToEvaluate == null) {
					throw new ArgumentNullException (nameof (solutionsToEvaluate), "The parameter is null.");
				}

				var solutionCount = solutionsToEvaluate.Count;
				if (solutionCount == 0) {
					throw new ArgumentException ("The parameter is empty.", nameof (solutionsToEvaluate));
				}

				//create a concurrent queue with max concurrency equal 
				//to the endpoint count and add the delegates to the queue
				foreach (var solution in solutionsToEvaluate) {
					var chromosome = solution;
					var task = _pcQueue.Enqueue (() => Evaluate (chromosome));
					Log.Debug (string.Format ("Chromosome [{0}] evaluation queued as Task {1} queued.", chromosome.Id, task.Id));
				}

				var allTasks = Task.WhenAll (_pcQueue.AllTasks);
				await allTasks;

				// set the number of evaluations we have done (one per solution)
				return solutionCount;


			} catch (Exception) {

				throw;
			}

		}

		private ServerStatus GetServerStatus (Socket client)
		{
			var statusRequestPacket = new Packet (PacketId.Status);
			var statusPacket = SocketClient.TransmitData (client, statusRequestPacket);

			//check the status packet and decode with the ServerStatus class.
			if (statusPacket == null) {
				throw new GAF.Network.SocketException ("Status Packet was not received or was empty.");
			}

			return new ServerStatus (statusPacket);

		}

		private void Evaluate (Chromosome chromosome)
		{
			SocketPoolItem socketPoolItem = null;

			try {
				//Get a connected socket from the socket collection. As this is a blocking collection
				//wrapping a concurrent queue, then '_sockets.Take()' will remove the next item (Socket) 
				//from the queue. If nothing is on the queue the task will wait (block). This is by 
				//design as the task(s) that call this method (one worker per endpoint) are defined
				//as long running tasks.

				//use the connected socket and then pop it back on the queue.
				//this ensures that only unused sockets are selected.

				socketPoolItem = _socketPool.Dequeue ();

				Log.Debug (string.Format ("SocketPoolItem: {0} retrieved from the pool.", socketPoolItem.EndPoint));

				//if not connected, attempt to reconnect
				if (!socketPoolItem.Socket.Connected) {

					Log.Debug (string.Format ("SocketPoolItem: {0} not connected, attempting to re-connect.", socketPoolItem.EndPoint));

					socketPoolItem = new SocketPoolItem (
						SocketClient.Connect (socketPoolItem.EndPoint), socketPoolItem.EndPoint);

				} else {

					Log.Debug (string.Format ("SocketPoolItem: {0} connected.", socketPoolItem.EndPoint));

				}

				//pass the socket to the fitness function using the tag property
				chromosome.Tag = socketPoolItem;
				chromosome.Evaluate (RemoteFitnessDelegateFunction);

			} catch (Exception ex) {

				if (socketPoolItem != null && socketPoolItem.Socket != null && socketPoolItem.EndPoint != null) {
					Log.Error (string.Format ("{0} [Socket:{1}]", ex.Message, socketPoolItem.EndPoint));
				} else {
					Log.Error (ex);
				}

				var task = _pcQueue.Enqueue (() => Evaluate (chromosome));
				Log.Debug (string.Format ("Chromosome [{0}] evaluation re-queued as Task {1} queued.", chromosome.Id, task.Id));

				throw;

			} finally {
				//finished with the endpoint so pop it back in the list to be used by this or another worker
				_socketPool.Enqueue (socketPoolItem);
			}
		}

		private double RemoteFitnessDelegateFunction (Chromosome chromosome)
		{
			double fitness = 0.0;

			//retrieve the client to be used;
			var socketPoolItem = (SocketPoolItem)chromosome.Tag;

			// Convert the passed chromosome to a byte array
			//actually just the genes are serialised as we dont need to sent the rest
			//var byteData = Serializer.Serialize<Chromosome> (chromosome, _fitnessAssembly.KnownTypes);
			var byteData = Binary.Serialize<List<Gene>> (chromosome.Genes, _fitnessAssembly.KnownTypes);

			var xmitPacket = new Packet (byteData, PacketId.Data, chromosome.Id);

			var recPacket = SocketClient.TransmitData (socketPoolItem.Socket, xmitPacket);
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

		public void Dispose ()
		{
			_pcQueue.Dispose ();

			//close all available sockets
			_socketPool.Dispose ();

		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the evaluations undertaken since the class was instantiated.
		/// </summary>
		/// <value>The evaluations.</value>
		public int Evaluations { get; private set; }

		#endregion

	}
}
