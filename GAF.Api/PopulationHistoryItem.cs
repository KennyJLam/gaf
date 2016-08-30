using System;
using System.Collections.Generic;

namespace GAF.Api
{
	public class PopulationHistoryItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:GAF.Api.PopulationHistoryItem"/> class.
		/// </summary>
		/// <param name="population">Population.</param>
		public PopulationHistoryItem (GAF.Population population, int sourceGeneration)
		{
			if (population == null)
				throw new ArgumentNullException ("population");
			if (population.Solutions == null)
				throw new NullReferenceException ("The population.Solutions property is null.");

			SolutionData = new List<Chromosome> ();

			foreach (var solution in population.Solutions) {
				SolutionData.Add (new Chromosome(solution.ToString (), solution.Fitness));
			}
			SourceGeneration = sourceGeneration;
		}

		/// <summary>
		/// Gets or sets the solution data.
		/// </summary>
		/// <value>The solution data.</value>
		public List<Chromosome> SolutionData { get; private set; }

		/// <summary>
		/// Gets the source generation.
		/// </summary>
		/// <value>The source generation.</value>
		public int SourceGeneration { get; private set; }

		/// <summary>
		/// Gets the fitness.
		/// </summary>
		/// <value>The fitness.</value>
		public double Fitness { get; private set; }
	}

	public class Chromosome
	{
		public Chromosome (string data, double fitness)
		{
			Data = data;
			Fitness = fitness;
		}
		public string Data { get; private set; }
		public double Fitness { get; private set; }
	}
}

