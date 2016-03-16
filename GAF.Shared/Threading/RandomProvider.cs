using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GAF.Threading
{
    /// <summary>
    /// Provides one Random class per thread.
    /// </summary>
    public static class RandomProvider
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> randomWrapper =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        /// <summary>
        /// Returns a thread safe System.Random class
        /// </summary>
        /// <returns></returns>
        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}
