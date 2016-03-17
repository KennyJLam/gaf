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

namespace GAF
{
    /// <summary>
    /// This is the interface that all reproductive operators must implement.
    /// </summary>
    /// <remarks>Each operator should ensure that any chromosomes passed from 
    /// the current population to the returned population are cloned 
    /// using the Chromosomes Clone() method. An operator shoult only NOT change
    /// the currentPopulation. It is acceptable for an operator to return a new
    /// population with a different number of chromosomes than the current population.
    /// </remarks>
    [Obsolete("This Interface has been deprecated and may not be available in future releases. Use IGeneticOperator instead.", false)]
    public interface IOperator
    {
        /// <summary>
        /// This method should be used to perform the operation. The the 'currentPopulation' variable will be in an 
		/// unknown state following the call.
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <param name="newPopulation"></param>
        /// <param name="fitnesFunctionDelegate"></param>
        /// <returns>Population</returns>
        void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate);

        /// <summary>
        /// This method should return the number of evaluations that were carried out, 
        /// i.e. the number of times the fitness function was called during the Invoke method.
        /// </summary>
        /// <returns></returns>
        int GetOperatorInvokedEvaluations();

    }
}
