using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF
{
    /// <summary>
    /// Parent selection method.
    /// </summary>
    public enum ParentSelectionMethod
    {
        /// <summary>
        /// Fitness Proportional (Roulette Wheel).
        /// </summary>
        FitnessProportionateSelection,
        /// <summary>
        /// Stochastic Universal Sampling.
        /// </summary>
        StochasticUniversalSampling,
        /// <summary>
        /// Tournament Selection.
        /// </summary>
        TournamentSelection

    }
}
