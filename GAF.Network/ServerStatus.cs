using System;
namespace GAF.Network
{
	public class ServerStatus
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.ServerStatus"/> class.
		/// </summary>
		/// <param name="statusPacket">Status packet.</param>
		public ServerStatus (Packet statusPacket)
		{
			if (statusPacket == null && statusPacket.Header.PacketId != PacketId.Status) {
				throw new ArgumentException ("The specified status packet is invalid.");
			}

			//check the status value by converting from the (double) result to an integer and 
			// ANDing this with ServerStatus.Initialised.


				var result = (int)BitConverter.ToDouble (statusPacket.Data, 0);
				Initialised = (result & (int)ServerStatusFlags.Initialised) == (int)ServerStatusFlags.Initialised;
				ServerDefinedFitness = (result & (int)ServerStatusFlags.ServerDefinedFitness) == (int)ServerStatusFlags.ServerDefinedFitness;

		}

		/// <summary>
		/// Gets a value indicating whether the server is initialised.
		/// </summary>
		/// <value><c>true</c> if initialised; otherwise, <c>false</c>.</value>
		public bool Initialised { private set; get; }

		/// <summary>
		/// Gets a value indicating whether the fitness being used has been defined at the server.
		/// </summary>
		/// <value><c>true</c> if server defined fitness; otherwise, <c>false</c>.</value>
		public bool ServerDefinedFitness { private set; get; }

	}
}

