using System;

namespace GAF.Api.Operators
{
	public sealed class SwapMutate : OperatorBase, IOperator
	{
		private bool _enabled;
		private double _probability;
		private string _description;

		public SwapMutate (GAF.Operators.SwapMutate swapMutate) : base(swapMutate)
		{
			//see base class
			this.Description = HelpText.SwapMutateDescription;

			//set initial state
			this.Enabled = _operator.Enabled;
			this.Probability = ((GAF.Operators.SwapMutate)_operator).MutationProbability;
		}

		public double Probability {
			get {
				return ((GAF.Operators.SwapMutate)_operator).MutationProbability;
			}
			set {
				if (UpdateField (ref _probability, value, "Probability")) {
					((GAF.Operators.SwapMutate)_operator).MutationProbability = value;
				}
			}
		}
		#region IOperator implementation

		public override bool Enabled {
			get {
				return _operator.Enabled;
			}
			set {
				if (UpdateField (ref _enabled, value, "Enabled")) {
					_operator.Enabled = value;
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

		#endregion
	}
}

