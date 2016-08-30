/*
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
using GAF;
using GAF.Operators;
using System.Diagnostics;
using System.Globalization;

namespace Example.BinaryF6
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const double crossoverProbability = 0.85;
            const double mutationProbability = 0.08;
            const int elitismPercentage = 5;

            //create a Population of 100 random chromosomes of length 44
            var population = new Population(100, 44, false, false);

            //create the genetic operators 
            var elite = new Elite(elitismPercentage);

            var crossover = new Crossover(crossoverProbability, true)
            {
                CrossoverType = CrossoverType.SinglePoint
            };

            var mutation = new BinaryMutate(mutationProbability);

            //create the GA itself 
            var ga = new GeneticAlgorithm(population, EvaluateFitness);

            //subscribe to the GAs Generation Complete event 
            ga.OnGenerationComplete += ga_OnGenerationComplete;

            //add the operators to the ga process pipeline 
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            //run the GA 
            ga.Run(TerminateAlgorithm);

        }

        public static double EvaluateFitness(Chromosome chromosome) 
        {
            double fitnessValue = -1;
            if (chromosome != null)
            {
                //this is a range constant that is used to keep the x/y range between -100 and +100
                var rangeConst = 200 / (System.Math.Pow(2, chromosome.Count / 2) - 1);

                //get x and y from the solution
                var x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count / 2), 2);
                var y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count / 2, chromosome.Count / 2), 2);

                //Adjust range to -100 to +100
                var x = (x1 * rangeConst) - 100;
                var y = (y1 * rangeConst) - 100;

                //using binary F6 for fitness.
                var temp1 = System.Math.Sin(System.Math.Sqrt(x * x + y * y));
                var temp2 = 1 + 0.001 * (x * x + y * y);
                var result = 0.5 + (temp1 * temp1 - 0.5) / (temp2 * temp2);

                fitnessValue = 1 - result;
            }
            else
            {
                //chromosome is null
                throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
            }

            return fitnessValue;
        }

        public static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration >= 500;     
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {

            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];

            //decode chromosome

            //get x and y from the solution 
            var x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count/2), 2);
            var y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count/2, chromosome.Count/2), 2);

            //Adjust range to -100 to +100 
            var rangeConst = 200 / (System.Math.Pow(2, chromosome.Count / 2) - 1);
            var x = (x1 * rangeConst) - 100;
            var y = (y1*rangeConst) - 100;

            //display the X, Y and fitness of the best chromosome in this generation 
            Console.WriteLine("Generation: {0} Evaluation: {1} x:{2} y:{3} Max Fitness:{4}", 
                e.Generation.ToString(CultureInfo.InvariantCulture),
                e.Evaluations.ToString(CultureInfo.InvariantCulture),
                x, 
                y, 
                e.Population.MaximumFitness);
        }
    }
}
