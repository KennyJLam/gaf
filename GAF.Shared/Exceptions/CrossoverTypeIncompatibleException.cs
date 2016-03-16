using System;

namespace GAF
{
	/// <summary>
	/// Crossover type incompatible exception.
	/// </summary>
	public class CrossoverTypeIncompatibleException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrossoverTypeIncompatibleException"/> classIncompatibleCrossoverTypeExceptiony>
		/// </summary>
		public CrossoverTypeIncompatibleException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.CrossoverTypeIncompatibleException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public CrossoverTypeIncompatibleException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrossoverTypeIncompatibleException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exceptiIncompatibleCrossoverTypeException </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public CrossoverTypeIncompatibleException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

