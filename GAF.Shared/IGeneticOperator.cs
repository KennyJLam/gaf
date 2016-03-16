using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF
{
    /// <summary>
    /// This is the interface that should be implemented when creating custom genetic operators.
    /// </summary>
#pragma warning disable 618
    public interface IGeneticOperator : IOperator
#pragma warning restore 618
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
        bool Enabled { set; get; }
    }
}
