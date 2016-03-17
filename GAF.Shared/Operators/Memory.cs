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

namespace GAF.Operators
{
    /// <summary>
    /// This class, relies on the new population being fully populated and is designed to be the last operator in the chain.
    /// </summary>
    public class Memory : IGeneticOperator
    {
        private Population _memory;
        private int _evaluations;
        private readonly int _memorySize;
        private readonly int _generationalUpdatePeriod;
        private int _currentGeneration = 0;

        /// <summary>
        /// This memory operator is designed to store recently identified solutions.
        /// The operator stores the fittest solution determined and stores this after
        /// a period of generations determined by the generationsBetweenUpdate parameter.
        /// Once the memory is full, older solutions are overwritten.
        /// </summary>
        /// <param name="memorySize"></param>
        /// <param name="generationsBetweenUpdate"></param>
        public Memory(int memorySize, int generationsBetweenUpdate)
        {
            Enabled = false;
            _memorySize = memorySize;
            _generationalUpdatePeriod = generationsBetweenUpdate;
            
            Enabled = true;

        }
        /// <summary>
        /// This is the method that invokes the operator. This should not normally be called explicitly.
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <param name="newPopulation"></param>
        /// <param name="fitnessFunctionDelegate"></param>
        public void Invoke(Population currentPopulation, ref Population newPopulation,
                           FitnessFunction fitnessFunctionDelegate)
        {

            _currentGeneration++;

            if (newPopulation == null)
                newPopulation = new Population(0,
                                               0,
                                               currentPopulation.ReEvaluateAll,
                                               currentPopulation.LinearlyNormalised,
                                               currentPopulation.ParentSelectionMethod);

            if (!Enabled) return;


            //create the memory if one doesn't exist
            if (_memory == null)
            {
                _memory = new Population(_memorySize, 0, true, false);
            }

            //if the new population is null, create an empty population
            if (newPopulation == null || newPopulation.Solutions.Count != currentPopulation.Solutions.Count)
            {
                throw new ArgumentException(
                    "The current Population and the new Population contain a different number of Solutions.");
            }

            //if what we have in memory is better than the best in the current population, copy this with the
            //best of the current population to the new population. In this case we don't need to update the memory.
            //re evaluate the memory
            _evaluations = _memory.Evaluate(fitnessFunctionDelegate);

            //compare best memory with population and replace worst in population if appropriate
            var memorySolution = _memory.GetTop(1)[0];
            var bestInPopulation = currentPopulation.GetTop(1)[0];

            if (memorySolution.Fitness > bestInPopulation.Fitness)
            {
                //replace the worst with the best from memory
                newPopulation.DeleteLast();
                newPopulation.Solutions.Add(memorySolution);
            }
            else
            {
                //TODO: Adding these every n generations 
                //simply add the best to our memory
                if (_currentGeneration % _generationalUpdatePeriod == 0)
                {
                    this.AddToMemory(newPopulation.GetTop(1)[0]);
                }
            }
        }
        /// <summary>
        /// Returns the number of evaluations performed by this operator.
        /// </summary>
        /// <returns></returns>
        public int GetOperatorInvokedEvaluations()
        {
            return _evaluations;
        }

        /// <summary>
        /// Enabled property. Diabling this operator will cause the population to 'pass through' unaltered.
        /// </summary>
        public bool Enabled { set; get; }

        private void AddToMemory(Chromosome solution)
        {
            //simply add the best to our memory
			_memory.Solutions.Add(solution);
            //_memory.Solutions.Add(solution.DeepClone());
            if (_memory.Solutions.Count > _memorySize)
            {
                //remove the oldest
                _memory.Solutions.RemoveAt(0);
            }
        }
    }
}
