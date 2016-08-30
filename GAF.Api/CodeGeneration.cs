using System;
using System.Text;
using GAF;
using GAF.Operators;

namespace GAF.Api
{
	public class CodeGeneration
	{
		private Api.GeneticAlgorithm _apiGa;

		public CodeGeneration (Api.GeneticAlgorithm apiGa)
		{
			_apiGa = apiGa;
		}

		public string GeneticAlgorithmCode ()
		{
			var sourceCode = new StringBuilder ();

			if (_apiGa == null) {
				return string.Empty;
			}

			FunctionHeaderCode (ref sourceCode);

			if(_apiGa.HasPopulation)
				PopulationCode (ref sourceCode, _apiGa.Population);

			EliteCode (ref sourceCode, _apiGa.GeneticOperators.Elite);
			CrossoverCode (ref sourceCode, _apiGa.GeneticOperators.Crossover);
			BinaryMutateCode (ref sourceCode, _apiGa.GeneticOperators.BinaryMutate);
			RandomReplaceCode (ref sourceCode, _apiGa.GeneticOperators.RandomReplace);
			MemoryCode (ref sourceCode, _apiGa.GeneticOperators.Memory);
			SwapMutateCode (ref sourceCode, _apiGa.GeneticOperators.SwapMutate);


			FunctionFooterCode (ref sourceCode, _apiGa);

			return sourceCode.ToString ();
		}

		private static void FunctionHeaderCode (ref StringBuilder sourceCode)
		{
			sourceCode.Append("private void ExampleFunction()\r\n{\r\n");
		}

		private void FunctionFooterCode (ref StringBuilder sourceCode, Api.GeneticAlgorithm apiGa)
		{
			sourceCode.Append ("  var ga = new GeneticAlgorithm(population, EvaluateFitness);\r\n\r\n");

			if (apiGa.GeneticOperators.Elite.Enabled) {
				sourceCode.Append ("  ga.Operators.Add(elite);\r\n");
			}
			if (apiGa.GeneticOperators.Crossover.Enabled) {
				sourceCode.Append ("  ga.Operators.Add(crossover);\r\n");
			}
			if (apiGa.GeneticOperators.BinaryMutate.Enabled) {
				sourceCode.Append ("  ga.Operators.Add(binaryMutate);\r\n");
			}
			if (apiGa.GeneticOperators.RandomReplace.Enabled) {
				sourceCode.Append ("  ga.Operators.Add(randomReplace);\r\n");
			}
			if (apiGa.GeneticOperators.SwapMutate.Enabled) {
				sourceCode.Append ("  ga.Operators.Add(swapMutate);\r\n");
			}

			sourceCode.Append ("\r\n");
			sourceCode.Append ("  ga.Run(TerminateAlgorithm);\r\n\r\n");
			sourceCode.Append ("}\r\n");

		}

		private static void PopulationCode (ref StringBuilder sourceCode, Api.Population population)
		{

			if (population != null) {
				sourceCode.Append (
					string.Format (
						"  var population = new Population(populationSize: {0},\r\n    chromosomeLength: {1},\r\n    reEvaluateAll: {2},\r\n    useLinearlyNormalisedFitness: {3},\r\n    selectionMethod: ParentSelectionMethod.{4},\r\n    evaluateInParallel: {5});\r\n\r\n",
						population.PopulationSize,
						population.ChromosomeLength,
						population.ReEvaluateAll.ToString ().ToLower (),
						population.LinearlyNormalised.ToString ().ToLower (),
						population.ParentSelectionMethod,
						population.EvaluateInParallel.ToString ().ToLower ()));
			}

		}

		private static void EliteCode (ref StringBuilder sourceCode, Api.Operators.Elite geneticOperator)
		{
			if (geneticOperator.Enabled) {
				sourceCode.AppendFormat("  var elite = new Elite(elitismPercentage: {0});\r\n\r\n", geneticOperator.Percentage);
			} 
		}

		private static void CrossoverCode (ref StringBuilder sourceCode, Api.Operators.Crossover geneticOperator)
		{
			if (geneticOperator.Enabled) {
				sourceCode.Append (string.Format ("  var crossover = new Crossover(crossOverProbability: {0})\r\n", geneticOperator.CrossoverProbability));
				sourceCode.Append ("  {\r\n");
				sourceCode.Append (string.Format ("    AllowDuplicates = {0},\r\n", geneticOperator.AllowDuplicates.ToString ().ToLower ()));
				sourceCode.Append (string.Format ("    CrossoverType = CrossoverType.{0},\r\n", geneticOperator.CrossoverType));
				sourceCode.Append (string.Format ("    ReplacementMethod = ReplacementMethod.{0}\r\n", geneticOperator.ReplacementMethod));    
				sourceCode.Append ("  };\r\n\r\n"); 
			} 
		}

		private static void BinaryMutateCode (ref StringBuilder sourceCode, Api.Operators.BinaryMutate geneticOperator)
		{
			if (geneticOperator.Enabled) {
				sourceCode.Append (string.Format ("  var binaryMutate = new BinaryMutate(mutationProbability: {0});\r\n\r\n", geneticOperator.MutationProbability));
			}
		}

		private static void RandomReplaceCode (ref StringBuilder sourceCode, Api.Operators.RandomReplace geneticOperator)
		{
			if (geneticOperator.Enabled) {
				sourceCode.Append (string.Format ("  var randomReplace = new RandomReplace(percentageToReplace: {0})\r\n",
					geneticOperator.Percentage));
				sourceCode.Append ("  {\r\n");
				sourceCode.Append (string.Format ("    AllowDuplicates = {0}\r\n", geneticOperator.AllowDuplicates.ToString ().ToLower ()));
				sourceCode.Append ("  };\r\n\r\n");
			}
		}

		private static void MemoryCode (ref StringBuilder sourceCode, Api.Operators.Memory geneticOperator)
		{
			if (geneticOperator.Enabled) {
				sourceCode.Append (string.Format ("  var memory = new Memory(memorySize: {0}, generationalUpdatePeriod: {1});\r\n\r\n",
					geneticOperator.MemorySize,
					geneticOperator.GenerationalUpdatePeriod));
			}
		}


		private static void SwapMutateCode (ref StringBuilder sourceCode, Api.Operators.SwapMutate geneticOperator)
		{
			if (geneticOperator.Enabled) {
				sourceCode.Append (string.Format ("  var swapMutate = new SwapMutate(mutationProbability: {0});\r\n\r\n",
					geneticOperator.Probability));
			}
		}
	}
}

