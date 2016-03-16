using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GAF
{	
	/// <summary>
	/// Population selection exception.
	/// </summary>
	public class PopulationSelectionException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationSelectionException"/> class
		/// </summary>
		public PopulationSelectionException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationSelectionException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exceptChromosomeException </param>
		public PopulationSelectionException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationSelectionException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of thChromosomeExceptionxception. </param>
		public PopulationSelectionException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}
