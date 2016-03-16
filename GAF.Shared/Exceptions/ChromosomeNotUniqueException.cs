using System;

namespace GAF
{
	/// <summary>
	/// Chromosome not unique exception.
	/// </summary>
	public class ChromosomeNotUniqueException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeNotUniqueException"/> class
		/// </summary>
		public ChromosomeNotUniqueException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeNotUniqueException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the ChromosomeException </param>
		public ChromosomeNotUniqueException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeNotUniqueException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of thChromosomeExceptionxception. </param>
		public ChromosomeNotUniqueException (string message, Exception inner) : base (message, inner)
		{
		}
			
	}
	
}


