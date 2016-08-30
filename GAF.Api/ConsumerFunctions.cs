using System;
using System.Reflection;
using System.Linq;

namespace GAF.Api
{
	public class ConsumerFunctions
	{
		public ConsumerFunctions (string assemblyPath)
		{
			LoadedAssembly = Assembly.LoadFile (assemblyPath);
		
			var type = typeof(IGafLabConsumerFunctions);
			var types = LoadedAssembly.DefinedTypes.Where (type.IsAssignableFrom).ToList ();

			if (types.Count == 0) {
				throw new ApplicationException (string.Format ("Consumer functions not found within the specified dll [{0}].", assemblyPath));
			}

			//get the first type available
			var consumerFunctions = types [0];

			OperatorOptions = (OperatorOptionsAttribute)consumerFunctions.GetCustomAttribute(typeof(OperatorOptionsAttribute));

			//get method inf objects based on method names as defined in IConsumerFunctions/IGafConsumerFunctions
			var fitnessMethodInfo = consumerFunctions.GetDeclaredMethod ("EvaluateFitness");
			var terminateMethodInfo = consumerFunctions.GetDeclaredMethod ("TerminateAlgorithm");

			this.LoadedAssemblyInstance = Activator.CreateInstance (consumerFunctions);

			this.FitnessFunction =
				(FitnessFunction)Delegate.CreateDelegate (typeof(FitnessFunction), LoadedAssemblyInstance, fitnessMethodInfo);

			this.TerminateFunction =
				(TerminateFunction)Delegate.CreateDelegate (typeof(TerminateFunction), LoadedAssemblyInstance, terminateMethodInfo);

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
		public TerminateFunction TerminateFunction { private set; get; }

		/// <summary>
		/// Gets the loaded assembly.
		/// </summary>
		/// <value>The loaded assembly.</value>
		public Assembly LoadedAssembly { private set; get; }

		/// <summary>
		/// Gets the loaded assembly instance.
		/// </summary>
		/// <value>The loaded assembly instance.</value>
		public object LoadedAssemblyInstance { private set; get; }

		/// <summary>
		/// Gets or sets the operator options attribute.
		/// </summary>
		/// <value>The operator options.</value>
		public OperatorOptionsAttribute OperatorOptions { private set; get; }

		/// <summary>
		/// Creates the initial population.
		/// </summary>
		/// <value>The population.</value>
		public GAF.Population CreatePopulation() 
		{
			return ((IGafLabConsumerFunctions)LoadedAssemblyInstance).CreatePopulation ();
		}

		/// <summary>
		/// Creates the GenerationComplete message.
		/// </summary>
		/// <value>The population.</value>
		public string CreateGenerationCompleteMessage(GAF.Population currentPopulation, int currentGeneration, long evaluations) 
		{
			return ((IGafLabConsumerFunctions)LoadedAssemblyInstance).CreateGenerationCompleteMessage(currentPopulation, currentGeneration, evaluations);
		}

		/// <summary>
		/// Creates the RunComplete message.
		/// </summary>
		/// <value>The population.</value>
		public string CreateRunCompleteMessage(GAF.Population currentPopulation, int currentGeneration, long evaluations) 
		{
			return ((IGafLabConsumerFunctions)LoadedAssemblyInstance).CreateRunCompleteMessage(currentPopulation, currentGeneration, evaluations);
		}
	}

}

