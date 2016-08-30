using System;

namespace GAF.LabGtk
{

	public class BinderException : Exception
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public BinderException()
			: base()
		{
		}

		/// <summary>
		/// Constructor accepting a message.
		/// </summary>
		/// <param name="message"></param>
		public BinderException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor accepting a formatted message.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public BinderException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}

		/// <summary>
		/// Constructor accepting a message and inner exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public BinderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor accepting a formatted message and inner exception.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="innerException"></param>
		/// <param name="args"></param>
		public BinderException(string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException)
		{
		}

	}

}

