﻿/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GAF;
using GAF.Extensions;
using GAF.Operators;

namespace Example.TravellingSalesman
{
    internal class Program
    {
		private static Stopwatch _stopwatch;

		private static void Main(string[] args)
        {
            const int populationSize = 100;
			_stopwatch = new Stopwatch ();

            //get our cities
            var cities = CreateCities().ToList();

            //Each city is an object the chromosome is a special case as it needs 
            //to contain each city only once. Therefore, our chromosome will contain 
            //all the cities with no duplicates

            //we can create an empty population as we will be creating the 
            //initial solutions manually.
            var population = new Population();
			
			//create the chromosomes
            for (var p = 0; p < populationSize; p++)
            {

                var chromosome = new Chromosome();
                foreach (var city in cities)
                {
                    chromosome.Genes.Add(new Gene(city));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }
			
			//create the elite operator
            var elite = new Elite(5);
			
			//create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };
			
			//create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

			//hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;
			
			//add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);
			
			//run the GA
            ga.Run(Terminate);

            Console.Read();
        }

        static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            foreach (var gene in fittest.Genes)
            {
                Console.WriteLine(((City)gene.ObjectValue).Name);
            }
			Console.WriteLine (((City)fittest.Genes[0].ObjectValue).Name);
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var distanceToTravel = CalculateDistance(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Distance: {2}", e.Generation, fittest.Fitness, distanceToTravel);

        }

        private static IEnumerable<City> CreateCities()
        {
            var cities = new List<City>
            {
                new City("Birmingham", 52.486125, -1.890507),
                new City("Bristol", 51.460852, -2.588139),
                new City("London", 51.512161, -0.116215),
                new City("Leeds", 53.803895, -1.549931),
                new City("Manchester", 53.478239, -2.258549),
                new City("Liverpool", 53.409532, -3.000126),
                new City("Hull", 53.751959, -0.335941),
                new City("Newcastle", 54.980766, -1.615849),
                new City("Carlisle", 54.892406, -2.923222),
                new City("Edinburgh", 55.958426, -3.186893),
                new City("Glasgow", 55.862982, -4.263554),
                new City("Cardiff", 51.488224, -3.186893),
                new City("Swansea", 51.624837, -3.94495),
                new City("Exeter", 50.726024, -3.543949),
                new City("Falmouth", 50.152266, -5.065556),
                new City("Canterbury", 51.289406, 1.075802)
            };

            return cities;
        }

        public static double CalculateFitness(Chromosome chromosome)
        {
			var distanceToTravel = CalculateDistance(chromosome);

			//experience suggests that 2000 is just less than the shortest possible distance
			var fitness = 2000 / distanceToTravel;
			return fitness > 1.0 ? 1.0 : fitness;

        }

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
                    distanceToTravel += previousCity.GetDistanceFromPosition(currentCity.Latitude,
                                                                        currentCity.Longitude);
                 }
			
                previousCity = currentCity;
            }

			//add distance back to the starting point
			var firstCity = (City)chromosome.Genes[0].ObjectValue;
			distanceToTravel += previousCity.GetDistanceFromPosition (firstCity.Latitude,
													firstCity.Longitude);

            return distanceToTravel;
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 400;
        }
    }
}
