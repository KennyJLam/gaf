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

