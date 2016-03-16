using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GAF
{
    /*
 * 1. Throw exception with out message
 * throw new CustomException() 
 * 
 * 2. Throw exception with simple message
 * throw new CustomException(message)
 * 
 * 3. Throw exception with message format and parameters
 * throw new CustomException("Exception with parameter value '{0}'", param) 
 * 
 * 4. Throw exception with simple message and inner exception
 * throw new CustomException(message, innerException) 
 * 
 * 5. Throw exception with message format and inner exception. Note that, the variable length params are always floating.
 * throw new CustomException("Exception with parameter value '{0}'", innerException, param)

 */
    /// <summary>
    /// Custom exception used to indicate an exception with a gene. See the inner exception and message for full exception details.
    /// </summary>
//    [Serializable]
    public class GeneException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GeneException()
            : base()
        {
        }

        /// <summary>
        /// Constructor accepting a message.
        /// </summary>
        /// <param name="message"></param>
        public GeneException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor accepting a formatted message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public GeneException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        /// <summary>
        /// Constructor accepting a message and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public GeneException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor accepting a formatted message and inner exception.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public GeneException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

    }
}
