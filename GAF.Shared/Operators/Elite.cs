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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GAF.Operators
{
    /// <summary>
    /// The Elite operator, when enabled, ensures that a specified percentage of the fittest 
    /// chromosomes are passed to the next generation without modification.
    /// 
    /// The Elite operator ensures that each generation produces a solution that is at least 
    /// as good as that produced by the previous generation.
    /// 
    /// This operator will append the selected percentatge of solutions (Chromosomes) from the 
    /// current population to the new population. The selected number (%) of solutions will be copied
    /// to the new population as long as there is space within the population, irrespective of the 
    /// specified percentage to copy. 
    /// 
    /// This operator will only copy as many as are required to fill the population.
    /// This operator does not consider duplicates. To maintain a unique population, ensure that this operator
    /// is used before any operators that modify the solutions.
    /// 
    /// </summary>
    public class Elite : IGeneticOperator
    {
        private int _percentageS;
        private readonly object _syncLock = new object();
/// <summary>
/// Constructor.
/// </summary>
/// <param name="elitismPercentage"></param>
        public Elite(int elitismPercentage)
        {
            _percentageS = elitismPercentage;
            Enabled = true;
        }

        /// <summary>
        /// Enabled property. Diabling this operator will cause the population to 'pass through' unaltered.
        /// </summary>
        public bool Enabled { set; get; }

        /// <summary>
        /// This is the method that invokes the operator. This should not normally be called explicitly.
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <param name="newPopulation"></param>
        /// <param name="fitnessFunctionDelegate"></param>
        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
        {
            //Debug.WriteLine(string.Format("Elite: {0}", currentPopulation.ParentSelectionMethod));

            //if the new population is null, create an empty population
            if (newPopulation == null)
                newPopulation = new Population(0,
                                               0,
                                               currentPopulation.ReEvaluateAll,
                                               currentPopulation.LinearlyNormalised,
                                               currentPopulation.ParentSelectionMethod);

            if (!Enabled) return;
            
			CopyElites (currentPopulation, ref newPopulation , Percentage);

//            var chromosomes = currentPopulation.GetTopPercent(Percentage);
//            chromosomes.ForEach(c => c.IsElite = true);
//
//            //we need to establish how many to move over
//            var populationSize = currentPopulation.Solutions.Count();
//            var newPopultionSize = newPopulation.Solutions.Count;
//            var numberToCopy = chromosomes.Count();
//
//            //restrict the amount to copy if we run out of space
//            if (newPopultionSize < populationSize)
//            {
//                //ok to copy some at least
//                if (numberToCopy > populationSize - newPopultionSize)
//                {
//                    numberToCopy = populationSize - newPopultionSize;
//                }
//
//                newPopulation.Solutions.AddRange(chromosomes.Take(numberToCopy));
//
//            }

        }

		internal void CopyElites(Population currentPopulation, ref Population newPopulation, int percentage)
		{

			newPopulation.Solutions.Clear ();
			newPopulation.Solutions.AddRange(currentPopulation.Solutions);

			//reset
			foreach (var chromosome in newPopulation.Solutions) {
				chromosome.IsElite = false;
			}

			//get the top n% and set as elites
			var chromosomes = newPopulation.GetTopPercent(percentage);
			foreach (var chromosome in chromosomes) {
				chromosome.IsElite = true;
			}

			var max = newPopulation.MaximumFitness;


//			//we need to establish how many to move over
//			var populationSize = currentPopulation.Solutions.Count();
//			var newPopultionSize = newPopulation.Solutions.Count;
//			var numberToCopy = chromosomes.Count();
//
//			//restrict the amount to copy if we run out of space
//			if (newPopultionSize < populationSize)
//			{
//				//ok to copy some at least
//				if (numberToCopy > populationSize - newPopultionSize)
//				{
//					numberToCopy = populationSize - newPopultionSize;
//				}
//
//				newPopulation.Solutions.AddRange(chromosomes.Take(numberToCopy));
//
//			}
		
		}
        /// <summary>
        /// Returns the number of evaluations performed by this operator.
        /// </summary>
        /// <returns></returns>
        public int GetOperatorInvokedEvaluations()
        {
            //no evaluations done
            return 0;
        }

        /// <summary>
        /// Sets/Gets the Percentage Elites. The setting and getting of this property is thread safe.
        /// </summary>
        public int Percentage
        {
            get
            {
                //not really needed as 32bit int updates are atomic on 32bit systems 
                lock (_syncLock)
                {
                    return _percentageS;
                }
            }

            set
            {
                lock (_syncLock)
                {
                    _percentageS = value;
                }
            }
        }
    }
}
