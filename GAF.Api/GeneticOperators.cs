using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Operators;
using GAF.Api.Operators;
using System.ComponentModel;
using GAF.Extensions;

namespace GAF.Api
{
	public class GeneticOperators: GafApiBase, INotifyPropertyChanged
	{
		private List<IGeneticOperator> _operators = new List<IGeneticOperator> ();
		private Api.Operators.Crossover _crossover;
		private Api.Operators.BinaryMutate _binaryMutate;
		private Api.Operators.Elite _elite;
		private Api.Operators.RandomReplace _randomReplace;
		private Api.Operators.SwapMutate _swapMutate;
		private Api.Operators.Memory _memory;

		public GeneticOperators ()
		{
			CreateDefaultOperators ();
		}

		/// <summary>
		/// Gets the crossover operator.
		/// </summary>
		/// <value>The crossover.</value>
		public Api.Operators.Crossover Crossover { 
			private set { 
				UpdateField (ref _crossover, value, "Crossovet");
			} 
			get { return _crossover; }
		}

		/// <summary>
		/// Gets the binary mutate operator.
		/// </summary>
		/// <value>The binary mutate.</value>
		public Api.Operators.BinaryMutate BinaryMutate {
			private set { 
				UpdateField (ref _binaryMutate, value, "BinaryMutate");
			} 
			get { return _binaryMutate; }
		}

		/// <summary>
		/// Gets the elite operator.
		/// </summary>
		/// <value>The elite.</value>
		public Api.Operators.Elite Elite { 
			private set { 
				UpdateField (ref _elite, value, "Elite");
			} 
			get { return _elite; }
		}

		/// <summary>
		/// Gets the memory operator.
		/// </summary>
		/// <value>The memory.</value>
		public Api.Operators.Memory Memory { 
			private set { 
				UpdateField (ref _memory, value, "Memory");
			} 
			get { return _memory; }
		}

		/// <summary>
		/// Gets the random replace operator.
		/// </summary>
		/// <value>The random replace.</value>
		public Api.Operators.RandomReplace RandomReplace { 
			private set { 
				UpdateField (ref _randomReplace, value, "RandomReplace");
			} 
			get { return _randomReplace; }
		}

		/// <summary>
		/// Gets the swap mutate operator.
		/// </summary>
		/// <value>The swap mutate.</value>
		public Api.Operators.SwapMutate SwapMutate { 
			private set { 
				UpdateField (ref _swapMutate, value, "SwapMutate");
			} 
			get { return _swapMutate; }
		}

		/// <summary>
		/// Creates the default operators.
		/// </summary>
		private void CreateDefaultOperators ()
		{
			//Note: In this method we are dealing with both the GAF.Operators operators AND
			//the Api.Operators namespace. The former is the actual operator as used in the GAF, 
			//the latter is the API interface presented to clients (GUIs etc.).
			//The CreateOperatorApi helper function is used to create these combinations.

			//create a set of default operators
			IGeneticOperator elite = new GAF.Operators.Elite (5);
			Elite = CreateOperatorApi<GAF.Api.Operators.Elite> (elite, true);

			IGeneticOperator crossover = new GAF.Operators.Crossover (0.8, true, GAF.Operators.CrossoverType.DoublePoint, GAF.Operators.ReplacementMethod.GenerationalReplacement);
			Crossover = CreateOperatorApi<GAF.Api.Operators.Crossover> (crossover, true);

			IGeneticOperator binaryMutate = new GAF.Operators.BinaryMutate (0.02);
			BinaryMutate = CreateOperatorApi<GAF.Api.Operators.BinaryMutate> (binaryMutate, true);

			IGeneticOperator swapMutate = new GAF.Operators.SwapMutate (0.04);
			SwapMutate = CreateOperatorApi<GAF.Api.Operators.SwapMutate> (swapMutate, false);

			IGeneticOperator randomReplace = new GAF.Operators.RandomReplace (10, true);
			RandomReplace = CreateOperatorApi<GAF.Api.Operators.RandomReplace> (randomReplace, false);

			IGeneticOperator memory = new GAF.Operators.Memory (100, 10);
			Memory = CreateOperatorApi<GAF.Api.Operators.Memory> (memory, false);

		}

		/// <summary>
		/// Sets the operators for permutation problem.
		/// </summary>
		/// <returns>The operators for permutation problem.</returns>
		internal void SetOperatorsForPermutationProblem ()
		{
			Crossover.CrossoverType = Api.Operators.CrossoverType.DoublePointOrdered;
			BinaryMutate.Enabled = false;
			SwapMutate.Enabled = true;
			RandomReplace.Enabled = false;
			Memory.Enabled = false;

			// Elite is OK.

		}

		/// <summary>
		/// Helper function to creat the API and GAF operator combinations.
		/// </summary>
		/// <returns>The operator API.</returns>
		/// <param name="gafOperator">Gaf operator.</param>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private T CreateOperatorApi<T> (IGeneticOperator gafOperator, bool enabled)
		{	
			gafOperator.Enabled = enabled;
			this.GafOperators.Add (gafOperator);

			//var result = new T (gafOperator); //This cannot be used, hence the following line
			var result = (T)Activator.CreateInstance (typeof(T), gafOperator);

			((IOperator)result).OnException += (object sender, ExceptionEventArgs e) => RaiseExceptionEvent (e.Method, e.Message);
			((IOperator)result).OnLogging += (object sender, LoggingEventArgs e) => RaiseLoggingEvent (e.Message, e.IsWarning);
			((IOperator)result).PropertyChanged += (object sender, PropertyChangedEventArgs e) => RaisePropertyChangedEvent(e.PropertyName);

			return result;
		}

		/// <summary>
		/// Gets the gaf operators.
		/// </summary>
		/// <value>The gaf operators.</value>
		public List<IGeneticOperator> GafOperators {
			get { 
				return _operators; 
			} 
			private set {
				_operators = value;
				UpdateField (ref _operators, value, "GafOperators");
			}
		}
	}
}

