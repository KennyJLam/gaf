using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GAF.Extensions;
using GAF.Threading;

namespace GAF.Operators
{
    /// <summary>
    /// This operator will replace the weakest solutions in the new population 
    /// with the selected amount (by percentatge) of randomly generated solutions 
    /// (Chromosomes) from the current population. Any chromosome marked as Elite
    /// will not be replaced. Therefore, 50% of a population of 100 that has 10
    /// 'Elites' will replace 45 solutions.
    /// </summary>
    public class RandomReplace : IGeneticOperator
    {
        private readonly object _syncLock = new object();
        private bool _allowDuplicatesS;
        private FitnessFunction _fitnessFunctionDelegate;
        private int _evaluations;

        private int _percentageToReplace;

        /// <summary>
        /// Replaces the whole population with randomly generated solutions.
        /// </summary>
        internal RandomReplace()
        {
        }

        /// <summary>
        /// Replaces the whole population with randomly generated solutions.
        /// </summary>
        public RandomReplace(bool allowDuplicates)
            :this(100, allowDuplicates)
        {
        }

        /// <summary>
        /// Replaces the specified number of the weakest individuals, with randomly generated ones.
        /// </summary>
        /// <param name="percentageToReplace">Set the number to replace.</param>
        public RandomReplace(int percentageToReplace)
            :this(percentageToReplace, false)
        {
        }

        /// <summary>
        /// Replaces the specified number of the weakest individuals, with randomly generated ones.
        /// </summary>
        /// <param name="percentageToReplace">Set the number to replace.</param>
        /// <param name="allowDuplicates"></param>
        public RandomReplace(int percentageToReplace, bool allowDuplicates)
        {
            _percentageToReplace = percentageToReplace;
            _allowDuplicatesS = allowDuplicates;
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
            if (newPopulation == null)
                newPopulation = new Population(0,
                                               0,
                                               currentPopulation.ReEvaluateAll,
                                               currentPopulation.LinearlyNormalised,
                                               currentPopulation.ParentSelectionMethod);

            if (!Enabled) return;

			if (currentPopulation.Solutions != null && currentPopulation.Solutions.Count > 0) {
				throw new ArgumentException ("There are no Solutions in the current Population.");
			}

			if(currentPopulation.Solutions[0].Genes.Any(g => g.GeneType != GeneType.Binary))
            {
                throw new Exception("Only Genes with a GeneType of Binary can be handled by the RandomReplace operator.");
            }

            _fitnessFunctionDelegate = fitnessFunctionDelegate;

            Replace(currentPopulation, ref newPopulation, this.Percentage, this.AllowDuplicates, _fitnessFunctionDelegate);

        }

        /// <summary>
        /// Helper Method marked as Internal for Unit Testing purposes.
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <param name="newPopulation"></param>
        /// <param name="percentage"></param>
        /// <param name="allowDuplicates"></param>
        /// <param name="fitnessFunctionDelegate"></param>
        internal void Replace(Population currentPopulation, ref Population newPopulation, int percentage, bool allowDuplicates, FitnessFunction fitnessFunctionDelegate)
        {

			if (currentPopulation.Solutions.Count == 0) {
			
			}
            //copy everything accross in order of fitness i.e. Elites at the top
			newPopulation.Solutions = currentPopulation.Solutions;
			newPopulation.Solutions.Sort ();

            //find the number of non elites
			var chromosomeCount = newPopulation.Solutions.Count(s => !s.IsElite);

            //determine how many we are replacing based on the percentage
			var numberToReplace = (int)System.Math.Round((chromosomeCount / 100.0) * percentage);

            //we fill it up if we are short.
            if (numberToReplace > chromosomeCount)
            {
                numberToReplace = chromosomeCount;
            }

            if (numberToReplace > 0)
            {
                //we are adding random imigrants to the new population
                if (newPopulation == null || newPopulation.PopulationSize < numberToReplace)
                {
                    throw new ArgumentException(
                        "The 'newPopulation' does not contain enough solutions for the current operation.");
                }

				//reduce the population as required
				newPopulation.Solutions.RemoveRange(chromosomeCount - numberToReplace,numberToReplace);

				var chromosomeLength = currentPopulation.ChromosomeLength;

                //var immigrants = new List<Chromosome>();
                for (var index = 0; index < numberToReplace; index++)
                {
					if (!allowDuplicates) {

						Chromosome uniqueChromosome;

						//if the new population is empty
						uniqueChromosome = CreateUniqueChromosome (chromosomeLength, newPopulation);
						uniqueChromosome.Evaluate (fitnessFunctionDelegate);
						AddImigrant (newPopulation, uniqueChromosome, fitnessFunctionDelegate);

						//should find one only
						var duplicates = newPopulation.Solutions.FindAll (s => s.ToString ().Equals (uniqueChromosome.ToString ()));
						if (duplicates.Count != 1) {

							throw new ChromosomeException ("The Chromosome was not unique");
						}
					}
                    else
                    {
						AddImigrant (newPopulation, new Chromosome(chromosomeLength), fitnessFunctionDelegate);
                    }

                }

//	                //need to add these to the solution, sort and then remove the weakest
//	                var imigrantCount = immigrants.Count;
//	                if (imigrantCount > 0)
//	                {
//	                    foreach (var imigrant in immigrants)
//	                    {
//	                        imigrant.Evaluate(fitnessFunctionDelegate);
//	                        _evaluations++;
//	                    }
//
//	                    //dont want to remove elites so sort
//	                    newPopulation.Solutions.Sort();
//	                    newPopulation.Solutions.RemoveRange(newPopulation.Solutions.Count - imigrantCount, imigrantCount);
//	                    newPopulation.Solutions.AddRange(immigrants);
//	                }

            }
            else
            {
                //do nothing
            }

        }

		private void AddImigrant(Population population, Chromosome imigrant, FitnessFunction fitnessFunctionDelegate)
		{
			//need to add these to the solution, sort and then remove the weakest
			if (imigrant != null && population != null) {
				
				imigrant.Evaluate (fitnessFunctionDelegate);
				_evaluations++;

				//TODO: Fix this, Random does not want to remove weakest as we are trying to increase diversityr, it needs to add random.
				//dont want to remove elites so sort
				//population.Solutions.Sort ();

				//add the imigrant this extends the population
				population.Solutions.Add (imigrant);

			}
		}

		/// <summary>
		/// Creates a random unique Binary Gene. The RandomReplacement operator can only be used with Binary Genes. 
		/// </summary>
		/// <returns>The unique chromosome.</returns>
		/// <param name="chromosomeLength">Chromosome length.</param>
		/// <param name="population">Population.</param>
		internal Chromosome CreateUniqueChromosome(int chromosomeLength, Population population)
        {

			Chromosome rndChromosome = null;

			const int maxAttempts = 100; //give up after 10 attempts
			var success = false;

			if (chromosomeLength == 0)
				throw new ChromosomeNotUniqueException ("The chromosome length is set to zero. Zero length chromosomes cannot be unique.");

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                rndChromosome = new Chromosome(chromosomeLength);

				if (!population.SolutionExists(rndChromosome))
                {
					success = true;
                    break;
                }

            }
				
			if(!success)
			{
				throw new ChromosomeNotUniqueException ("It has not been possible to create a unique random Chromosome. This may be because the Chromosome length is two short or the Population is two large.");
			}

            return rndChromosome;

        }

//		private int GetChromosomeLength(Population population)
//		{
//			var result = 0;
//			if (population.Solutions != null &&
//				population.Solutions.Count > 0 &&
//				population.Solutions [0].Genes != null) {
//
//				result = population.Solutions [0].Genes.Count;
//			}
//
//			return result;
//		}

        /// <summary>
        /// Returns the number of evaluations performed by this operator.
        /// </summary>
        /// <returns></returns>
        public int GetOperatorInvokedEvaluations()
        {
            return _evaluations;
        }

        /// <summary>
        /// Sets/Gets the Percentage number to be replaced. The setting and getting of this property is thread safe.
        /// </summary>
        public int Percentage
        {
            get
            {
                //not really needed as 32bit int updates are atomic on 32bit systems 
                lock (_syncLock)
                {
                    return _percentageToReplace;
                }
            }

            set
            {
                lock (_syncLock)
                {
                    _percentageToReplace = value;
                }
            }
        }

        ///// <summary>
        ///// Sets/Gets the Number of solutions to replace. The setting and getting of this property is thread safe.
        ///// </summary>
        //public int NumberToReplace
        //{
        //    get
        //    {
        //        //not really needed as 32bit int updates are atomic on 32bit systems 
        //        lock (_syncLock)
        //        {
        //            return _numberToReplace;
        //        }
        //    }

        //    set
        //    {
        //        lock (_syncLock)
        //        {
        //            _numberToReplace = value;
        //        }
        //    }
        //}

        /// <summary>
        /// Sets/Gets whether duplicates are allowed in the population. 
        /// The setting and getting of this property is thread safe.
        /// </summary>
        public bool AllowDuplicates
        {
            get
            {
                lock (_syncLock)
                {
                    return _allowDuplicatesS;
                }
            }
            set
            {
                lock (_syncLock)
                {
                    _allowDuplicatesS = value;
                }
            }
        }
    }
}
