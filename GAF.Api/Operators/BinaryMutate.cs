using System;

namespace GAF.Api.Operators
{
	public sealed class BinaryMutate : OperatorBase
	{
		private bool _enabled;
		private bool _allowDuplicates;
		private double _mutationProbaility;
		private string _description;

		public BinaryMutate (GAF.Operators.BinaryMutate binaryMutate) : base (binaryMutate)
		{
			//see base class
			this.Description = HelpText.BinaryMutateDescription;

			//set initial state
			this.Enabled = _operator.Enabled;
			this.MutationProbability = ((GAF.Operators.BinaryMutate)_operator).MutationProbability;
		}

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

		public bool AllowDuplicates {
			set {
				if (UpdateField (ref _allowDuplicates, value, "AllowDuplicates")) {
					((GAF.Operators.BinaryMutate)_operator).AllowDuplicates = value;
				}
			}
			get {
				return ((GAF.Operators.BinaryMutate)_operator).AllowDuplicates;

			}
		}

		public double MutationProbability {
			get {
				return ((GAF.Operators.BinaryMutate)_operator).MutationProbability;
			}
			set {
				if (UpdateField (ref _mutationProbaility, value, "MutationProbability")) {
					((GAF.Operators.BinaryMutate)_operator).MutationProbability = value;
				}
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
	}
}

