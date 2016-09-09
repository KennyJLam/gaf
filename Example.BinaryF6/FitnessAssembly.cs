using System;
using System.Linq;
using System.Reflection;
using GAF;

namespace Example.BinaryF6
{
    /// <summary>
    /// This class is supplied as an example of how to exract a Fitness function from an assembly.
    /// </summary>
    public class FitnessAssembly
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Example.BinaryF6.FitnessAssembly"/> class.
        /// </summary>
        /// <param name="assemblyPath">Assembly path.</param>
        public FitnessAssembly (string assemblyPath)
        {
            var fitnessDll = Assembly.LoadFile (assemblyPath);
            var type = typeof (GAF.IFitness);
            var types = fitnessDll.DefinedTypes.Where (type.IsAssignableFrom).ToList ();

            if (types.Count == 0) {
                throw new ApplicationException (string.Format ("Fitness function not found within the specified dll [{0}].", assemblyPath));
            }

            var consumerFunctions = types [0];

            //get method inf objects based on method names as defined in IConsumerFunctions
            var fitnessMethodInfo = consumerFunctions.GetDeclaredMethod ("EvaluateFitness");
            var typeAsInstance = Activator.CreateInstance (consumerFunctions);

            FitnessFunction =
                 (FitnessFunction)Delegate.CreateDelegate (typeof (FitnessFunction), typeAsInstance, fitnessMethodInfo);

        }

        /// <summary>
        /// Gets the fitness function.
        /// </summary>
        /// <value>The fitness function.</value>
        public FitnessFunction FitnessFunction { private set; get; }

    }
}


