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
using System.Globalization;
using System.Linq;
using System.Text;

namespace GAF
{

    /// <summary>
    /// This static class, primarily provided for demonstration and testing purposes. 
    /// </summary>
    public static class TestProblems
    {

        /// <summary>
        /// A fitness function for the Schaffer Binary F6 problem.
        /// </summary>
        /// <returns></returns>
        public static double BinaryF6(double x, double y)
        {

            var temp1 = System.Math.Sin(System.Math.Sqrt(x * x + y * y));
            var temp2 = 1 + 0.001 * (x * x + y * y);
            var result = 0.5 + (temp1 * temp1 - 0.5) / (temp2 * temp2);

            var fitness = 1 - result;

            return fitness;

        }

    }
}
