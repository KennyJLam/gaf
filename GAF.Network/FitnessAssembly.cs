using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace GAF.Network
{
	public class FitnessAssembly
	{
		public FitnessAssembly (string assemblyPath)
		{
			if (string.IsNullOrWhiteSpace (assemblyPath)) {
				throw new ArgumentException ("The specified path null or empty.", nameof (assemblyPath));
			}

			if (!File.Exists (assemblyPath))
			{
				throw new FileNotFoundException (string.Format ("Cannot find Assembly '{0}'", assemblyPath));
			}
			    
			var fitnessDll = Assembly.LoadFile (assemblyPath);
			var type = typeof (IRemoteFitness);
			var types = fitnessDll.DefinedTypes.Where (type.IsAssignableFrom).ToList ();
			//var types = fitnessDll.GetTypes();

			if (types.Count == 0) {
				throw new ApplicationException (string.Format ("An IRemoteFitness type connot be found within the specified dll [{0}].", assemblyPath));
			}

			//get the first type available
			var fitnessFunction = types [0];

			//get method inf objects based on method names as defined in IConsumerFunctions
			var fitnessMethodInfo = fitnessFunction.GetDeclaredMethod ("EvaluateFitness");
			//var knownTypesMethodInfo = fitnessFunction.GetDeclaredMethod ("GetKnownTypes");

			var typeAsInstance = Activator.CreateInstance (fitnessFunction);

			FitnessFunction =
				(FitnessFunction)Delegate.CreateDelegate (typeof (FitnessFunction), typeAsInstance, fitnessMethodInfo);

			KnownTypes = ((IRemoteFitness)typeAsInstance).GetKnownTypes ();

			AssemblyName = fitnessDll.FullName.Split(',')[0];
		}

		/// <summary>
		/// Returns the fitness function discovered within the assembly specified in the constructor.
		/// </summary>
		/// <value>The fitness function.</value>
		public FitnessFunction FitnessFunction { private set; get; }

		/// <summary>
		/// Gets the known types.
		/// </summary>
		/// <value>The known types.</value>
		public List<Type> KnownTypes { private set; get; }

		/// <summary>
		/// Gets the full name of the assembly.
		/// </summary>
		/// <value>The name of the assembly.</value>
		public string AssemblyName { private set; get; }
	}
}

