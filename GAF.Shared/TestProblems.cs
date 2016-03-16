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
