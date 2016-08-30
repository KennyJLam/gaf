
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GAF.Extensions;
using System.ComponentModel;
using System.Diagnostics;

namespace GAF.Api
{
	/// <summary>
	/// This class provides statistics relating to a population.
	/// </summary>
	public class PopulationStatistics : GafApiBase
	{

		private const int BucketCount = 16;
		//NOTE: THIS MUST BE DIVISIBLE BY 4 !!!
		private const int UpperQuartileBucketCount = BucketCount / 4;

		private GAF.Population _population = null;
		private int _populationSize = 0;
		private int _chromosomeLength = 0;
		private int _duplicates = 0;
		private double _diversity = 0;
		private double _diversityF = 0;
		private double _convergence = 0;
		private double _convergenceF = 0;
		private double _averageHammingDistance = 0;
		private double _averageFitnessDistance = 0;
		private double _averageFitness = 0;
		private double _standardDeviation = 0;
		private double _minimumFitness = 0;
		private double _maximumFitness = 0;
		private int _upperQuartileCount = 0;
		private bool _enabled = true;
		private double _c9Fitness = 0;
		private int [] _diversityGraph = new int [BucketCount];
		private double _diversityGraphScale = 2;
		private string _description;
		private string _genotypicDescription;
		private PopulationHistory _populationHistory = new PopulationHistory (20);

		#region Constructor

		public PopulationStatistics ()
		{
			_description = HelpText.StatisticsDescription;
			_genotypicDescription = HelpText.GenotypicViewDescription;
		}

		#endregion

		/// <summary>
		/// Derive statistics from the specified population.
		/// The includeHammingDistance parameter causes the class to calculate the 
		/// Hamming Distance of the population. This should be used with care as setting
		/// this property to true could have performance implecations.
		/// </summary>
		/// <param name="population"></param>
		public void ProcessPopulation (GAF.Population population, int generation)
		{
			if (population == null)
				throw new ArgumentNullException (nameof (population));

			if (population.Solutions == null || !population.Solutions.Any ())
				throw new ArgumentException ("The specified population has no solutions");

			_population = population;
			_populationSize = population.PopulationSize;
			_chromosomeLength = population.ChromosomeLength;

			var geneType = _population.Solutions [0].Genes [0].GeneType;

			this.PopulationHistory.AddPopulation (population, generation);
			RaisePropertyChangedEvent ("PopulationHistory");

			//do the basics ??
			Process (geneType);
		}




		#region Public Properties

		public PopulationHistory PopulationHistory {
			private set { UpdateField (ref _populationHistory, value, "PopulationHistory"); }
			get { return _populationHistory; }
		}

		/// <summary>
		/// This property is used to 'amplify' the diversity graph values for the 
		/// purposes of better graph display. The range is 0-10 where 0 
		/// makes no change to the diversity graph values and 10 multiplies
		/// each of the buckets by 10. The buckets will never return a value greater
		/// than 100 irrespective of the Scale value. 
		/// </summary>
		public double DiversityGraphScale {
			set { UpdateField (ref _diversityGraphScale, value, "DiversityGraphScale"); }
			get { return _diversityGraphScale; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="PopulationStatistics"/> is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public bool Enabled {
			get { return _enabled; }
			set { UpdateField (ref _enabled, value, "Enabled"); }
		}

		/// <summary>
		/// Returns the average Hamming Distance. This property will only return the Hamming Distance
		/// if the includeHammingDistance parameter is set to true.
		/// </summary>
		public double AverageHammingDistance {
			get { return _averageHammingDistance; }
			private set { UpdateField (ref _averageHammingDistance, value, "AverageHammingDistance"); }
		}


		/// <summary>
		/// Gets the average fitness distance.
		/// </summary>
		/// <value>The average fitness distance.</value>
		public double AverageFitnessDistance {
			get { return _averageFitnessDistance; }
			private set { UpdateField (ref _averageFitnessDistance, value, "AverageFitnessDistance"); }
		}

		/// <summary>
		/// Gets the convergence factor. This value is derived from the Average Hamming Distance and gives an indication
		/// of convergence in the range 0-1.0. The higher the number, the greater the level of convergence.
		/// </summary>
		/// <value>The convergence factor.</value>
		public double Convergence {
			get { return _convergence; }
			private set { UpdateField (ref _convergence, value, "Convergence"); }
		}

		/// <summary>
		/// Gets the convergence factor. This value is derived from the Average Fitness Distance and gives an indication
		/// of convergence in the range 0-1.0. The higher the number, the greater the level of convergence.
		/// </summary>
		/// <value>The convergence factor.</value>
		public double ConvergenceF {
			get { return _convergenceF; }
			private set { UpdateField (ref _convergenceF, value, "ConvergenceF"); }
		}

		/// <summary>
		/// Returns an indication of diversity based on average hamming distance.
		/// </summary>
		/// <value>The diversity.</value>
		public double Diversity {
			get { return _diversity; }
			private set { UpdateField (ref _diversity, value, "Diversity"); }
		}

		/// <summary>
		/// Returns an indication of diversity based on average fitness distance.
		/// </summary>
		/// <value>The diversity.</value>
		public double DiversityF {
			get { return _diversityF; }
			private set { UpdateField (ref _diversityF, value, "DiversityF"); }
		}

		/// <summary>
		/// Returns the average fitness value of the population.
		/// </summary>
		public double AverageFitness {
			get { return _averageFitness; }
			private set { UpdateField (ref _averageFitness, value, "AverageFitness"); }
		}

		/// <summary>
		/// Reurns the standard deviation of the population, based on fitness.
		/// </summary>
		public double StandardDeviation {
			get { return _standardDeviation; }
			private set { UpdateField (ref _standardDeviation, value, "StandardDeviation"); }
		}

		/// <summary>
		/// Returns the minimum fitness value of the population.
		/// </summary>
		public double MinimumFitness {
			get { return _minimumFitness; }
			private set { UpdateField (ref _minimumFitness, value, "MinimumFitness"); }
		}

		/// <summary>
		/// Returns the maximum fitness value of the population.
		/// </summary>
		public double MaximumFitness {
			get { return _maximumFitness; }
			private set { UpdateField (ref _maximumFitness, value, "MaximumFitness"); }
		}

		/// <summary>
		/// Returns the percentage of duplicates in the population.
		/// </summary>
		public int Duplicates {
			get { return _duplicates; }
			private set { UpdateField (ref _duplicates, value, "Duplicates"); }
		}

		/// <summary>
		/// Returns an array representing a graph of the populations diversity based on fitness.
		/// </summary>
		public int [] DiversityGraph {
			get { return _diversityGraph; }
			private set { UpdateField (ref _diversityGraph, value, "DiversityGraph"); }
		}

		/// <summary>
		/// Returns the number of sollutions in the population that are within the upper quartile 
		/// based on the fitmess value.
		/// </summary>
		public int UpperQuartileCount {
			get { return _upperQuartileCount; }
			private set { UpdateField (ref _upperQuartileCount, value, "UpperQuartileCount"); }
		}

		/// <summary>
		/// Returns fitness BASED on the number of cosecutive 9s within the fractional
		/// part of the fitness value. The value returned is between 0.0-1.0 and is useful
		/// for graphing the fitness.
		/// </summary>
		/// <value>The c9 fitness.</value>
		public double C9Fitness {
			get {
				return _c9Fitness;
			}
			private set { UpdateField (ref _c9Fitness, value, "C9Fitness"); }
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description {
			get {
				return _description;
			}
			protected set {
				UpdateField (ref _description, value, "Description");
			}
		}

		/// <summary>
		/// Gets or sets the genotypic description.
		/// </summary>
		/// <value>The genotypic description.</value>
		public string GenotypicDescription {
			get {
				return _genotypicDescription;
			}
			protected set {
				UpdateField (ref _genotypicDescription, value, "GenotypicDescription");
			}
		}

		#endregion

		#region Private Functions


		private double CalculateStdDev (IEnumerable<double> values)
		{
			double ret = 0;
			var enumerable = values as IList<double> ?? values.ToList ();

			if (enumerable.Any ()) {
				//Compute the Average      
				var avg = enumerable.Average ();
				//Perform the Sum of (value-avg)_2_2      
				var sum = enumerable.Sum (d => System.Math.Pow (d - avg, 2));
				//Put it all together      
				ret = System.Math.Sqrt ((sum) / (enumerable.Count () - 1));
			}
			return ret;
		}

		private double GetUpperQuartile (int [] buckets)
		{
			const int upperQuartileBucketCount = BucketCount / 4;
			var popSize = _populationSize;

			var result = 0;

			for (var bucket = BucketCount - upperQuartileBucketCount; bucket < BucketCount; bucket++) {
				result += buckets [bucket];
			}

			return ((double)result / popSize) * 100;
		}

		private int GetConsecutiveNines (double fitness)
		{
			//crude but fast

			if (fitness < 0.9)
				return 0;
			if (fitness < 0.99)
				return 1;
			if (fitness < 0.999)
				return 2;
			if (fitness < 0.9999)
				return 3;
			if (fitness < 0.99999)
				return 4;
			if (fitness < 0.999999)
				return 5;
			if (fitness < 0.9999999)
				return 6;
			if (fitness < 0.99999999)
				return 7;
			if (fitness < 0.99999999)
				return 8;
			if (fitness < 0.999999999)
				return 9;
			return 10;

		}

		#endregion

		#region Main Loop

		private void Process (GeneType geneType)
		{
			var solutions = _population.Solutions;
			double totalFitness = 0.0;
			double maximumFitness = 0.0;
			double minimumFitness = 1.0;
			var fitnessList = new List<double> (_populationSize);
			double totalHammingDistance = 0.0;
			double totalFitnessDistance = 0.0;
			int [] diversityGraph = new int [BucketCount];


			//loop through each solution
			for (var sIndex = 0; sIndex < _populationSize; sIndex++) {

				var solution = solutions [sIndex];

				#region Storing Data for Later

				//store the values for using later on with Average and StddDev
				var currentFitness = solution.Fitness;
				totalFitness += currentFitness;
				fitnessList.Add (currentFitness);

				#endregion

				#region Max and Min

				if (currentFitness < minimumFitness)
					minimumFitness = currentFitness;
				if (currentFitness > maximumFitness)
					maximumFitness = currentFitness;

				#endregion

				#region Hamming Distance

				//TODO: Check that this is correct. 
				//We safely compare to ourselves as this will produce a 0 result.

				//compare the current solution with all others. Note that we only need
				//to compare each once hence the shortened 'innerIndex' loop
				for (var innerIndex = sIndex; innerIndex < _populationSize; innerIndex++) {

					// hamming only applies to binary chromosome
					if (geneType == GeneType.Binary) {
						//calculate hamming distance

						var string1 = solutions [sIndex].ToBinaryString ();
						var string2 = solutions [innerIndex].ToBinaryString ();

						//TODO: Look at converting the binary to an integer and using the bitwise function (See below)
						for (var index = 0; index < _chromosomeLength; index++) {
							if (string1 [index] != string2 [index]) {
								totalHammingDistance++;
							}
						}
						//our gene is actually strored as a double.
						//we also need to make sure we dont cause errors if a non binary chromosome is used;
						//e.g. Here is an example in C comparing two integers
						/*
                            def hamming2(x,y):
                                """Calculate the Hamming distance between two bit strings"""
                                assert len(x) == len(y)
                                count,z = 0,x^y
                                while z:
                                    count += 1
                                    z &= z-1 # magic!
                                return count                          
                         */

						//end of hamming distance
					}
					//fitness distance (diversity via fitness

					var fitness1 = solutions [sIndex].Fitness;
					var fitness2 = solutions [innerIndex].Fitness;

					totalFitnessDistance += System.Math.Abs (fitness1 - fitness2);
				}

				//Debug.Print ("Total Hamming Distance: {0}", new [] { totalHammingDistance.ToString() });
				//Debug.Print ("Total Fitness Distance: {0}", new [] { totalFitnessDistance.ToString () });


				#endregion

				#region Diversity Graph

				var bucket = (int)System.Math.Floor (currentFitness / (1.0 / BucketCount));

				//if fitness ends up being 1.0 we could end up with a bucket of 16 which would fail
				if (bucket == BucketCount)
					bucket--;

				diversityGraph [bucket]++;

				/*
					Each bucket will contain the number of solutions from the total number of solutions
					in the population.
				*/


				#endregion

			}

			#region Hamming Distance

			// sampleSize is given by = N(N-1)/2 and is the number of chromosome comparisons
			// totalHammingDistance refers to the number of bits that are different when comparing
			// all chromosomes with each other e.g. N(N-1)/2
			// average hammiing distance starts at approximatelly: chromosomeLength/2
			// and ends up at 0 when the GA is converged

			var sampleSize = (_populationSize * (_populationSize - 1)) / 2.0;
			AverageHammingDistance = (totalHammingDistance / sampleSize);
			AverageFitnessDistance = totalFitnessDistance / sampleSize;

			//if (AverageHammingDistance < 0)
			//	throw new ApplicationException ("Hamming Distance is negative.");
			//if (AverageFitnessDistance < 0)
			//	throw new ApplicationException ("Fitness Distance is negative.");

			#endregion

			#region Duplicates

			double percentageDuplicates = 0.0;
			var count = _population.GetDuplicates ();

			if (count > 0) {
				percentageDuplicates = ((double)count / _populationSize) * 100;
			}

			this.Duplicates = (int)System.Math.Round (percentageDuplicates);

			#endregion

			#region Upper Quartile Count

			var uqCount = 0;

			for (var bucket = BucketCount - UpperQuartileBucketCount; bucket < BucketCount; bucket++) {
				uqCount += _diversityGraph [bucket];
			}

			this.UpperQuartileCount = (int)(((double)uqCount / _populationSize) * 100);

			#endregion


			#region Scale the Distribution Graph

			for (int index = 0; index < diversityGraph.Length; index++) {
				var bucketPercentage = ((double)diversityGraph [index] / _populationSize) * 100;
				var bucketPercentageScaled = bucketPercentage * _diversityGraphScale;
				diversityGraph [index] = (int)(bucketPercentageScaled <= 100 ? bucketPercentageScaled : 100);
			}

			this.DiversityGraph = diversityGraph;

			#endregion

			#region Average Fitness

			if (!Math.AboutEqual (totalFitness, 0.0) && _populationSize != 0) {
				this.AverageFitness = totalFitness / _populationSize;
			}

			#endregion

			#region Standard Deviation

			this.StandardDeviation = CalculateStdDev (fitnessList);

			#endregion

			#region Max/Min

			this.MaximumFitness = maximumFitness;
			this.MinimumFitness = minimumFitness;

			#endregion

			this.C9Fitness = GAF.Math.ReRange (GetConsecutiveNines (maximumFitness), 0, 10, 0, 1);

			//TODO: Determine how is this derived?
			//average hamming distance is per population, and ranges from chromosomeLenth/2 
			//(good estimate of initial hamming distance, see Louis & Rawlins, Predicting Convergence Time for Genetic Algorithms)
			//and 0 (fully converged).
			//e.g. with a chromsome length of 22 the range would be 22 - 0 the following rescales to 1 - 0;)
			var diversity = (AverageHammingDistance / (_chromosomeLength / 2));
			if (diversity > 100)
				diversity = 100;

			this.Diversity = diversity;

			var diversityF = AverageFitnessDistance / 0.5;
			if (diversityF > 100)
				diversityF = 100;

			this.DiversityF = diversityF;

			if (geneType == GeneType.Binary) {
				this.Convergence = 1 - diversity;
				this.DiversityGraph = diversityGraph;
			} else {
				this.Convergence = 0;
				this.Diversity = 0;
			}

			this.ConvergenceF = 1 - diversityF;
		}

		#endregion



	}
}

