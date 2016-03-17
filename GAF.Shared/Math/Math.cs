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
    /// This class is a simple math helper class.
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Rounds a number to r=the nearest even whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int RoundEven(double value)
        {
            if (value < 0)
            {
                throw new ArgumentException("A negative number was specified");
            }

            var result = System.Math.Round(value, MidpointRounding.AwayFromZero);

            if (result%2 > 0)
            {
                if (value < result)
                {
                    result--;
                }
                else
                {
                    result++;
                }
            }

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Returns a posive integer value, i.e. -10 becomes 10.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Positive(long value)
        {
            if (value < 0) value = value*-1;
            return value;
        }
		/// <summary>
		/// Returns a range constant that can be used for normalisation.
		/// </summary>
		/// <returns>The range constant.</returns>
		/// <param name="range">Range.</param>
		/// <param name="numberOfBits">Number of bits.</param>
		public static double GetRangeConstant(double range, int numberOfBits)
		{
			return range / (System.Math.Pow(2, numberOfBits) - 1);
		}
    }
}
