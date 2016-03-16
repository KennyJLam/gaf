using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF.Operators
{
    /// <summary>
    /// Enum that defines the type of Crossover to be implemented.
    /// </summary>
    public enum CrossoverType
    {
        /// <summary>
        /// A single random point is selected for each parent chromosome and one part is swapped between them.
        /// </summary>
        SinglePoint = 1,
        /// <summary>
        /// Two points are selected to determine a centre section in each parent, this is swapped between them.
        /// </summary>
        DoublePoint = 2,
        /// <summary>
        /// A single parent is used to create a child however the order of a second parent determines how the 
        /// chromosome is arranged. This method only works with cromosomes that have a unique set of genes (by Value).
        /// For this to be useable with custom object based genes, the Equals method should be overriden in the gene definition to return a value. 
        /// </summary>
        DoublePointOrdered = 3
    }
}
