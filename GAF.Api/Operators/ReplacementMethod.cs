using System;

namespace GAF.Api.Operators
{
	/// <summary>
	/// Enum that defines the Replacement Method used within the Genetic Algorithm.
	/// </summary>
	public enum ReplacementMethod
	{
		/// <summary>
		/// Generational Replacement.
		/// </summary>
		GenerationalReplacement = 1,
		/// <summary>
		/// Deletes the weakest solutions.
		/// </summary>
		DeleteLast = 2
	}
}

