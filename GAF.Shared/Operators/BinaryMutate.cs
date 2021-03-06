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
using System.Security;
using System.Text;
using System.Threading.Tasks;
using GAF.Threading;
using GAF.Extensions;

namespace GAF.Operators
{
	/// <summary>
	/// The Binary Mutation Operator, when enabled, traverses each gene 
	/// in the population and, based on the probability swaps a gene 
	/// from one state to the other. The aim of this opperator is to 
	/// better reflect nature and add diversity to the population.
	/// This operator cannot be used with genes of type Object.
	/// </summary>
	public class BinaryMutate : IGeneticOperator
	{
		private double _mutationProbabilityS;
		private bool _allowDuplicatesS;
		private readonly object _syncLock = new object ();

		/// <summary>
		/// Event definition for the LoggingEventHandler event handler.
		/// </summary>
		public event LoggingEventHandler OnLogging;

		/// <summary>
		/// Internal Constructor for unit Testing.
		/// </summary>
		internal BinaryMutate () : this (1.0)
		{

		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mutationProbability"></param>
		public BinaryMutate (double mutationProbability)
		{
			_mutationProbabilityS = mutationProbability;
			_allowDuplicatesS = true;
			Enabled = true;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mutationProbability"></param>
		/// <param name="allowDuplicates"></param>
		public BinaryMutate (double mutationProbability, bool allowDuplicates)
		{
			_mutationProbabilityS = mutationProbability;
			_allowDuplicatesS = allowDuplicates;
			Enabled = true;
		}

		/// <summary>
		/// Enabled property. Diabling this operator will cause the population to 'pass through' unaltered.
		/// </summary>
		public bool Enabled { set; get; }

		/// <summary>
		/// This is the method that invokes the operator. This should not normally be called explicitly.
		/// 
		/// This method is virtual and allows the consumer to override and extend 
		/// the functionality of the operator to be extended        /// </summary>
		/// <param name="currentPopulation"></param>
		/// <param name="newPopulation"></param>
		/// <param name="fitnessFunctionDelegate"></param>
		public virtual void Invoke (Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
		{

			if (newPopulation == null) {
				newPopulation = currentPopulation.CreateEmptyCopy ();
			}

			if (!Enabled)
				return;

			if (currentPopulation.Solutions == null || currentPopulation.Solutions.Count == 0) {
				throw new ArgumentException ("There are no Solutions in the current Population.");
			}

			if (currentPopulation.Solutions [0].Genes.Any (g => g.GeneType != GeneType.Binary)) {
				throw new Exception ("Only Genes with a GeneType of Binary can be handled by the BinaryMutate operator.");
			}

			//copy everything accross including elites
			newPopulation.Solutions.Clear ();
			newPopulation.Solutions.AddRange (currentPopulation.Solutions);

			//run through the non elites mutating as required
			var solutionsToProcess = newPopulation.GetNonElites ();

			foreach (var chromosome in solutionsToProcess) {

				var mutationProbability = MutationProbability >= 0 ? MutationProbability : 0.0;

				if (AllowDuplicates) {

					//allowing duplicates so simply mutate
					Mutate (chromosome, mutationProbability);

				} else {

					//duplicates not allowed so we have to clone the chromosome
					var clonedChromosome = chromosome.DeepClone ();

					//mutate the clone
					Mutate (clonedChromosome, mutationProbability);

					//only add the mutated chromosome if it does not exist otherwise do nothing
					if (!newPopulation.SolutionExists (clonedChromosome)) {

						//swap existing genes for the mutated onese
						chromosome.Genes = clonedChromosome.Genes;
					}
				}
			}
		}

		/// <summary>
		/// Internal Method for Unit test purposes.
		/// </summary>
		/// <param name="child"></param>
		internal void Mutate (Chromosome child)
		{
			Mutate (child, MutationProbability);
		}

		/// <summary>
		/// This method is virtual and allows the consumer to override and extend 
		/// the functionality of the operator to be extended within a derived class.
		/// </summary>
		/// <param name="child"></param>
		/// <param name="mutationProbability"></param>
		protected virtual void Mutate (Chromosome child, double mutationProbability)
		{
			//cannot mutate elites or else we will ruin them
			if (child.IsElite)
				return;

			if (child == null || child.Genes == null) {
				throw new ArgumentException ("The Cromosome is either null or the Chromosomes Genes are null.");
			}

			foreach (var gene in child.Genes) {
				if (gene.GeneType == GeneType.Object) {
					throw new OperatorException ("Genes with a GeneType of Object cannot be mutated by the BinaryMutate operator.");
				}

				//Debug.WriteLine("Calculated Mutation Probability: {0}", workingProbability);

				//check probability by generating a random number between zero and one and if 
				//this number is less than or equal to the given mutation probability 
				//e.g. 0.001 then the bit value is changed.
				var rd = RandomProvider.GetThreadRandom ().NextDouble ();

				if (rd <= mutationProbability) {
					//we are changing a chromosome so any existing fitness is now inappropriate
					child.Fitness = 0;
					child.FitnessNormalised = 0;

					switch (gene.GeneType) {
					case GeneType.Binary: {
							gene.ObjectValue = !(bool)gene.ObjectValue;
							break;
						}
					case GeneType.Real: {
							gene.ObjectValue = (double)gene.ObjectValue * -1;
							break;
						}
					case GeneType.Integer: {
							gene.ObjectValue = (int)gene.ObjectValue * -1;
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the number of evaluations performed by this operator.
		/// </summary>
		/// <returns></returns>
		public int GetOperatorInvokedEvaluations ()
		{
			return 0;
		}

		/// <summary>
		/// Sets/gets the Mutation probabilty. The setting and getting of this property is thread safe.
		/// </summary>
		public double MutationProbability {
			get {
				lock (_syncLock) {
					//this only locks the object, not its members
					//this is ok as the MutationProbability object is immutable.
					return _mutationProbabilityS;
				}
			}
			set {
				lock (_syncLock) {
					_mutationProbabilityS = value;
				}
			}
		}

		/// <summary>
		/// Sets/Gets whether duplicates are allowed in the population. 
		/// The setting and getting of this property is thread safe.
		/// </summary>
		public bool AllowDuplicates {
			get {
				lock (_syncLock) {
					return _allowDuplicatesS;
				}
			}
			set {
				lock (_syncLock) {
					_allowDuplicatesS = value;
				}
			}
		}
	}
}
