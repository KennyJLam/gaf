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
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;

namespace GAF.Network
{
	/// <summary>
	/// Evaluation server.
	/// </summary>
	public class EvaluationServer
	{
		private const string _fitnessAssemblyName = "f60011de-c680-4ccc-b69e-8c958b91ff4d.dll";
		private FitnessAssembly _fitnessAssembly;
		private bool _serverDefinedFitness;

		/// <summary>
		/// Delegate definition for the InitialEvaluationComplete event handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void EvaluationCompleteHandler (object sender, RemoteEvaluationEventArgs e);

		/// <summary>
		/// Event definition for the InitialEvaluationComplete event handler.
		/// </summary>
		public event EvaluationCompleteHandler OnEvaluationComplete;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.EvaluationServer"/> class.
		/// </summary>
		/// <param name="fitnessAssemblyName">Fitness assembly name.</param>
		//public EvaluationServer (string fitnessAssemblyName)
		//{
		//	if (string.IsNullOrWhiteSpace (fitnessAssemblyName)) {
		//		throw new ArgumentException ("The specified fitness assembly name is null or empty.", nameof (fitnessAssemblyName));
		//	}
		//	_fitnessAssembly = new FitnessAssembly (fitnessAssemblyName);

		//}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.EvaluationServer"/> class.
		/// </summary>
		public EvaluationServer ()
		{
			_serverDefinedFitness = false;
			if (File.Exists (_fitnessAssemblyName)) {
				_fitnessAssembly = new FitnessAssembly (_fitnessAssemblyName);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.EvaluationServer"/> class.
		/// </summary>
		/// <param name="fitnessAssemblyName">Fitness assembly name.</param>
		public EvaluationServer (string fitnessAssemblyName)
		{
			//if the parameter ios null or empty use the dynamic name
			if (string.IsNullOrWhiteSpace (fitnessAssemblyName)) {
				_serverDefinedFitness = false;
				fitnessAssemblyName = _fitnessAssemblyName;
			} else {
				_serverDefinedFitness = true;
			}

			if (File.Exists (fitnessAssemblyName)) {
				_fitnessAssembly = new FitnessAssembly (fitnessAssemblyName);
			}
		}

		/// <summary>
		/// Start the server listening on the specified ipAddress and port.
		/// </summary>
		/// <param name="endPoint">End point.</param>
		public void Start (IPEndPoint endPoint)
		{
			SocketListener.OnPacketReceived += listener_OnPacketReceived;
			SocketListener.StartListening (endPoint);
		}

		private void listener_OnPacketReceived (object sender, PacketEventArgs e)
		{
			//Console.WriteLine ("Packet Received: {0}", e.Packet.Header.PacketId);

			switch ((PacketId)e.Packet.Header.PacketId) {

			case PacketId.Chromosome: {
					if (e.Packet.Header.DataLength > 0) {

						//if (_fitnessAssembly == null) {
						//	_fitnessAssembly = new FitnessAssembly (_fitnessAssemblyName);
						//}

						var chromosome = Serializer.DeSerialize<Chromosome> (e.Packet.Data, _fitnessAssembly.KnownTypes);
						e.Result = chromosome.Evaluate (_fitnessAssembly.FitnessFunction);

						if (OnEvaluationComplete != null) {

							var eventArgs = new RemoteEvaluationEventArgs (chromosome);
							this.OnEvaluationComplete (this, eventArgs);
						}
					}

					break;
				}

			case PacketId.Init: {

					//if (_fitnessAssembly == null) {
					//	_fitnessAssembly = new FitnessAssembly (_fitnessAssemblyName);
					//}

					if (e.Packet.Header.DataLength > 0) {
						File.WriteAllBytes (_fitnessAssemblyName, e.Packet.Data);
						_fitnessAssembly = new FitnessAssembly (_fitnessAssemblyName);
					}

					break;
				}

			case PacketId.Status: {

					Console.WriteLine ("Status Packet Received: {0}", e.Packet.Header.PacketId);

					var result = 0x0;

					//check for fitness file
					if (File.Exists (_fitnessAssemblyName)) {
						result = result | (int)ServerStatusFlags.Initialised;
					}

					if (_serverDefinedFitness) {
						result = result | (int)ServerStatusFlags.ServerDefinedFitness;
					}

					e.Result = (double)result;

					break;
				}
			}
		}
	}
}
