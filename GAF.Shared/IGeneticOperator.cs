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
    /// This is the interface that should be implemented when creating custom genetic operators.
    /// </summary>
#pragma warning disable 618
    public interface IGeneticOperator : IOperator
#pragma warning restore 618
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
        bool Enabled { set; get; }
    }
}
