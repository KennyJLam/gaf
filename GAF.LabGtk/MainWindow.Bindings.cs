using System;
using GAF.Api.Operators;
using GAF.Api;


public partial class MainWindow
{

	/// <summary>
	/// Registers the UI Bindings. Note: this is using the JN developed binder.
	/// </summary>
	/// <remarks>
	/// Note that the last parameter is an optional delegate that can be used to convert/scale the final output.
	/// 
	/// Examples of use (delegate syntax):
	/// 	delegate(object o){return o;};
	/// 
	/// Examples of use (expression lambda):
	/// 	(o) => o;
	/// 
	/// Examples of use (statement lambda):
	/// 	(o) => { return o };
	/// 
	/// Note theat in the examples above 'o' is specified as type 'object'. Therefore to change the return type
	/// form say, an int to a double, a double cast is required. The first to cast from object, the second to cast
	/// to the required double.
	/// int to double the following would be required 
	/// 	(double)((int)o)/100.0
	/// </remarks>
	public void RegisterBindings ()
	{
		Gtk.Application.Invoke (delegate {



			_bindings.RegisterBinding (resumeButton, "Sensitive", _ga, "IsPaused");
			_bindings.RegisterBinding (stepButton, "Sensitive", _ga, "IsPaused");

			_bindings.RegisterBinding (monitorTabLabel, "TooltipText", _ga.PopulationStatistics, "Description");
			_bindings.RegisterBinding (sourceCodeTabLabel, "TooltipText", _ga, "SourceCodeDescription");
			_bindings.RegisterBinding (genoTypicTabLabel, "TooltipText", _ga.PopulationStatistics, "GenotypicDescription");


			//GenotypicViewDescription

			_bindings.RegisterBinding (elapsedTimeLabel, "Text", _ga, "RunMilliseconds");
			_bindings.RegisterBinding (elapsedGenTimeLabel, "Text", _ga, "GenerationMilliseconds");
			_bindings.RegisterBinding (threadDelayHScale, "Value", _ga, "ThreadDelayMilliseconds");

			_bindings.RegisterBinding (generationsLabel, "Text", _ga, "Generations", o => ((int)o).ToString ("00"));
			_bindings.RegisterBinding (evaluationsLabel, "Text", _ga, "Evaluations", o => ((long)o).ToString ("00"));
			_bindings.RegisterBinding (fitnessLabel, "Text", _ga, "MaximumFitness", o => ((double)o).ToString ("0.0000000000"));

			_bindings.RegisterBinding (stopButton, "Sensitive", _ga, "IsRunning");

			_bindings.RegisterBinding (hammingDistanceLabel, "Text", _ga.PopulationStatistics, "AverageHammingDistance", o => ((double)o).ToString ("0.0000000000"));
			_bindings.RegisterBinding (hammingFitnessLabel, "Text", _ga.PopulationStatistics, "AverageFitnessDistance", o => ((double)o).ToString ("0.0000000000"));

			_bindings.RegisterBinding (averageFitnessLabel, "Text", _ga.PopulationStatistics, "AverageFitness", o => ((double)o).ToString ("0.0000000000"));
			_bindings.RegisterBinding (maximumFitnessLabel, "Text", _ga.PopulationStatistics, "MaximumFitness", o => ((double)o).ToString ("0.0000000000"));
			_bindings.RegisterBinding (minimumFitnessLabel, "Text", _ga.PopulationStatistics, "MinimumFitness", o => ((double)o).ToString ("0.0000000000"));
			_bindings.RegisterBinding (standardDeviationLabel, "Text", _ga.PopulationStatistics, "StandardDeviation", o => ((double)o).ToString ("0.0000000000"));

			_bindings.RegisterBinding (convergenceProgressBar, "Fraction", _ga.PopulationStatistics, "Convergence");
			_bindings.RegisterBinding (convergenceFProgressBar, "Fraction", _ga.PopulationStatistics, "ConvergenceF");
			_bindings.RegisterBinding (diversityProgressBar, "Fraction", _ga.PopulationStatistics, "Diversity");
			_bindings.RegisterBinding (diversityFProgressBar, "Fraction", _ga.PopulationStatistics, "DiversityF");
			_bindings.RegisterBinding (duplicateProgressBar, "Fraction", _ga.PopulationStatistics, "Duplicates", o => (double)((int)o) / 100.0);
			_bindings.RegisterBinding (duplicateLabel, "Text", _ga.PopulationStatistics, "Duplicates");

			//crossover
			_bindings.RegisterBinding (crossoverEnabledCheckButton, "Active", _ga.GeneticOperators.Crossover, "Enabled");
			_bindings.RegisterBinding (crossoverAllowDuplicatesCheckButton, "Active", _ga.GeneticOperators.Crossover, "AllowDuplicates");
			_bindings.RegisterBinding (crossoverProbabilityHScale, "Value", _ga.GeneticOperators.Crossover, "CrossoverProbability");

			//elite
			_bindings.RegisterBinding (elitePercentageHScale, "Value", _ga.GeneticOperators.Elite, "Percentage");
			_bindings.RegisterBinding (eliteEnabledCheckButton, "Active", _ga.GeneticOperators.Elite, "Enabled");

			//binary Mutate
			_bindings.RegisterBinding (binaryMutateProbabilityHScale, "Value", _ga.GeneticOperators.BinaryMutate, "MutationProbability");
			_bindings.RegisterBinding (binaryMutateEnabledCheckButton, "Active", _ga.GeneticOperators.BinaryMutate, "Enabled");
			_bindings.RegisterBinding (binaryMutateAllowDuplicatesCheckButton, "Active", _ga.GeneticOperators.BinaryMutate, "AllowDuplicates");

			//random replace
			_bindings.RegisterBinding (randomReplacePercentageHScale, "Value", _ga.GeneticOperators.RandomReplace, "Percentage");
			_bindings.RegisterBinding (randomReplaceEnabledCheckButton, "Active", _ga.GeneticOperators.RandomReplace, "Enabled");
			_bindings.RegisterBinding (randomReplaceAllowDuplicatesCheckButton, "Active", _ga.GeneticOperators.RandomReplace, "AllowDuplicates");

			//swap mutate
			_bindings.RegisterBinding (swapMutateProbabilityHScale, "Value", _ga.GeneticOperators.SwapMutate, "Probability");
			_bindings.RegisterBinding (swapMutateEnabledCheckButton, "Active", _ga.GeneticOperators.SwapMutate, "Enabled");

			//memory
			_bindings.RegisterBinding (memorySizeHScale, "Value", _ga.GeneticOperators.Memory, "MemorySize");
			_bindings.RegisterBinding (memoryUpdateHScale, "Value", _ga.GeneticOperators.Memory, "GenerationalUpdatePeriod");
			_bindings.RegisterBinding (memoryEnabledCheckButton, "Active", _ga.GeneticOperators.Memory, "Enabled");


			_bindings.RegisterBinding (fitnessProgressBar, "Fraction", _ga.PopulationStatistics, "C9Fitness");
			_bindings.RegisterBinding (distributionHScale, "Value", _ga.PopulationStatistics, "DiversityGraphScale", o => (double)o * 10);

			_bindings.RegisterBinding (populationFrame, "Sensitive", _ga, "HasPopulation");
			_bindings.RegisterBinding (parentSelectionFrame, "Sensitive", _ga, "HasPopulation");

			_bindings.RegisterBinding (populationSizeLabel, "Text", _ga.Population, "PopulationSize");
			_bindings.RegisterBinding (chromosomeLengthLabel, "Text", _ga.Population, "ChromosomeLength");

			_bindings.RegisterBinding (reEvaluateCheckbutton, "Active", _ga.Population, "ReEvaluateAll");
			_bindings.RegisterBinding (linearNormalisationCheckButton, "Active", _ga.Population, "LinearlyNormalised");
			_bindings.RegisterBinding (evaluateInParallelCheckButton, "Active", _ga.Population, "EvaluateInParallel");
			_bindings.RegisterBinding (statisticsCheckButton, "Active", _ga.PopulationStatistics, "Enabled");

			_bindings.RegisterBinding (crossoverTextView.Buffer, "Text", _ga.GeneticOperators.Crossover, "Description");
			_bindings.RegisterBinding (binaryMutateTextView.Buffer, "Text", _ga.GeneticOperators.BinaryMutate, "Description");
			_bindings.RegisterBinding (eliteTextView.Buffer, "Text", _ga.GeneticOperators.Elite, "Description");
			_bindings.RegisterBinding (randomReplaceTextView.Buffer, "Text", _ga.GeneticOperators.RandomReplace, "Description");
			_bindings.RegisterBinding (swapMutateTextView.Buffer, "Text", _ga.GeneticOperators.SwapMutate, "Description");
			_bindings.RegisterBinding (memoryTextView.Buffer, "Text", _ga.GeneticOperators.Memory, "Description");


			_bindings.RegisterBinding (distributionProgressBarZero, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [0] / 100);
			_bindings.RegisterBinding (distributionProgressBar1, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [1] / 100);
			_bindings.RegisterBinding (distributionProgressBar2, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [2] / 100);
			_bindings.RegisterBinding (distributionProgressBar3, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [3] / 100);
			_bindings.RegisterBinding (distributionProgressBar4, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [4] / 100);
			_bindings.RegisterBinding (distributionProgressBar5, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [5] / 100);
			_bindings.RegisterBinding (distributionProgressBar6, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [6] / 100);
			_bindings.RegisterBinding (distributionProgressBar7, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [7] / 100);
			_bindings.RegisterBinding (distributionProgressBar8, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [8] / 100);
			_bindings.RegisterBinding (distributionProgressBar9, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [9] / 100);
			_bindings.RegisterBinding (distributionProgressBar10, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [10] / 100);
			_bindings.RegisterBinding (distributionProgressBar11, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [11] / 100);
			_bindings.RegisterBinding (distributionProgressBar12, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [12] / 100);
			_bindings.RegisterBinding (distributionProgressBar13, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [13] / 100);
			_bindings.RegisterBinding (distributionProgressBar14, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [14] / 100);
			_bindings.RegisterBinding (distributionProgressBar15, "Fraction", _ga.PopulationStatistics, "DiversityGraph", o => (double)((int[])o) [15] / 100);

			_bindings.RegisterBinding (crossoverTypeSingleRadioButton, "Active", _ga.GeneticOperators.Crossover, "CrossoverType", o => (CrossoverType)o == CrossoverType.SinglePoint);
			_bindings.RegisterBinding (crossoverTypeDoubleRadioButton, "Active", _ga.GeneticOperators.Crossover, "CrossoverType", o => (CrossoverType)o == CrossoverType.DoublePoint);
			_bindings.RegisterBinding (crossoverTypeDoubleOrderedRadioButton, "Active", _ga.GeneticOperators.Crossover, "CrossoverType", o => (CrossoverType)o == CrossoverType.DoublePointOrdered);

			_bindings.RegisterBinding (crossoverReplacementGenerational, "Active", _ga.GeneticOperators.Crossover, "ReplacementMethod", o => (ReplacementMethod)o == ReplacementMethod.GenerationalReplacement);
			_bindings.RegisterBinding (crossoverReplacementDeleteLast, "Active", _ga.GeneticOperators.Crossover, "ReplacementMethod", o => (ReplacementMethod)o == ReplacementMethod.DeleteLast);


			_bindings.RegisterBinding (fitnessProportionalRadioButton, "Active", _ga.Population, "ParentSelectionMethod", o => (ParentSelectionMethod)o == ParentSelectionMethod.FitnessProportionateSelection);
			_bindings.RegisterBinding (stochasticUniversalRadioButton, "Active", _ga.Population, "ParentSelectionMethod", o => (ParentSelectionMethod)o == ParentSelectionMethod.StochasticUniversalSampling);
			_bindings.RegisterBinding (tournamentRadioButton, "Active", _ga.Population, "ParentSelectionMethod", o => (ParentSelectionMethod)o == ParentSelectionMethod.TournamentSelection);

			_bindings.RegisterBinding (sourceCodeTextView.Buffer, "Text", _ga, "SourceCode");

		});
	}
}

