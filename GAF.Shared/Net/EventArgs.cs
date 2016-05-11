using System;

namespace GAF.Net
{



	/// <summary>
	/// Event arguments used within the GAF Socket Listener.
	/// </summary>
	public class PacketEventArgs : EventArgs
	{
		private readonly long _packetsReceived;
		private readonly Packet _packet;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name = "packetsReceived"></param>
		/// <param name = "packet"></param>
		public PacketEventArgs(long packetsReceived, Packet packet)
		{
			_packetsReceived = packetsReceived;
			_packet = packet;

		}

		/// <summary>
		/// Returns the number of complete packets received so far.
		/// </summary>
		public long PacketsReceived
		{
			get { return _packetsReceived; }
		}
		/// <summary>
		/// Returns the Packet associated with this event.
		/// </summary>
		/// <value>The header.</value>
		public Packet Packet
		{
			get { return _packet; }
		}
			
		/// <summary>
		/// Used to return a result from the event code.
		/// </summary>
		/// <value>The fitness.</value>
		public double Result{ set; get;}
	}
}

