using System;
using System.Collections.Generic;

namespace GAF.Api
{
	public class PopulationHistory
	{
		private const int _defaultHistorySize = 100;
		private int _populationHistorySize;

		public PopulationHistory () : this(_defaultHistorySize)
		{
		}

		public PopulationHistory (int populationHistorySize)
		{
			_populationHistorySize = populationHistorySize;
			PopulationHistoryItems = new List<PopulationHistoryItem> ();
		}

		/// <summary>
		/// Adds a population to the history.
		/// </summary>
		/// <returns>The population.</returns>
		/// <param name="population">Population.</param>
		public void AddPopulation (GAF.Population population, int sourceGeneration)
		{
			if (population == null)
				throw new ArgumentNullException ("population");
			if (population.Solutions == null)
				throw new NullReferenceException ("The population.Solutions property is null.");


			var populationHistoryItem = new PopulationHistoryItem (population, sourceGeneration);
			PopulationHistoryItems.Add (populationHistoryItem);

			if (PopulationHistoryItems.Count > _populationHistorySize)
				PopulationHistoryItems.RemoveAt(0);
		}

		/// <summary>
		/// Clears the history.
		/// </summary>
		/// <returns>The history.</returns>
		public void ClearHistory ()
		{
			PopulationHistoryItems = new List<PopulationHistoryItem> ();
		}

		/// <summary>
		/// Gets the population history items.
		/// </summary>
		/// <value>The population history items.</value>
		public List<PopulationHistoryItem> PopulationHistoryItems { get; private set; }
	}
}

