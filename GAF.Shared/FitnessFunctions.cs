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
    /// These functions are obsolete, please use the similarly named functions in the 'GAF.TestProblems' class. 
    /// </summary>
    [Obsolete("These functions are obsolete, please use the similarly named functions in the 'GAF.TestProblems' class.", true)]
    public static class FitnessFunctions
    {
        /// <summary>
        /// This function is obsolete, please use the similarly named function in the 'GAF.TestProblems' class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [Obsolete("This function is obsolete, please use the similarly named function in the 'GAF.TestProblems' class.", true)]
        public static double BinaryF6(double x, double y)
        {
            return -1.0;
        }
    }
}
