/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

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
			    
			var assembly = Assembly.LoadFile (assemblyPath);
			var type = typeof (IRemoteFitness);
			var types = assembly.DefinedTypes.Where (type.IsAssignableFrom).ToList ();
			//var types = fitnessDll.GetTypes();

			if (types.Count == 0) {
				throw new ApplicationException (string.Format ("An IRemoteFitness type connot be found within the specified assembly [{0}].", assemblyPath));
			}

			//get the first type available
			var fitnessClass = types [0];

			//get method inf objects based on method names as defined in IConsumerFunctions
			var fitnessMethodInfo = fitnessClass.GetDeclaredMethod ("EvaluateFitness");
			//var knownTypesMethodInfo = fitnessFunction.GetDeclaredMethod ("GetKnownTypes");

			var typeAsInstance = Activator.CreateInstance (fitnessClass);

			FitnessFunction =
				(FitnessFunction)Delegate.CreateDelegate (typeof (FitnessFunction), typeAsInstance, fitnessMethodInfo);

			KnownTypes = ((IRemoteFitness)typeAsInstance).GetKnownTypes ();

			AssemblyName = assembly.FullName.Split(',')[0];
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

