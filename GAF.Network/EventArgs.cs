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
		public PacketEventArgs (long packetsReceived, Packet packet)
		{
			_packetsReceived = packetsReceived;
			_packet = packet;

		}

		/// <summary>
		/// Returns the number of complete packets received so far.
		/// </summary>
		public long PacketsReceived {
			get { return _packetsReceived; }
		}

		/// <summary>
		/// Returns the Packet associated with this event.
		/// </summary>
		/// <value>The header.</value>
		public Packet Packet {
			get { return _packet; }
		}

		/// <summary>
		/// Used to return a result from the event code.
		/// </summary>
		/// <value>The fitness.</value>
		public double Result{ set; get; }
	}
		
	/// <summary>
	/// Packet Maneger exception event arguments.
	/// </summary>
	public class ProcessingExceptionEventArgs : EventArgs
	{
		private readonly string _message;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Net.ProcessingExceptionEventArgs"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public ProcessingExceptionEventArgs (string message)
		{
			_message = message;
		}

		/// <summary>
		/// Returns the list of Exception messages.
		/// </summary>
		public string Message {
			get { return _message; }
		}
	}

	/// <summary>
	/// Remote evaluation event arguments.
	/// </summary>
	public class RemoteEvaluationEventArgs : EventArgs
	{
		private readonly Chromosome _solution;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Net.ProcessingExceptionEventArgs"/> class.
		/// </summary>
		/// <param name="solution">Message.</param>
		public RemoteEvaluationEventArgs (Chromosome solution)
		{
			_solution = solution;
		}

		/// <summary>
		/// Returns the current solution.
		/// </summary>
		public Chromosome Solution {
			get { return _solution; }
		}
	}
	/// <summary>
	/// Event arguments used within the logging events.
	/// </summary>
	public class LoggingEventArgs : EventArgs
	{
		private readonly string _message;
		private readonly LoggingType _loggingType;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.LoggingEventArgs"/> class.
		/// </summary>
		/// <param name="format">Format.</param>
		/// <param name="args">Arguments.</param>
		public LoggingEventArgs (string format, params object[] args) : this (LoggingType.Info, format, args)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.LoggingEventArgs"/> class.
		/// </summary>
		/// <param name = "loggingType"></param>
		/// <param name="format">Format.</param>
		/// <param name="args">Arguments.</param>
		public LoggingEventArgs (LoggingType loggingType, string format, params object[] args)
		{
			var msg = string.Format (format, args);
			_message = msg;
			_loggingType = loggingType;
		}

		/// <summary>
		/// Returns the list of Exception messages.
		/// </summary>
		public string Message {
			get { return _message; }
		}
		public LoggingType LoggingType {
			get { return _loggingType; }
		}

	}

	//public enum LoggingLevel
	//{
	//	Warning,
	//	Info,
	//	Debug
	//}

	public enum LoggingType
	{
		Error,
		Warning,
		Info//,
		//Debug
	}
}
