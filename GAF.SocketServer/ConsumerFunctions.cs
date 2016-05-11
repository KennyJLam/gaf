using System;
using System.Reflection;
using System.Linq;

namespace GAF.ConsoleTest
{
	public class ConsumerFunctions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.ConsoleTest.ConsumerFunctions"/> class based
		/// on the specified assembly. the assembly should contain a class that implements the IConsumerFunctions
		/// interface.
		/// </summary>
		/// <param name="assemblyPath">Assembly path.</param>
		public ConsumerFunctions (string assemblyPath)
		{

			var fitnessDll = Assembly.LoadFile (assemblyPath);
			var type = typeof(IConsumerFunctions);
			var types = fitnessDll.DefinedTypes.Where (type.IsAssignableFrom).ToList ();
			//var types = fitnessDll.GetTypes();

			if (types.Count == 0) {
				throw new ApplicationException (string.Format("Consumer functions not found within the specified dll [{0}].", assemblyPath));
			}

			//get the first type available
			var consumerFunctions = types [0];

			//get method inf objects based on method names as defined in IConsumerFunctions
			var fitnessMethodInfo = consumerFunctions.GetDeclaredMethod ("EvaluateFitness");
			var terminateMethodInfo = consumerFunctions.GetDeclaredMethod ("TerminateAlgorithm");

			var typeAsInstance = Activator.CreateInstance (consumerFunctions);

			FitnessFunction =
				(FitnessFunction) Delegate.CreateDelegate(typeof (FitnessFunction), typeAsInstance, fitnessMethodInfo);

			TerminateFunction =
				(TerminateFunction) Delegate.CreateDelegate(typeof (TerminateFunction), typeAsInstance, terminateMethodInfo);
			

		}
		/// <summary>
		/// Returns the fitness function discovered within the assembly specified in the constructor.
		/// </summary>
		/// <value>The fitness function.</value>
		public FitnessFunction FitnessFunction { private set; get; }

		/// <summary>
		/// Returns the terminate function discovered within the assembly specified in the constructor.
		/// </summary>
		/// <value>The terminate function.</value>
		public TerminateFunction TerminateFunction { private set; get;}

	}
}

