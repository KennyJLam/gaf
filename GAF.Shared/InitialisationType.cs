using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAF
{
    /// <summary>
    /// Internal as there is only one option, and therefore cannot be set by the consumer.
    /// Provided as an enumerator for future use.
    /// </summary>
    internal enum InitialisationType
    {
        Random,
        RandomUnique
    }
}
