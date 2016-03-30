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
using System.Threading.Tasks;
using GAF.Threading;

namespace GAF.Operators
{
    /// <summary>
    /// Th Swap Mutate operator, when enabled, traverses each gene in the population and, 
    /// based on the probability swaps one gene in the chromosome with another. 
    /// The aim of this operator is to provide mutation without changing any gene values.
    /// </summary>
    public class SwapMutate : IGeneticOperator
    {
        private double _mutationProbabilityS;
        private readonly object _syncLock = new object();

		/// <summary>
		/// Constructor for Unit Testing
		/// </summary>
		internal SwapMutate () : this (1.0)
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mutationProbability"></param>
        public SwapMutate(double mutationProbability)
        {
            _mutationProbabilityS = mutationProbability;
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
        public virtual void Invoke(Population currentPopulation, ref Population newPopulation,
                                   FitnessFunction fitnessFunctionDelegate)
        {

			if (newPopulation == null) {
				newPopulation = currentPopulation.CreateEmptyCopy ();			
			}

			if (!Enabled) return;

			newPopulation.Solutions.Clear();
			newPopulation.Solutions.AddRange(currentPopulation.Solutions);

            foreach (var chromosome in newPopulation.Solutions)
            {
                var mutationProbability = MutationProbability >= 0 ? MutationProbability : 0.0;

				if (chromosome == null) {
					throw new ChromosomeException ("The Cromosome is null.");
				}

				if (!chromosome.IsElite) {
					if (chromosome.Genes == null) {
						throw new GeneException ("The Chromosomes Genes are null.");
					}

					var rd = RandomProvider.GetThreadRandom ().NextDouble ();
					if (rd <= mutationProbability) {

						Mutate (chromosome, mutationProbability);
					}
				}
            }
        }

        /// <summary>
        /// This method is virtual and allows the consumer to override and extend 
        /// the functionality of the operator to be extended within a derived class.
        /// </summary>
		/// <param name="chromosome"></param>
        /// <param name="mutationProbability"></param>
        protected virtual void Mutate(Chromosome chromosome, double mutationProbability)
        {
			var points = GetSwapPoints (chromosome);
			Mutate (chromosome, points[0], points[1]);
        }

		/// <summary>
		/// Exposed for Unit Testing.
		/// </summary>
		/// <param name="chromosome">Genes.</param>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		internal void Mutate(Chromosome chromosome, int first, int second)
		{
			var temp = chromosome.Genes [first];
			chromosome.Genes [first] = chromosome.Genes [second];
			chromosome.Genes [second] = temp;

		}

		/// <summary>
		/// Gets the swap points.
		/// </summary>
		/// <returns>The swap points.</returns>
		/// <param name="chromosome">Gene.</param>
		internal List<int> GetSwapPoints(Chromosome chromosome)
		{
			var result = new List<int> ();

			var first = RandomProvider.GetThreadRandom ().Next (chromosome.Genes.Count - 1);

			var second = 0;
			while (first == second || second == 0) {
				second = RandomProvider.GetThreadRandom ().Next (chromosome.Genes.Count - 1);
			}

			result.Add (first);
			result.Add (second);
			return result;

		}

        /// <summary>
        /// Returns the number of evaluations performed by this operator.
        /// </summary>
        /// <returns></returns>
        public int GetOperatorInvokedEvaluations()
        {
            return 0;
        }

        /// <summary>
        /// Sets/gets the Mutation probabilty. The setting and getting of this property is thread safe.
        /// </summary>
        public double MutationProbability
        {
            get
            {
                lock (_syncLock)
                {
                    //this only locks the object, not its members
                    //this is ok as the MutationProbability object is immutable.
                    return _mutationProbabilityS;
                }
            }
            set
            {
                lock (_syncLock)
                {
                    _mutationProbabilityS = value;
                }
            }
        }
    }
}
