using System;

namespace GAF.ConsumerFunctions
{
	public class TravellingSalesman : IConsumerFunctions
	{
		#region IConsumerFunctions implementation

		public double EvaluateFitness (Chromosome chromosome)
		{
			var distanceToTravel = CalculateDistance(chromosome);

			//experience suggests that 1500 is just less than the shortest possible distance
			var fitness = 1500/distanceToTravel; 
					
			return fitness > 1.0 ? 1.0 : fitness;

		}

		public bool TerminateAlgorithm (Population population, int currentGeneration, long currentEvaluation)
		{
			return currentGeneration >= 350;
		}
		#endregion

		#region Helper Methods

		public static double CalculateDistance(Chromosome chromosome)
		{
			var distanceToTravel = 0.0;
			City previousCity = null;

			//run through each city in the order specified in the chromosome
			foreach (var gene in chromosome.Genes)
			{
				var currentCity = (City)gene.ObjectValue;

				if (previousCity != null)
				{
					var distance = previousCity.GetDistanceFromPosition(currentCity.Latitude,
						currentCity.Longitude);

					distanceToTravel += distance;
				}

				previousCity = currentCity;
			}

			return distanceToTravel;
		}

		#endregion
	}
}


