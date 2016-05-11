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
using System.Collections.Generic;
using System.Collections;

#if !PCL

using System;

namespace GAF.Net
{
	/// <summary>
	/// Packet manager class to manage a cyclic byte buffer.
	/// </summary>
	public class Buffer
	{
		private byte[] _byteQueue;
		private int _inPtr = 0;
		private int _outPtr = 0;
		private int _bytesUsed = 0;
		private object syncLock = new object ();
		private Queue<Packet> _packetQueue = new Queue<Packet> ();

		//TODO: Raise event/exception when data is overwritten
		//TODO: Raise event when a packet is ready.

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Net.PacketManager"/> class
		/// with a default buffer size of 1Mb.
		/// </summary>
		public Buffer () : this (1048576)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Net.PacketManager"/> class.
		/// </summary>
		/// <param name="maxQueueSize">Max queue size.</param>
		public Buffer (int maxQueueSize)
		{
			// initialise
			MaxQueueSize = maxQueueSize;

			_byteQueue = new byte[maxQueueSize];
		}

		/// <summary>
		/// Adds data to the internal buffer.
		/// </summary>
		/// <param name="data">Data.</param>
		public void Add (byte[] data)
		{
			var dataLength = data.Length;

			if (dataLength > MaxQueueSize - _bytesUsed) {
				throw new ArgumentOutOfRangeException ("data", "Buffer overun.");
			}

			if (dataLength > 0) {


				var bytesBeforeWrapping = MaxQueueSize - _inPtr;

				//if enough room without wrapping simply copy the array into
				//the buffer and update the inPtr.
				if (bytesBeforeWrapping > dataLength) {

					Array.Copy (data, 0, _byteQueue, _inPtr, dataLength);
					IncrementPointer (ref _inPtr, dataLength);


				} else {

					//if wrapping is required copy in two steps
					//
					//

					IncrementPointer (ref _inPtr, dataLength);

				}

				_bytesUsed += dataLength;
			}

		}

		/// <summary>
		/// Adds data to the internal buffer.
		/// </summary>
		/// <param name="data">Data.</param>
		public void Add (byte data)
		{ 
			var dataArray = new byte[1] { data };
			Add (dataArray);
		}

		/// <summary>
		/// Gets the maximum size of the queue.
		/// </summary>
		/// <value>The size of the queue.</value>
		public int MaxQueueSize { private set; get; }

		/// <summary>
		/// Gets the size of the queue.
		/// </summary>
		/// <value>The size of the queue.</value>
		public int QueueSize { 

			get {
				return MaxQueueSize - _bytesUsed;
			} 
		}

		#region private methods

		private bool IncrementPointer (ref int pointer, int count)
		{
			bool wrapped = false;

			pointer += count;
			var overflow = pointer % MaxQueueSize;

			if (overflow >= 0) {
				pointer = overflow;
				wrapped = true;

			}

			return wrapped;
		}



		#endregion
	}
}

#endif

