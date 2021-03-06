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
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using GAF.Network.Serialization;

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
		public EvaluationServer ()
		{
			_serverDefinedFitness = false;
			FitnessAssemblyName = _fitnessAssemblyName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Network.EvaluationServer"/> class.
		/// </summary>
		/// <param name="fitnessAssemblyName">Fitness assembly name.</param>
		public EvaluationServer (string fitnessAssemblyName)
		{
			//if the parameter is null or empty use the dynamic name
			if (string.IsNullOrWhiteSpace (fitnessAssemblyName)) {
				_serverDefinedFitness = false;
				FitnessAssemblyName = _fitnessAssemblyName;
			} else {
				_serverDefinedFitness = true;
				FitnessAssemblyName = fitnessAssemblyName;
			}

		}

		/// <summary>
		/// Gets the name of the fitness assembly being used.
		/// </summary>
		/// <value>The name of the fitness assembly.</value>
		public string FitnessAssemblyName { private set; get; }

		/// <summary>
		/// Start the server listening on the specified endpoint.
		/// </summary>
		/// <param name="endPoint">End point.</param>
		public void Start (IPEndPoint endPoint)
		{
			Log.Info (string.Format ("Attempting to load the assemby {0}.", FitnessAssemblyName));

			if (File.Exists (FitnessAssemblyName)) {
				_fitnessAssembly = new FitnessAssembly (FitnessAssemblyName);
				if (FitnessAssemblyName == null) {
					Log.Error (string.Format ("The Assembly {0} exists, but could not be loaded.", FitnessAssemblyName));
				} else {
					Log.Info (string.Format ("Assembly {0} has been loaded.", FitnessAssemblyName));
				}
			} else {
				Log.Warning (string.Format ("Assembly {0} not loaded. File does not exist. Server may need initialising", FitnessAssemblyName));
			}

			Log.Info (string.Format ("GAF Evaluation Server Listening on {0}:{1}.",
									endPoint.Address, endPoint.Port));

			SocketListener.OnPacketReceived += listener_OnPacketReceived;
			SocketListener.StartListening (endPoint);
		}

		private void listener_OnPacketReceived (object sender, PacketEventArgs e)
		{
			try {

				Log.Debug (string.Format ("Packet Received, PacketId:{0} ObjectId:{1} Data Bytes:{2}", 
				                          e.Packet.Header.PacketId, 
				                          e.Packet.Header.ObjectId, 
				                          e.Packet.Data.Length));

				switch ((PacketId)e.Packet.Header.PacketId) {

				case PacketId.Data: {

						if (e.Packet.Header.DataLength > 0) {

							//deserialise to get the genes, create a new chromosome from these
							//this saves having to send the whole chromosome
							var genes = Binary.DeSerialize<List<Gene>> (e.Packet.Data, _fitnessAssembly.KnownTypes);
							var chromosome = new Chromosome (genes);

							e.Result = chromosome.Evaluate (_fitnessAssembly.FitnessFunction);

							if (OnEvaluationComplete != null) {

								var eventArgs = new RemoteEvaluationEventArgs (chromosome);
								this.OnEvaluationComplete (this, eventArgs);
							}
						}

						break;
					}

				case PacketId.Init: {

						Log.Info ("Initialisation initiated.");

						if (e.Packet.Header.DataLength > 0) {

							Log.Info ("Writing Fitness Assembly to filesystem.");

							File.WriteAllBytes (_fitnessAssemblyName, e.Packet.Data);

							//check for fitness file
							if (File.Exists (_fitnessAssemblyName)) {

								Log.Info ("Loading the Fitness Assembly.");
								_fitnessAssembly = new FitnessAssembly (_fitnessAssemblyName);

							} else {
								Log.Error ("Fitness Assembly not accesible or missing.");
							}
						}

						break;
					}

				case PacketId.Status: {

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
			} catch (Exception ex) {

				while (ex.InnerException != null) {
					ex = ex.InnerException;
				}
				Log.Error (ex);
			}
		}

	}
}
