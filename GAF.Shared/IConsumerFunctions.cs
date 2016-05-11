using System;

namespace GAF
{
	/// <summary>
	/// This interface is provided to support external programs such as the GAF.Lab GUI application.
	/// </summary>
	public interface IConsumerFunctions
	{

		/// <summary>
		/// This interface is provided to support external programs such as the GAF.Lab GUI application.
		/// </summary>
		/// <returns>The fitness.</returns>
		/// <param name="chromosome">Chromosome.</param>
		double EvaluateFitness(Chromosome chromosome);

		/// <summary>
		/// This interface is provided to support external programs such as the GAF.Lab GUI application.
		/// </summary>/// <returns><c>true</c>, if algorithm was terminated, <c>false</c> otherwise.</returns>
		/// <param name="population">Population.</param>
		/// <param name="currentGeneration">Current generation.</param>
		/// <param name="currentEvaluation">Current evaluation.</param>
		bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation);

	}
}

