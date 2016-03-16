using System;

namespace GAF
{
	/// <summary>
	/// Chromosome corrupt exception.
	/// </summary>
	public class ChromosomeCorruptException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeCorruptException"/> class
		/// </summary>
		public ChromosomeCorruptException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeCorruptException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the ChromosomeException </param>
		public ChromosomeCorruptException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeCorruptException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of thChromosomeExceptionxception. </param>
		public ChromosomeCorruptException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

