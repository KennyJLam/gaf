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
    public interface IGafLabFunctions
    {

        /// <summary>
        /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
        /// for GAF.Lab for further details.
        /// </summary>
        double EvaluateFitness(Chromosome chromosome);
        /// <summary>
        /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
        /// for GAF.Lab for further details.
        /// </summary>
        bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation);

    }

    /// <summary>
    /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
    /// for GAF.Lab for further details.
    /// </summary>
    public interface IGafLabPopulation
    {
        /// <summary>
        /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
        /// for GAF.Lab for further details.
        /// </summary>
        Population CreatePopulation();
    }

    /// <summary>
    /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
    /// for GAF.Lab for further details.
    /// </summary>
    public interface IGafLabResults
    {
        /// <summary>
        /// This interface is provided to support the GAF.Lab GUI application. Please see the product information
        /// for GAF.Lab for further details.
        /// </summary>
        string DisplayResults(Population currentPopulation, int currentGeneration, long evaluations);
    }

    /// <summary>
    /// This class is provided to support the GAF.Lab GUI application. Please see the product information
    /// for GAF.Lab for further details.
    /// </summary>
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

