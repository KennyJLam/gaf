using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GAF.Api
{
	public class Population : GafApiBase
	{

		private GAF.GeneticAlgorithm _ga = null;
		private int _populationSize = 0;
		private int _chromosomeLength = 0;
		private bool _evaluateInParallel = false;
		private bool _linearlyNormalised = false;
		private bool _reEvaluateAll = false;
		private Api.ParentSelectionMethod _parentSelectionMethod;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.Api.Population"/> class.
		/// 
		/// This constructor is used before the GA is created. This allows the population
		/// state to be set and changed by the consumer before the GA initial GAF.Populatiom
		/// is created.
		/// 
		/// Once the GA (and Population) has been created they will be passed to this object
		/// using the SetGenericAlgorithm() method.
		internal Population ()
		{
			//this constructor is used before the GA is created.
		}

		/// <summary>
		/// Applies a GA object (with population) to the Api.Population object
		/// if resetState is False the ga.Population will take on the state of 
		/// the Api.Population settings. If resetState is true, the Api.Populationn
		/// object will take on the state of the specified ga.Population
		/// </summary>
		/// <param name="ga">Ga.</param>
		/// <param name="resetState">If set to <c>true</c> reset state.</param>
		public void SetGeneticAlgorithm (GAF.GeneticAlgorithm ga, bool resetState)
		{
			//this constructor is used after the GA is created
			if (ga == null) {
				throw new ArgumentNullException ("ga");
			}
			if (ga.Population == null) {
				throw new NullReferenceException ("The value 'ga.Population' is null.");
			}
			if (ga.Population.PopulationSize != this.PopulationSize) {
				throw new ArgumentException ("The Population being applied does not have the same PopulationSize as that specified in the Api.Population object.");
			}
			if (ga.Population.ChromosomeLength != this.ChromosomeLength) {
				throw new ArgumentException ("The Population being applied does not have the same ChromosomeLength as that specified in the Api.Population object.");
			}

			if (resetState) {
				SetState (ga.Population);
			} else {
			
				//set the ga.Population object to the same state as the Api.Population
				ga.Population.EvaluateInParallel = this.EvaluateInParallel;
				ga.Population.LinearlyNormalised = this.LinearlyNormalised;
				ga.Population.ReEvaluateAll = this.ReEvaluateAll;
				ga.Population.ParentSelectionMethod = (GAF.ParentSelectionMethod)this.ParentSelectionMethod;

			}
			_ga = ga;
		
		}

		public int PopulationSize { 
			get {				
				return _populationSize;
			}
			private set {
				UpdateField (ref _populationSize, value, "PopulationSize");
			}
		}

		public int ChromosomeLength {
			get {
				return _chromosomeLength;
			}
			private set {
				UpdateField (ref _chromosomeLength, value, "ChromosomeLength");
			}
		}

		public ParentSelectionMethod ParentSelectionMethod {
			get { 
				
				return _parentSelectionMethod;
			}
			set { 
				if (UpdateField (ref _parentSelectionMethod, value, "ParentSelectionMethod")) {
					if (_ga != null && _ga.Population != null) {
						_ga.Population.ParentSelectionMethod = (GAF.ParentSelectionMethod)value;
					}
				}
			}
		}

		public bool EvaluateInParallel {
			get {
				return _evaluateInParallel;
			}
			set {
				if (UpdateField (ref _evaluateInParallel, value, "EvaluateInParallel")) {
					if (_ga != null && _ga.Population != null) {
						_ga.Population.EvaluateInParallel = value;
					}
				}
			}
		}

		public bool LinearlyNormalised {
			get {
				return _linearlyNormalised;
			}
			set {
				if (UpdateField (ref _linearlyNormalised, value, "LinearlyNormalised")) {
					if (_ga != null && _ga.Population != null) {
						_ga.Population.LinearlyNormalised = value;
					}
				}
			}
		}

		public bool ReEvaluateAll {
			get {
				return _reEvaluateAll;
			}
			set {
				if (UpdateField (ref _reEvaluateAll, value, "ReEvaluateAll")) {
					if (_ga != null && _ga.Population != null) {
						_ga.Population.ReEvaluateAll = value;
					}
				}
			}
		}

		#region Methods



		internal PopulationState GetState ()
		{
			var state = new PopulationState ();
			state.EvaluateInParallel = this.EvaluateInParallel;
			state.ReEvaluateAll = this.ReEvaluateAll;
			state.ParentSelectionMethod = this.ParentSelectionMethod;
			state.LinearlyNormalised = this.LinearlyNormalised;
			state.PopulationSize = this.PopulationSize;
			state.ChromosomeLength = this.ChromosomeLength;
			return state;
		}

		internal static PopulationState GetState (GAF.Population population)
		{
			var state = new PopulationState ();
			state.EvaluateInParallel = population.EvaluateInParallel;
			state.ReEvaluateAll = population.ReEvaluateAll;
			state.ParentSelectionMethod = (Api.ParentSelectionMethod)population.ParentSelectionMethod;
			state.LinearlyNormalised = population.LinearlyNormalised;
			state.PopulationSize = population.PopulationSize;
			state.ChromosomeLength = population.ChromosomeLength;
			return state;
		}

		internal void SetState (PopulationState populationState)
		{
			this.PopulationSize = populationState.PopulationSize;
			this.ChromosomeLength = populationState.ChromosomeLength;
			this.EvaluateInParallel = populationState.EvaluateInParallel;
			this.ReEvaluateAll = populationState.ReEvaluateAll;
			this.ParentSelectionMethod = populationState.ParentSelectionMethod;
			this.LinearlyNormalised = populationState.LinearlyNormalised;
		}

		internal void SetState (GAF.Population population)
		{
			//update member variables from population
			this.PopulationSize = population.PopulationSize;
			this.ChromosomeLength = population.ChromosomeLength;
			this.EvaluateInParallel = population.EvaluateInParallel;
			this.LinearlyNormalised = population.LinearlyNormalised;
			this.ReEvaluateAll = population.ReEvaluateAll;
			this.ParentSelectionMethod = (Api.ParentSelectionMethod)population.ParentSelectionMethod;			
		}

		//		internal void Update (GAF.GeneticAlgorithm ga)
		//		{
		//
		//			if (ga == null) {
		//				throw new ArgumentNullException ("population");
		//			}
		//
		//			//update population from member variables
		//			population.EvaluateInParallel = _evaluateInParallel;
		//			population.LinearlyNormalised = _linearlyNormalised;
		//			population.ReEvaluateAll = _reEvaluateAll;
		//			population.ParentSelectionMethod = (GAF.ParentSelectionMethod)_parentSelectionMethod;
		//
		//			_population = population;
		//
		//		}


		#endregion

	}
}

