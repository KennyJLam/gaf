﻿using System;
using GAF;
using System.Collections.Generic;
using System.Linq;
using GAF.Extensions;
using System.Text;

namespace Example.IGafLabConsumerFunctions.TravellingSalesman
{
	[GAF.Api.OperatorOptions (PermutationProblem = true)]
	public class TravellingSalesmanFunctions : GAF.Api.IGafLabConsumerFunctions
	{
		#region IGafLabConsumerFunctions implementation

		public Population CreatePopulation ()
		{
			const int populationSize = 100;

			//get our cities
			var cities = CreateCities ().ToList ();

			//Each city is an object the chromosome is a special case as it needs 
			//to contain each city only once. Therefore, our chromosome will contain 
			//all the cities with no duplicates

			//we can create an empty population as we will be creating the 
			//initial solutions manually.
			var population = new Population ();

			//create the chromosomes
			for (var p = 0; p < populationSize; p++) {

				var chromosome = new Chromosome ();
				foreach (var city in cities) {
					chromosome.Genes.Add (new Gene (city));
				}

				chromosome.Genes.ShuffleFast ();
				population.Solutions.Add (chromosome);
			}

			return population;
		}

		public string CreateGenerationCompleteMessage (Population currentPopulation, int currentGeneration, long evaluations)
		{
			var fittest = currentPopulation.GetTop (1) [0];
			var distanceToTravel = CalculateDistance (fittest);
			return string.Format ("Generation: {0}, Fitness: {1}, Distance: {2}", currentGeneration, fittest.Fitness, distanceToTravel);
		}

		public string CreateRunCompleteMessage (Population currentPopulation, int currentGeneration, long evaluations)
		{
			var result = new StringBuilder ("Result: ");
			var fittest = currentPopulation.GetTop (1) [0];
			foreach (var gene in fittest.Genes) {
				result.AppendFormat ("{0}, ", ((City)gene.ObjectValue).Name);
			}

			return result.ToString ().TrimEnd(", ".ToCharArray());
		}

		public bool TerminateAlgorithm (Population population, int currentGeneration, long currentEvaluation)
		{
			return currentGeneration >= 350;
		}

		public double EvaluateFitness (Chromosome chromosome)
		{
			var distanceToTravel = CalculateDistance (chromosome);

			//experience suggests that 1500 is just less than the shortest possible distance
			var fitness = 1600 / distanceToTravel; 
					
			return fitness > 1.0 ? 1.0 : fitness;

		}

		#endregion

		#region Helper Methods

		public static double CalculateDistance (Chromosome chromosome)
		{
			var distanceToTravel = 0.0;
			City previousCity = null;

			//run through each city in the order specified in the chromosome
			foreach (var gene in chromosome.Genes) {
				var currentCity = (City)gene.ObjectValue;

				if (previousCity != null) {
					var distance = previousCity.GetDistanceFromPosition (currentCity.Latitude,
						               currentCity.Longitude);

					distanceToTravel += distance;
				}

				previousCity = currentCity;
			}

			return distanceToTravel;
		}

		private static IEnumerable<City> CreateCities ()
		{
			var cities = new List<City> {
				new City ("Birmingham", 52.486125, -1.890507),
				new City ("Bristol", 51.460852, -2.588139),
				new City ("London", 51.512161, -0.116215),
				new City ("Leeds", 53.803895, -1.549931),
				new City ("Manchester", 53.478239, -2.258549),
				new City ("Liverpool", 53.409532, -3.000126),
				new City ("Hull", 53.751959, -0.335941),
				new City ("Newcastle", 54.980766, -1.615849),
				new City ("Carlisle", 54.892406, -2.923222),
				new City ("Edinburgh", 55.958426, -3.186893),
				new City ("Glasgow", 55.862982, -4.263554),
				new City ("Cardiff", 51.488224, -3.186893),
				new City ("Swansea", 51.624837, -3.94495),
				new City ("Exeter", 50.726024, -3.543949),
				new City ("Falmouth", 50.152266, -5.065556),
				new City ("Canterbury", 51.289406, 1.075802)
			};

			return cities;
		}

		#endregion
	}
}


