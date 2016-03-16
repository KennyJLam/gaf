using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF.Extensions
{
    /// <summary>
    /// Extension class for the IList&lt;T&gt; type.
    /// </summary>
    public static class IList
    {

        /// <summary>
        /// This method performs a simple fast Fisher Yates Shuffle.
        /// </summary>
        public static void ShuffleFast<T>(this IList<T> list)
        {
            //Random rng = new Random();
			var rng = Threading.RandomProvider.GetThreadRandom();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        /// <summary>
        /// This method performs a simple fast Fisher Yates Shuffle.
        /// </summary>
        public static void ShuffleFast<T>(this IList<T> list, Random rng)
        {

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        } 

    }
}
