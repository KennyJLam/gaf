using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GAF.Threading;
using GAF.Extensions;

namespace GAF.Operators
{

	/// <summary>
	/// This operator will expand the new population to the same size 
	/// as the current population using the Crossover genetic operation.
	/// Properties of the class allow for a single or double point crossover
	/// and either 'Generational' or 'Delete Last' replacement mechanism.
	/// </summary>
	public class Crossover : IGeneticOperator
	{
		private readonly object _syncLock = new object ();

		/// <summary>
		/// Delegage definition for the CrossoverComplete event handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        public delegate void CrossoverCompleteHandler (object sender, CrossoverEventArgs e);

		/// <summary>
		/// Event definition for the CrossoverComplete event handler.
		/// </summary>
		public event CrossoverCompleteHandler OnCrossoverComplete;

		private double _crossoverProbabilityS = 1.0;
		private FitnessFunction _fitnessFunctionDelegate;
		private int _evaluations;
		private bool _allowDuplicatesS;
		private CrossoverType _crossoverTypeS;
		private ReplacementMethod _replacementMethodS;
		private Population _currentPopulation;
		private Population _newPopulation;
		private int _currentPopulationSize;
		private int _numberOfChildrenToGenerate;

		/// <summary>
		/// Constructor.
		/// </summary>
		internal Crossover ()
			: this (1.0)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="crossOverProbability"></param>
		public Crossover (double crossOverProbability)
			: this (crossOverProbability, true)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="crossOverProbability"></param>
		/// <param name="allowDuplicates"></param>
		public Crossover (double crossOverProbability, bool allowDuplicates)
			: this (crossOverProbability, allowDuplicates, CrossoverType.SinglePoint)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="crossOverProbability"></param>
		/// <param name="allowDuplicates"></param>
		/// <param name="crossoverType"></param>
		public Crossover (double crossOverProbability, bool allowDuplicates, CrossoverType crossoverType)
			: this (crossOverProbability, allowDuplicates, crossoverType, Operators.ReplacementMethod.GenerationalReplacement)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="crossOverProbability"></param>
		/// <param name="allowDuplicates"></param>
		/// <param name="crossoverType"></param>
		/// <param name="replacementMethod"></param>
		public Crossover (double crossOverProbability, bool allowDuplicates, CrossoverType crossoverType, ReplacementMethod replacementMethod)
		{
			this.CrossoverProbability = crossOverProbability;
			this.AllowDuplicates = allowDuplicates;
			this.ReplacementMethod = replacementMethod;
			this.CrossoverType = crossoverType;

			Enabled = true;
		}

		/// <summary>
		/// Enabled property. Diabling this operator will cause the population to 'pass through' unaltered.
		/// </summary>
		public bool Enabled { set; get; }

		/// <summary>
		/// This is the method that invokes the operator. This should not normally be called explicitly.
		/// </summary>
		/// <param name="currentPopulation"></param>
		/// <param name="newPopulation"></param>
		/// <param name="fitnessFunctionDelegate"></param>
		public void Invoke (Population currentPopulation, ref Population newPopulation, FitnessFunction fitnessFunctionDelegate)
		{


			//Debug.WriteLine(string.Format("Crossover: {0}", currentPopulation.ParentSelectionMethod));

			//if the new population is null, create an empty population
			if (newPopulation == null)
				newPopulation = new Population (0, 0,
					currentPopulation.ReEvaluateAll,
					currentPopulation.LinearlyNormalised,
					currentPopulation.ParentSelectionMethod);

			if (!Enabled)
				return;



			//if the elite operator exists before this one, the population will already contain the elites
			//from the current genetartion.
//            if (_currentPopulationSize % 2 != 0 && _newPopulationSize % 2 != 0)
//            {
//                throw new ArgumentException("Population sizes must be an even number.");
//            }

			_fitnessFunctionDelegate = fitnessFunctionDelegate;
			_currentPopulation = currentPopulation;
			_newPopulation = newPopulation;

			//need to store this as we cannot handle a change until once this generation has started
			_replacementMethodS = ReplacementMethod;

			this.Process ();

			//TODO: Test this, we shouldn't need it.
			newPopulation = _newPopulation;
		}

		/// <summary>
		/// Returns the number of evaluations that were undertaken as part of the operators Invocation.
		/// For example the 'Steady State' reproduction method, compares new childrem with those already
		/// in the population and therefore performs the analysis as part of the operators invocation.
		/// </summary>
		/// <returns></returns>
		public int GetOperatorInvokedEvaluations ()
		{
			return _evaluations;
		}

		/// <summary>
		/// Main process loop for performing a crossover on a population.
		/// </summary>
		protected void Process ()
		{

			int maxLoop = 100;
			int eliteCount = 0;

			//reset the number of evaluations
			_evaluations = 0;
			
			//now that we know how many children we are to be creating, in the case of 'Delete Last'
			//we copy the current generation across and add new children where they are greater than
			//the worst solution.
			if (_replacementMethodS == ReplacementMethod.DeleteLast) {
				//copy everything accross including the elites
				_newPopulation.Solutions.Clear ();
				_newPopulation.Solutions.AddRange (_currentPopulation.Solutions);
			} else {
				//just copy the elites, this will take all elites

				//TODO: Sort out what we do if we overfill the population with elites
				var elites = _currentPopulation.GetElites ();
				eliteCount = elites.Count ();
				if (elites != null && eliteCount > 0) {
					_newPopulation.Solutions.AddRange (elites);
				}
			}
			_currentPopulationSize = _currentPopulation.Solutions.Count;
			//_newPopulationSize = _newPopulation.Solutions.Count;

			_numberOfChildrenToGenerate =
				_currentPopulationSize - eliteCount;

			while (_numberOfChildrenToGenerate > 0) {
				//emergency exit
				maxLoop--;
				if (maxLoop <= 0) {
					Debug.WriteLine (
						"Warning: Unable to create a child that is better than those in the current population");
					break;
				}

				//these will hold the children
				Chromosome c1 = null;
				Chromosome c2 = null;

				//select some parents
				var parents = _currentPopulation.SelectParents ();
				var p1 = parents [0];
				var p2 = parents [1];

				//crossover
				var crossoverData = CreateCrossoverData (p1.Genes.Count, CrossoverType);
				var crossoverResult = PerformCrossover (p1, p2, CrossoverProbability, CrossoverType, crossoverData, out c1, out c2);

				//pass the children out to derived classes 
				//(e.g. CrossoverMutate class uses this to perform mutation)
				if (OnCrossoverComplete != null) {
					var eventArgs = new CrossoverEventArgs (crossoverResult);
					OnCrossoverComplete (this, eventArgs);
				}
                
				if (AddChild (c1)) {
					_numberOfChildrenToGenerate--;
				}

				//see if we can add the secomd
				if (_numberOfChildrenToGenerate > 0) {
					if (AddChild (c2)) {
						_numberOfChildrenToGenerate--;
					}

				}

			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="crossoverProbability"></param>
		/// <param name="crossoverType"></param>
		/// <param name = "crossoverData"></param>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		#region Private Methods

		internal CrossoverData PerformCrossover (Chromosome p1, Chromosome p2, double crossoverProbability, CrossoverType crossoverType, CrossoverData crossoverData, out Chromosome c1, out Chromosome c2)
		{
			var result = new CrossoverData ();
			var chromosomeLength = p1.Genes.Count;

			if (chromosomeLength != p2.Genes.Count) {
				throw new ArgumentException ("Parent chromosomes are not the same length.");
			}

			if (crossoverData == null) {
				throw new ArgumentException ("The CrossoverData parameter is null.");
			}

			List<Gene> cg1 = new List<Gene> ();
			List<Gene> cg2 = new List<Gene> ();

			//check probability by generating a random number between zero and one and if 
			//this number is less than or equal to the given crossover probability 
			//then crossover takes place."
			var rd = RandomProvider.GetThreadRandom ().NextDouble ();
			if (rd <= crossoverProbability) {
				switch (crossoverType) {
				case CrossoverType.SinglePoint:
					{
						if (crossoverData.Points == null || crossoverData.Points.Count < 1) {
							throw new ArgumentException (
								"The CrossoverData.Points property is either null or is missing the required crossover points.");
						}

						var crossoverPoint1 = crossoverData.Points [0]; 
						result.Points.Add (crossoverPoint1);
                            
						//create the two children							
						cg1.AddRangeCloned (p1.Genes.Take (crossoverPoint1).ToList ());
						//cg1.AddRange(p2.Genes.Skip(crossoverPoint1).Take(chromosomeLength - crossoverPoint1));
						cg1.AddRangeCloned (p2.Genes.Skip (crossoverPoint1).Take (chromosomeLength - crossoverPoint1));

						cg2.AddRangeCloned (p2.Genes.Take (crossoverPoint1).ToList ());
						cg2.AddRangeCloned (p1.Genes.Skip (crossoverPoint1).Take (chromosomeLength - crossoverPoint1));

						break;
					}
				case CrossoverType.DoublePoint:
					{
						if (crossoverData.Points == null || crossoverData.Points.Count < 2) {
							throw new ArgumentException (
								"The CrossoverData.Points property is either null or is missing the required crossover points.");
						}

						var firstPoint = crossoverData.Points [0];
						var secondPoint = crossoverData.Points [1];

						result.Points.Add (firstPoint);
						result.Points.Add (secondPoint);

						//first child
						//first part of Parent 1
						cg1.AddRangeCloned (p1.Genes.Take (firstPoint).ToList ());
						//middle part pf Parent 2
						cg1.AddRangeCloned (p2.Genes.Skip (firstPoint).Take (secondPoint - firstPoint));
						//last part of Parent 1
						cg1.AddRangeCloned (p1.Genes.Skip (secondPoint).Take (chromosomeLength - secondPoint));

						//second child
						//first part of Parent 2
						cg2.AddRangeCloned(p2.Genes.Take (firstPoint).ToList ());
						//middle part pf Parent 1
						cg2.AddRangeCloned (p1.Genes.Skip (firstPoint).Take (secondPoint - firstPoint));
						//last part of Parent 2
						cg2.AddRangeCloned (p2.Genes.Skip (secondPoint).Take (chromosomeLength - secondPoint));

						break;
					}
				case CrossoverType.DoublePointOrdered:
					{
						if (crossoverData.Points == null || crossoverData.Points.Count < 2) {
							throw new ArgumentException (
								"The CrossoverData.Points property is either null or is missing the required crossover points.");
						}

						//this is like double point except that the values are all taken from one parent
						//first the centre section of the parent selection is taken to the child
						//the remaining values of the same parent are passed to the child in the order in
						//which they appear in the second parent. If the second parent does not include the value
						//an exception is thrown.

						//these can bring back the same number, this is ok as the values will be both inclusive
						//so if crossoverPoint1 and crossoverPoint2 are the same, one gene will form the center section.
						var firstPoint = crossoverData.Points [0];
						var secondPoint = crossoverData.Points [1];

						result.Points.Add (firstPoint);
						result.Points.Add (secondPoint);

						//pass the middle part of Parent 1 to child 1
						//cg1 = new List<Gene> ();
						cg1.AddRangeCloned (p1.Genes.Skip (firstPoint).Take (secondPoint - firstPoint)); //+1 make this exclusive e.g. 4-6 include 3 genes. 
                            
						//pass the middle part of Parent 2 to child 2
						//cg2 = new List<Gene> ();
						cg2.AddRangeCloned (p2.Genes.Skip (firstPoint).Take (secondPoint - firstPoint));

						//run through the P2 adding to C1 those that exist in P1 but not yet in C1
						//add them in the order found in P2. This has to be done by value as the first parent
						//is used but the order id determined by the second parent. Can't use Guid as the second 
						//parent has a different set of genes (guids)s)
						foreach (var gene in p2.Genes) {
							//if we have the value in P1 and it is not already in C1, then add it.
							if (p1.Genes.Any (g => g.ObjectValue.Equals (gene.ObjectValue)) &&
							                             !cg1.Any (g => g.ObjectValue.Equals (gene.ObjectValue))) {
								cg1.AddCloned (gene);
							}
						}
						//run through the P1 adding to C2 those that exist in P2 but not yet in C2
						//add them in the order found in P1. This has to be done by value as the first parent
						//is used but the order id determined by the second parent. Can't use Guid as the second 
						//parent has a different set of genes (guids)
						foreach (var gene in p1.Genes) {
							//if we have the value in P1 and it is not already in C1, then add it.
							if (p2.Genes.Any (g => g.ObjectValue.Equals (gene.ObjectValue)) &&
							                             !cg2.Any (g => g.ObjectValue.Equals (gene.ObjectValue))) {
								cg2.AddCloned (gene);
							}
						}

						//if at this point we do not have a complete child, raise an exception
						if (cg1.Count != p1.Count || cg2.Count != p2.Count) {
							throw new CrossoverTypeIncompatibleException ("The parent Chromosomes were not suitable for Ordered Crossover as they do not contain the same set of values. Consider using a different crossover type, or ensure all solutions are build with the same set of values.");
						}

						break;
					}
				}
			} else {
				//crossover probaility dictates that these pass through to the next generation untouched (except for an ID change.
				//get the existing parent genes and treat them as the new children 
				cg1.AddRangeCloned(p1.Genes);
				cg2.AddRangeCloned(p2.Genes);

			}

			if (cg1.Count != chromosomeLength || cg1.Count != chromosomeLength) {
				throw new ChromosomeCorruptException ("Chromosome is corrupt!");
			}
			c1 = new Chromosome (cg1);
			c2 = new Chromosome (cg2);

			return result;
		}


		internal CrossoverData CreateCrossoverData (int chromosomeLength, CrossoverType crossoverType)
		{
			var result = new CrossoverData ();

			//this is like double point except that the values are all taken from one parent
			//first the centre section of the parent selection is taken to the child
			//the remaining values of the same parent are passed to the child in the order in
			//which they appear in the second parent. If the second parent does not include the value
			//an exception is thrown.

			//these can bring back the same number, this is ok as the values will be both inclusive
			//so if crossoverPoint1 and crossoverPoint2 are the same, one gene will form the center section.
			switch (crossoverType) {
			case CrossoverType.SinglePoint:
                    //0 is invalid, range for single point is 1 to [length]
				result.Points.Add (RandomProvider.GetThreadRandom ().Next (1, chromosomeLength));
				break;

			case CrossoverType.DoublePoint:
			case CrossoverType.DoublePointOrdered:
                    //0 is invalid, range for double needs to leave room for right segment and is 1 to [length]

				int point1;
				int point2;
				do {
					point2 = RandomProvider.GetThreadRandom ().Next (1, chromosomeLength);
					point1 = RandomProvider.GetThreadRandom ().Next (1, chromosomeLength);
				} while (point2 == point1);

				result.Points.Add (System.Math.Min (point2, point1));
				result.Points.Add (System.Math.Max (point2, point1));

				break;
			}

			return result;
		}

		/// <summary>
		/// Adds a child to the new population depending upon the criteria set in relation to replacement
		/// method and duplicate handling. The method updates the evaluation count and returns true if a 
		/// child was added to the new population.
		/// </summary>
		/// <param name="child"></param>
		/// <returns></returns>
		private bool AddChild (Chromosome child)
		{
			var result = false;

			if (_replacementMethodS == ReplacementMethod.DeleteLast) {
				child.Evaluate (_fitnessFunctionDelegate);
				_evaluations++;

				if (child.Genes != null && child.Fitness > _currentPopulation.MinimumFitness) {
					//add the child if there is still space
					if (AllowDuplicates || !_newPopulation.SolutionExists (child)) {
						//add the new child and remove the last
						_newPopulation.Solutions.Add (child);
						if (_newPopulation.Solutions.Count > _currentPopulationSize) {
							_newPopulation.Solutions.Sort ();
							_newPopulation.Solutions.RemoveAt (_currentPopulationSize - 1);
							result = true;
						} else {
							//we return true whether we actually added or not what we are effectively
							//doing here is adding the original child from the current solution
							result = true;
						}
					}
				}
			} else {
				//we need to cater for the user switching from delete last to Generational Replacement
				//in this scenrio we will have a full population but with still some children to generate
				if (_newPopulation.Solutions.Count + _numberOfChildrenToGenerate > _currentPopulationSize) {
					//assume all done for this generation
					_numberOfChildrenToGenerate = 0;
					return false;
				}
					
				if (child.Genes != null) {
					//add the child if there is still space
					if (this.AllowDuplicates || !_newPopulation.SolutionExists (child)) {
						_newPopulation.Solutions.Add (child);
						result = true;
					}
				}
			}
			return result;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Sets/Gets whether duplicates are allowed in the population. 
		/// The setting and getting of this property is thread safe.
		/// </summary>
		public bool AllowDuplicates {
			get {
				lock (_syncLock) {
					return _allowDuplicatesS;
				}
			}
			set {
				lock (_syncLock) {
					_allowDuplicatesS = value;
				}
			}
		}

		/// <summary>
		/// Sets/Gets the type of crossover operation. 
		/// The setting and getting of this property is thread safe.
		/// </summary>
		public CrossoverType CrossoverType {
			set {
				lock (_syncLock) {
					_crossoverTypeS = value;
				}
			}
			get {
				lock (_syncLock) {
					return _crossoverTypeS;
				}
			}
		}

		/// <summary>
		/// Sets/Gets the method used for the deletion of chromosomes from the population.
		/// The setting and getting of this property is thread safe.
		/// </summary>
		public ReplacementMethod ReplacementMethod {
			set {
				lock (_syncLock) {
					_replacementMethodS = value;
				}
			}
			get {
				lock (_syncLock) {
					return _replacementMethodS;
				}
			}
		}

		/// <summary>
		/// Sets/gets the current crossover probability. 
		/// The setting and getting of this property is thread safe.
		/// </summary>
		public double CrossoverProbability {
			get {
				lock (_syncLock) {
					return _crossoverProbabilityS;
				}
			}
			set {
				lock (_syncLock) {
					_crossoverProbabilityS = value;
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// Data object used to pass data via the Crossover event arguments.
	/// </summary>
	public class CrossoverData
	{
		readonly List<int> _points = new List<int> ();

		/// <summary>
		/// Returns a list of crossover points. For single point crossover 
		/// one point would be present, 
		/// for double point crossover methods two would be present.
		/// </summary>
		public List<int> Points {
			get { return _points; } 
		}
	}

	/// <summary>
	/// Event arguments for the Crossover events.
	/// </summary>
	public class CrossoverEventArgs : EventArgs
	{
		private readonly CrossoverData _crossoverResult;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="crossoverResult"></param>
		public CrossoverEventArgs (CrossoverData crossoverResult)
		{
			_crossoverResult = crossoverResult;
		}

		/// <summary>
		/// Returns the crossover result.
		/// </summary>
		public CrossoverData CrossoverData {
			get { return _crossoverResult; }
		}
	}
}

