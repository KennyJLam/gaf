using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF.Operators
{
    /// <summary>
    /// Enum representing the method to be used for the Copy operator.
    /// </summary>
    public enum CopyMethod
    {
        /// <summary>
        /// A random selection of solutions are copied.
        /// </summary>
        Random,
        /// <summary>
        /// The fittest solutions are copied.
        /// </summary>
        Fittest
    }
}
