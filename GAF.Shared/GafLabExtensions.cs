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
//using System.Runtime.Remoting;
using System.Text;

namespace GAF
{
    /// <summary>
    /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
    /// for GAF.Lab for further details.
    /// </summary>
	[ObsoleteAttribute("This interface is obsolete.", false)]
    public interface IGafLabPopulation
    {
		/// <summary>
		/// This interface is provided to support the GAF.Lab GUI application. Please see the product information
		/// for GAF.Lab for further details.
		/// </summary>
		/// <returns>The population.</returns>
        Population CreatePopulation();
    }

    /// <summary>
    /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
    /// for GAF.Lab for further details.
    /// </summary>
	[ObsoleteAttribute("This interface is obsolete.", false)]
	public interface IGafLabResults
    {
		/// <summary>
		/// This interface is provided to support the GAF.Lab GUI application. Please see the product information
		/// for GAF.Lab for further details.
		/// </summary>
		/// <returns>The results.</returns>
		/// <param name="currentPopulation">Current population.</param>
		/// <param name="currentGeneration">Current generation.</param>
		/// <param name="evaluations">Evaluations.</param>
        string DisplayResults(Population currentPopulation, int currentGeneration, long evaluations);
    }

    /// <summary>
    /// This class is provided to support the GAF.Lab GUI application. Please see the product information
    /// for GAF.Lab for further details.
    /// </summary>
	[ObsoleteAttribute("This attribute is obsolete.", false)]
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class OperatorOptionsAttribute : Attribute
    {
        /// <summary>
        /// This class is provided to support the GAF.Lab GUI application. Please see the product information
        /// for GAF.Lab for further details.
        /// </summary>
        public bool PermutationProblem { get; set; }
        /// <summary>
        /// This class is provided to support the GAF.Lab GUI application. Please see the product information
        /// for GAF.Lab for further details.
        /// </summary>
        public OperatorOptionsAttribute()
        {
            PermutationProblem = false;
        }
    }

}

