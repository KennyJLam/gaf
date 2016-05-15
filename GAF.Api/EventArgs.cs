using System;

namespace GAF.Lab.Shared
{
	/// <summary>
	/// Event arguments used within the main GA exeption events.
	/// </summary>
	public class ExceptionEventArgs : EventArgs
	{
		private readonly string _message;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public ExceptionEventArgs(string message)
		{
			_message = message;
		}

		/// <summary>
		/// Returns the list of Exception messages.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
	}
		
}

