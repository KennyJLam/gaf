using System;

namespace GAF.Api.Operators
{
	public sealed class RandomReplace : OperatorBase, IOperator
	{
		private bool _enabled;
		private int _percentage;
		private bool _allowDuplicates;
		private string _description;

		public RandomReplace (GAF.Operators.RandomReplace randomReplace):base(randomReplace)
		{	
			//see base class
			this.Description = HelpText.RandomReplaceDescription;

			//set initial state
			this.Enabled = _operator.Enabled;
			this.Percentage = ((GAF.Operators.RandomReplace)_operator).Percentage;
			this.AllowDuplicates = ((GAF.Operators.RandomReplace)_operator).AllowDuplicates;

		}

		public int Percentage {
			get {
				return ((GAF.Operators.RandomReplace)_operator).Percentage;
			}
			set {
				if (UpdateField (ref _percentage, value, "Percentage")) {
					((GAF.Operators.RandomReplace)_operator).Percentage = value;
				}
			}
		}

		public bool AllowDuplicates { 
			set {
				if (UpdateField (ref _allowDuplicates, value, "AllowDuplicates")) {
					((GAF.Operators.RandomReplace)_operator).AllowDuplicates = value;
				}
			} 
			get {
				return ((GAF.Operators.RandomReplace)_operator).AllowDuplicates;

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

