using System;

namespace GAF.Api
{
	public interface IGafLabConsumerFunctions : IFitness
	{
		/// <summary>
		/// This method should implement the GA's terminate function.
		/// </summary>/// <returns><c>true</c>, if algorithm was terminated, <c>false</c> otherwise.</returns>
		/// <param name="population">Population.</param>
		/// <param name="currentGeneration">Current generation.</param>
		/// <param name="currentEvaluation">Current evaluation.</param>
		bool TerminateAlgorithm (GAF.Population population, int currentGeneration, long currentEvaluation);

		/// <summary>
		/// This interface is provided to support the GAF.Lab GUI application. Please see the product information
		/// for GAF.Lab for further details.
		/// </summary>
		/// <returns>The population.</returns>
		GAF.Population CreatePopulation();

		/// <summary>
		/// Creates the generation complete message.
		/// </summary>
		/// <returns>The generation complete message.</returns>
		/// <param name="currentPopulation">Current population.</param>
		/// <param name="currentGeneration">Current generation.</param>
		/// <param name="evaluations">Evaluations.</param>
		string CreateGenerationCompleteMessage(GAF.Population currentPopulation, int currentGeneration, long evaluations);

		/// <summary>
		/// Creates the run complete message.
		/// </summary>
		/// <returns>The run complete message.</returns>
		/// <param name="currentPopulation">Current population.</param>
		/// <param name="currentGeneration">Current generation.</param>
		/// <param name="evaluations">Evaluations.</param>
		string CreateRunCompleteMessage(GAF.Population currentPopulation, int currentGeneration, long evaluations);
	}
}

