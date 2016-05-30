using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Operators;

namespace GAF.Api
{
	public class Operators
	{
		private GAF.Operators.Elite _elite;
		private GAF.Operators.Crossover _crossover;
		private GAF.Operators.BinaryMutate _binaryMutate;
		private GAF.Operators.SwapMutate _swapMutate;
		private GAF.Operators.RandomReplace _randomReplace;
		private GAF.Operators.Copy _copy;
		private GAF.Operators.Memory _memory;

		public Operators ()
		{
			GeneticOperators = CreateDefaultOperators();
		}

		public Api.Operators.Crossover Crossover { private set; get; }
		public Api.Operators.BinaryMutate BinaryMutate { private set; get; }
		public Api.Operators.Copy Copy { private set; get; }
		public Api.Operators.Elite Elite { private set; get; }
		public Api.Operators.Memory Memory { private set; get; }
		public Api.Operators.RandomReplace RandomReplace { private set; get; }
		public Api.Operators.SwapMutate SwapMutate { private set; get; }

		public List<IGeneticOperator> GeneticOperators { private set; get; }

		/// <summary>
		/// Creates the default operators.
		/// </summary>
		private List<IGeneticOperator> CreateDefaultOperators ()
		{
			var result = new List<IGeneticOperator> ();

			//create a set of default operators
			_elite = new Elite (5);
			_elite.Enabled = true;
			result.Add (_elite);
			Elite = new GAF.Api.Operators.Elite (_elite);

			_crossover = new GAF.Operators.Crossover (0.8, true, GAF.Operators.CrossoverType.DoublePoint, ReplacementMethod.GenerationalReplacement);
			_crossover.Enabled = true;
			result.Add (_crossover);
			Crossover = new Api.Operators.Crossover (_crossover);


			_binaryMutate = new GAF.Operators.BinaryMutate (0.04, true);
			_binaryMutate.Enabled = true;
			result.Add (_binaryMutate);
			BinaryMutate = new GAF.Api.Operators.BinaryMutate (_binaryMutate);

			_swapMutate = new GAF.Operators.SwapMutate (0.04);
			_swapMutate.Enabled = false;
			result.Add (_swapMutate);
			SwapMutate = new GAF.Api.Operators.SwapMutate (_swapMutate);

			_randomReplace = new GAF.Operators.RandomReplace (10, true);
			_randomReplace.Enabled = false;
			result.Add (_randomReplace);
			RandomReplace = new GAF.Api.Operators.RandomReplace (_randomReplace);

			_copy = new GAF.Operators.Copy (5, GAF.Operators.CopyMethod.Random);
			_copy.Enabled = false;
			result.Add (_copy);
			Copy = new GAF.Api.Operators.Copy (_copy);

			_memory = new GAF.Operators.Memory (100, 10);
			_memory.Enabled = false;
			result.Add (_memory);
			Memory = new GAF.Api.Operators.Memory (_memory);

			return result;

		}
	}
}

