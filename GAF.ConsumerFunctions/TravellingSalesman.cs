using System;

namespace GAF.ConsumerFunctions
{
	public class TravellingSalesman : IConsumerFunctions
	{
		#region IConsumerFunctions implementation
		public double EvaluateFitness (Chromosome chromosome)
		{
			var distanceToTravel = CalculateDistance(chromosome);
			return 1 - distanceToTravel / 10000;
		}
		public bool TerminateAlgorithm (Population population, int currentGeneration, long currentEvaluation)
		{
			return currentGeneration > 400;
		}
		#endregion

		#region Helper Methods

		private static double CalculateDistance(Chromosome chromosome)
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


