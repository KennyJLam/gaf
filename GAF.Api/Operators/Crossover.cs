using System;
using System.Text;

namespace GAF.Api.Operators
{
	public sealed class Crossover : OperatorBase
	{
		private bool _allowDuplicates;
		private double _probability;
		private bool _enabled;
		private CrossoverType _crossoverType;
		private ReplacementMethod _replacementMethod;
		private string _description;

		public Crossover (GAF.Operators.Crossover crossover):base(crossover)
		{
			//see base class
			this.Description = HelpText.CrossoverDescription;

			//set initial state
			this.Enabled = _operator.Enabled;
			this.AllowDuplicates = ((GAF.Operators.Crossover)_operator).AllowDuplicates;
			this.CrossoverProbability = ((GAF.Operators.Crossover)_operator).CrossoverProbability;
			this.CrossoverType = (GAF.Api.Operators.CrossoverType)((GAF.Operators.Crossover)_operator).CrossoverType;
			this.ReplacementMethod = (GAF.Api.Operators.ReplacementMethod)((GAF.Operators.Crossover)_operator).ReplacementMethod;

		}
			
		public bool AllowDuplicates { 
			set {
				if (UpdateField (ref _allowDuplicates, value, "AllowDuplicates")) {
					((GAF.Operators.Crossover)_operator).AllowDuplicates = value;
				}
			} 
			get {
				return ((GAF.Operators.Crossover)_operator).AllowDuplicates;

			}
		}
			
		public double CrossoverProbability { 
			set {
				if (UpdateField (ref _probability, value, "Probablity")) {
					((GAF.Operators.Crossover)_operator).CrossoverProbability = value;
				}
			}
			get { 
				return ((GAF.Operators.Crossover)_operator).CrossoverProbability;
			}
		}

		public CrossoverType CrossoverType {
			set {
				if (UpdateField (ref _crossoverType, value, "CrossoverType")) {
					((GAF.Operators.Crossover)_operator).CrossoverType = (GAF.Operators.CrossoverType)value;
				}
			}
			get {
				return (GAF.Api.Operators.CrossoverType)((GAF.Operators.Crossover)_operator).CrossoverType;
			}
		}

		public ReplacementMethod ReplacementMethod {
			set {
				if (UpdateField (ref _replacementMethod, value, "ReplacementMethod")) {
					((GAF.Operators.Crossover)_operator).ReplacementMethod = (GAF.Operators.ReplacementMethod)value;
				}
			}
			get {
				return (GAF.Api.Operators.ReplacementMethod)((GAF.Operators.Crossover)_operator).ReplacementMethod;
			}
		}

		#region IOperator implementation

		public override bool Enabled { 
			set {
				if (UpdateField (ref _enabled, value, "Enabled")) {
					_operator.Enabled = value;
				}
			} 
			get {
				return _operator.Enabled;

			}
		}

		public override string Description {
			get {
				return _description;
			}
			protected set {
				UpdateField (ref _description, value, "Description"); 
			}
		}
			
		#endregion

	}
}

