using System;

namespace GAF
{
	/// <summary>
	/// Gene clone exception.
	/// </summary>
	public class GeneCloneException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:GeneCloneException"/> class
		/// </summary>
		GeneCloneException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GeneCloneException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> thaInconsistentCloneException the exception. </param>
		public GeneCloneException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:GeneCloneException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">TInconsistentCloneExceptionn that is the cause of the current exception. </param>
		public GeneCloneException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

