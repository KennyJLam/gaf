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
