using System;
using System.Reflection;
using System.Linq;

namespace GAF.Network
{
	internal class FitnessAssembly
	{
		public FitnessAssembly (string assemblyPath)
		{
			var fitnessDll = Assembly.LoadFile (assemblyPath);
			var type = typeof(IFitness);
			var types = fitnessDll.DefinedTypes.Where (type.IsAssignableFrom).ToList ();
			//var types = fitnessDll.GetTypes();

			if (types.Count == 0) {
				throw new ApplicationException (string.Format("Fitness function not found within the specified dll [{0}].", assemblyPath));
			}

			//get the first type available
			var fitnessFunction = types [0];

			//get method inf objects based on method names as defined in IConsumerFunctions
			var fitnessMethodInfo = fitnessFunction.GetDeclaredMethod ("EvaluateFitness");

			var typeAsInstance = Activator.CreateInstance (fitnessFunction);

			FitnessFunction =
				(FitnessFunction) Delegate.CreateDelegate(typeof (FitnessFunction), typeAsInstance, fitnessMethodInfo);

		}

		/// <summary>
		/// Returns the fitness function discovered within the assembly specified in the constructor.
		/// </summary>
		/// <value>The fitness function.</value>
		internal FitnessFunction FitnessFunction { private set; get; }

	}
}

