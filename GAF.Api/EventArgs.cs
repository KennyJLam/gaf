using System;

namespace GAF.Api
{

	/// <summary>
	/// Event arguments used within the main GA exeption events.
	/// </summary>
	public class ExceptionEventArgs : EventArgs
	{
		private readonly string _method;
		private readonly string _message;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name = "method"></param>
		/// <param name="message"></param>
		public ExceptionEventArgs (string method, string message)
		{
			_method = method;
			_message = message;
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message {
			get { return _message; }
		}

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		public string Method {
			get { return _method; }
		}
	}

	/// <summary>
	/// Event arguments used within the logging events.
	/// </summary>
	public class LoggingEventArgs : EventArgs
	{
		private readonly string _message;
		private readonly bool _isWarning;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Api.LoggingEventArgs"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name = "isWarning"></param>
		public LoggingEventArgs (string message, bool isWarning)
		{
			_message = message;
			_isWarning = isWarning;
		}

		/// <summary>
		/// Returns the list of Exception messages.
		/// </summary>
		public string Message {
			get { return _message; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is warning.
		/// </summary>
		/// <value><c>true</c> if this instance is warning; otherwise, <c>false</c>.</value>
		public bool IsWarning {
			get { return _isWarning; }
		}
	}
}

