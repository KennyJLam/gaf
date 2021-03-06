﻿/*
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
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace GAF.Network
{

	/// <summary>
	/// Asynchronous socket listener.
	/// </summary>
	public static class SocketListener
	{
		/// <summary>
		/// Delegate definition for the GenerationComplete event handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void PacketReceivedHandler (object sender, PacketEventArgs e);

		/// <summary>
		/// Event definition for the GenerationComplete event handler.
		/// </summary>
		public static event PacketReceivedHandler OnPacketReceived;

		/// <summary>
		/// Thread Signal
		/// </summary>
		private static ManualResetEvent _allDone = new ManualResetEvent (false);

		private static int _packetCount = 0;
		private const int pidEtx = 15;

		/// <summary>
		/// Start listening.
		/// </summary>
		/// <param name="endPoint">End point.</param>
		public static void StartListening (IPEndPoint endPoint)
		{

			// Create a TCP/IP socket.
			Socket listener = new Socket (AddressFamily.InterNetwork,
								  SocketType.Stream, ProtocolType.Tcp);


			// Bind the socket to the local endpoint and listen for incoming connections.

			listener.Bind (endPoint);
			listener.Listen (100);

			while (true) {
				// Set the event to nonsignaled state.
				_allDone.Reset ();

				// Start an asynchronous socket to listen for connections.
				listener.BeginAccept (
					new AsyncCallback (AcceptCallback),
					listener);

				// Wait until a connection is made before continuing.
				_allDone.WaitOne ();

			}
		}

		private static void AcceptCallback (IAsyncResult ar)
		{
			// Signal the main thread to continue.
			_allDone.Set ();

			// Get the socket that handles the client request.
			Socket listener = (Socket)ar.AsyncState;
			Socket handler = listener.EndAccept (ar);

			if (handler.RemoteEndPoint != null) {
				Log.Info (string.Format ("Connected to {0}.", handler.RemoteEndPoint.ToString ()));
			}

			// Create the state object.
			StateObject state = new StateObject ();
			state.WorkSocket = handler;

			handler.BeginReceive (state.Buffer, 0, StateObject.BufferSize, 0,
				new AsyncCallback (ReadCallback), state);

		}

		private static void ReadCallback (IAsyncResult ar)
		{
			Log.Debug ("Receiving...");

			// Retrieve the state object and the handler socket
			// from the asynchronous state object.
			StateObject state = (StateObject)ar.AsyncState;
			Socket handler = state.WorkSocket;
			bool etxReceived = false;

			// Read data from the client socket. 
			int bytesRead = handler.EndReceive (ar);

			if (bytesRead > 0) {

				Log.Debug (string.Format ("Bytes read from socket:0x{0} ({1})", bytesRead.ToString ("X6"), bytesRead));
				//Log.Debug ("Buffer:");
				//Log.Debug (state.Buffer);

				// At this point all of the read bytes are in the state.Buffer. The
				// state.Buffer is fixed in size, so bytes will need to be transferred to 
				// the PacketManager in chunks (or part chunk if its the last chunk).
				byte[] dataBytes = new byte[bytesRead];
				Array.Copy (state.Buffer, dataBytes, bytesRead);

				Log.Debug (string.Format("Adding {0} bytes to the Packet Manager queue.", bytesRead));

				state.PacMan.Add (dataBytes);

				//see if we have any packets
				var packet = state.PacMan.GetPacket ();

				if (packet != null) {

					Log.Debug ("Packet received.");

					double result = 0.0;

					_packetCount++;

					if (OnPacketReceived != null) {

						var args = new PacketEventArgs (_packetCount, packet);
						OnPacketReceived (null, args);
						result = args.Result;

					}

					//have we received all for this transmission?
					etxReceived = packet.Header.PacketId == PacketId.Etx;

					//Send the result/respose packet
					Log.Debug ("Sending response packet;");
					Send (ar, new Packet (BitConverter.GetBytes (result), PacketId.Result, packet.Header.ObjectId));
				}

				//if not end of transmission get more data
				if (!etxReceived) {
					state.WorkSocket.BeginReceive (state.Buffer, 0, StateObject.BufferSize, 0,
						new AsyncCallback (ReadCallback), state);
				} else {
					Log.Debug ("ETX received.");
				}


			}
		}

		private static void Send (IAsyncResult ar, Packet data)
		{
			StateObject state = (StateObject)ar.AsyncState;
			Socket handler = state.WorkSocket;

			// Begin sending the data to the remote device.
			handler.BeginSend (data.ToByteArray (), 0, data.Header.DataLength + PacketHeader.HeaderLength, 0,
				new AsyncCallback (SendCallback), state);

		}

		private static void SendCallback (IAsyncResult ar)
		{

			StateObject state = (StateObject)ar.AsyncState;
			Socket handler = state.WorkSocket;

			try {

				// Complete sending the data to the remote device.
				handler.EndSend (ar);

				if (state.PacMan.LastPidReceived == PacketId.Etx) {
					handler.LingerState = new LingerOption (true, 1);
					handler.Close ();
				}

			} catch (Exception ex) {
				if (state.PacMan.LastPidReceived != PacketId.Etx) {
					throw ex;
				}
			}
		}

	}
}
