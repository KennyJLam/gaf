using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GAF.Network.Treading
{
	public class SocketPoolItem
	{
		public SocketPoolItem (Socket socket, IPEndPoint endPoint)
		{
			Socket = socket;
			EndPoint = endPoint;
		}

		public Socket Socket { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
	}

	public class SocketPool : IDisposable
	{
		//private BlockingCollection<Socket> _socketPool;
		private BlockingCollection<SocketPoolItem> _socketPool;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.Treading.SocketPool"/> class.
		/// </summary>
		/// <param name="endPoints">End points.</param>
		public SocketPool (List<IPEndPoint> endPoints)
		{
			//create a blocking queue for the connected sockets;
			_socketPool = new BlockingCollection<SocketPoolItem> ();

			//add a connected socket along with its remote endpoint to the queue
			foreach (var remoteEndpoint in endPoints) {
				_socketPool.Add (
					new SocketPoolItem (
						SocketClient.Connect (remoteEndpoint), remoteEndpoint));
			}
		}

		/// <summary>
		/// Gets the sockets.
		/// </summary>
		/// <value>The sockets.</value>
		public IEnumerable<SocketPoolItem> Sockets {
			get {
				return _socketPool.AsEnumerable ();
			}
		}

		/// <summary>
		/// Dequeue a Socketb from the Queue.
		/// </summary>
		public SocketPoolItem Dequeue ()
		{
			return _socketPool.Take ();
		}

		/// <summary>
		/// Enqueue the specified socket.
		/// </summary>
		/// <param name="socket">Socket.</param>
		public void Enqueue (SocketPoolItem socket)
		{
			_socketPool.Add (socket);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:GAF.Network.Treading.SocketPool"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:GAF.Network.Treading.SocketPool"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="T:GAF.Network.Treading.SocketPool"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:GAF.Network.Treading.SocketPool"/> so the garbage collector can reclaim the memory that the
		/// <see cref="T:GAF.Network.Treading.SocketPool"/> was occupying.</remarks>
		public void Dispose ()
		{
			//close all pool sockets
		}
	}
}

