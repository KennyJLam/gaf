using System;

namespace GAF
{
	/// <summary>
	/// Gene type exception.
	/// </summary>
	public class GeneTypeException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:GeneTypeException"/> class
		/// </summary>
		GeneTypeException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GeneTypeException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> thaInconsistentCloneException the exception. </param>
		public GeneTypeException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GeneTypeException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">TInconsistentCloneExceptionn that is the cause of the current exception. </param>
		public GeneTypeException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

