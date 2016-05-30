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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GAF.Extensions;

namespace GAF
{
    /// <summary>
    /// This class provides statistics relating to a population.
    /// </summary>
    internal class PopulationStatistics
    {

        //NOTE: THIS MUST BE DIVISIBLE BY 4 !!!
        private const int BucketCount = 16;
        private const int UpperQuartileBucketCount = BucketCount / 4;

        private readonly Population _population;
        private readonly int _populationSize;
        private readonly int _chromosomeLength;

        private double _averageHammingDistance;
        private double _averageFitness;
        private double _standardDeviation;
        private double _minimumFitnes;
        private double _maximumFitness;
        private int[] _diversityGraph;
        private int _upperQuartileCount;

        #region Constructor
        /// <summary>
        /// Constructor that accepts the Population object to be analysed. The includeHammingDistance
        /// parameter causes the class to calculate the Hamming Distance of the population. This should be used 
        /// with care as setting this property to true could have performance implecations.
        /// </summary>
        /// <param name="population"></param>
        /// <param name="includeHammingDistance"></param>
        public PopulationStatistics(Population population, bool includeHammingDistance)
        {

            _population = population;
            _populationSize = population.PopulationSize;
            _chromosomeLength = GetChromosomeLength();
            _diversityGraph = new int[BucketCount];
            _maximumFitness = 0;
            _minimumFitnes = 1.0;
            _averageHammingDistance = -1;

            //do the basics ??
            Process(includeHammingDistance);
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the average Hamming Distance. This property will only return the Hamming Distance
        /// if the includeHammingDistance parameter is set to true.
        /// </summary>
        public double AverageHammingDistance
        {
            get { return _averageHammingDistance; }
        }
        /// <summary>
        /// Returns the average fitness value of the population.
        /// </summary>
        public double AverageFitness
        {
            get { return _averageFitness; }
        }

        /// <summary>
        /// Reurns the standard deviation of the population, based on fitness.
        /// </summary>
        public double StandardDeviation
        {
            get { return _standardDeviation; }
        }

        /// <summary>
        /// Returns the minimum fitness value of the population.
        /// </summary>
        public double MinimumFitness
        {
            get { return _minimumFitnes; }
        }

        /// <summary>
        /// Returns the maximum fitness value of the population.
        /// </summary>
        public double MaximumFitness
        {
            get { return _maximumFitness; }
        }

        /// <summary>
        /// Returns the number of duplicates in the population.
        /// </summary>
        public int GetDuplicateCount
        {

            get
            {
                double percentageUnique = 0.0;
                var q = from solution in _population.Solutions
                        group solution by solution.ToBinaryString()
                            into uniqueSolution
                        select uniqueSolution;

                var count = q.ToList().Count;

                if (count > 0)
                {
                    percentageUnique = ((double)count / _populationSize) * 100;
                }
                return 100 - (int)System.Math.Round(percentageUnique);

            }
        }
        /// <summary>
        /// Returns an array representing a graph of the populations diversity based on fitness.
        /// </summary>
        public int[] DiversityGraph
        {
            get { return _diversityGraph; }
        }
        /// <summary>
        /// Returns the number of sollutions in the population that are within the upper quartile 
        /// based on the fitmess value.
        /// </summary>
        public int UpperQuartileCount
        {
            get { return _upperQuartileCount; }
        }

        #endregion

        #region Private Functions

		private int GetChromosomeLength()
		{
			var result = 0;
			if (_population.Solutions != null &&
			   _population.Solutions.Count > 0 &&
			   _population.Solutions [0].Genes != null) {
			
				result = _population.Solutions [0].Genes.Count;
			}

			return result;
		}


        private double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            var enumerable = values as IList<double> ?? values.ToList();

            if (enumerable.Any())
            {
                //Compute the Average      
                var avg = enumerable.Average();
                //Perform the Sum of (value-avg)_2_2      
                var sum = enumerable.Sum(d => System.Math.Pow(d - avg, 2));
                //Put it all together      
                ret = System.Math.Sqrt((sum) / (enumerable.Count() - 1));
            }
            return ret;
        }

        private double GetUpperQuartile(int[] buckets)
        {
            const int upperQuartileBucketCount = BucketCount/4;
            var popSize = _populationSize;

            var result = 0;

            for (var bucket = BucketCount - upperQuartileBucketCount; bucket < BucketCount; bucket++)
            {
                result += buckets[bucket];
            }

            return ((double) result/popSize)*100;
        }

        #endregion
        #region Main Loop

        private void Process(bool includeHammingDistance)
        {
            var solutions = _population.Solutions;
            var totalFitness = 0.0;
            var fitnessList = new List<double>(_populationSize);
            var totalHammingDistance = 0.0;

            //loop through each solution
            for (var sIndex = 0; sIndex < _populationSize; sIndex++)
            {

                #region Storing Data for Later

                //store the values for using later on with Average and StddDev
                var currentFitness = solutions[sIndex].Fitness;
                totalFitness += currentFitness;
                fitnessList.Add(currentFitness);

                #endregion

                #region Max and Min

                if (currentFitness < _minimumFitnes) _minimumFitnes = currentFitness;
                if (currentFitness > _maximumFitness) _maximumFitness = currentFitness;

                #endregion

                #region Hamming Distance

                //TODO: Deal with non binary chromosomes.
                if (includeHammingDistance)
                {
                    //compare the current solution with all others. Note that we only need
                    //to compare each once hence the shortened 'innerIndex' loop
                    for (var innerIndex = sIndex + 1; innerIndex < _populationSize; innerIndex++)
                    {
                        var string1 = solutions[sIndex].ToBinaryString();
                        var string2 = solutions[innerIndex].ToBinaryString();

                        //TODO: Look at converting the binary to an integer and using the bitwise function (See below)
                        for (var index = 0; index < _chromosomeLength; index++)
                        {
                            if (string1[index] != string2[index])
                            {
                                totalHammingDistance++;
                            }
                        }
                        //our gene is actually strored as a double.
                        //we also need to make sure we dont cause errors is a non binary chromosome is used;
                        //e.g. Here is an example in C comparing two integers
                        /*
                            def hamming2(x,y):
                                """Calculate the Hamming distance between two bit strings"""
                                assert len(x) == len(y)
                                count,z = 0,x^y
                                while z:
                                    count += 1
                                    z &= z-1 # magic!
                                return count                          
                         */


                    }
                }

                #endregion

                #region Diversity Graph

                var bucket = (int)System.Math.Floor(currentFitness / (1.0 / (double)BucketCount));

                //if fitness ends up being 1.0 we could end up with a bucket of 16 which would fail
                if (bucket == BucketCount) bucket--;

                _diversityGraph[bucket]++;

                #endregion

            }

            #region Average Fitness

            if (totalFitness != 0 && _populationSize != 0)
            {
                _averageFitness = totalFitness / _populationSize;
            }

            #endregion

            #region Standard deviation

            _standardDeviation = CalculateStdDev(fitnessList);

            #endregion

            var uqCount = 0;

            for (var bucket = BucketCount - UpperQuartileBucketCount; bucket < BucketCount; bucket++)
            {
                uqCount += _diversityGraph[bucket];
            }

            //sample size is given by = N(N-1)/2
            var sampleSize = (_populationSize * (_populationSize - 1)) / 2.0;
            var maxEvaluations = sampleSize * _chromosomeLength;
            _averageHammingDistance = (totalHammingDistance / maxEvaluations) * 100;

            #region Upper Quartile

            _upperQuartileCount = (int)(((double)uqCount / _populationSize) * 100);

            #endregion
        }

        #endregion

    }
}

