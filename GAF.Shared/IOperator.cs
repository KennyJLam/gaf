using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF
{
    /// <summary>
    /// This is the interface that all reproductive operators must implement.
    /// </summary>
    /// <remarks>Each operator should ensure that any chromosomes passed from 
    /// the current population to the returned population are cloned 
    /// using the Chromosomes Clone() method. An operator shoult only NOT change
    /// the currentPopulation. It is acceptable for an operator to return a new
    /// population with a different number of chromosomes than the current population.
    /// </remarks>
    [Obsolete("This Interface has been deprecated and may not be available in future releases. Use IGeneticOperator instead.", false)]
    public interface IOperator
    {
        /// <summary>
        /// This method should be used to perform the operation. The the 'currentPopulation' variable will be in an 
		/// unknown state following the call.
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <param name="newPopulation"></param>
        /// <param name="fitnesFunctionDelegate"></param>
        /// <returns>Population</returns>
        void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate);

        /// <summary>
        /// This method should return the number of evaluations that were carried out, 
        /// i.e. the number of times the fitness function was called during the Invoke method.
        /// </summary>
        /// <returns></returns>
        int GetOperatorInvokedEvaluations();

    }
}
