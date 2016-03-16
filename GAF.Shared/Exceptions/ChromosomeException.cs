using System;

namespace GAF
{
	/// <summary>
	/// Chromosome exception.
	/// </summary>
	public class ChromosomeException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeException"/> class
		/// </summary>
		public ChromosomeException ()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.ChromosomeException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public ChromosomeException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of thChromosomeExceptionxception. </param>
		public ChromosomeException (string message, Exception inner) : base (message, inner)
		{
		}
			
	}
	
}


