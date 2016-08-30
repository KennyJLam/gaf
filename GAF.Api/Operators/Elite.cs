using System;

namespace GAF.Api.Operators
{
	public sealed class Elite : OperatorBase
	{
		private bool _enabled;
		private int _percentage;
		private string _description;

		public Elite (GAF.Operators.Elite elite) : base(elite)
		{
			//see base class
			this.Description = HelpText.EliteDescription;

			//set initial state
			this.Enabled = _operator.Enabled;
			this.Percentage = ((GAF.Operators.Elite)_operator).Percentage;

		}

		public int Percentage {
			get {
				return ((GAF.Operators.Elite)_operator).Percentage;
			}
			set {
				if (UpdateField (ref _percentage, value, "Percentage")) {
					((GAF.Operators.Elite)_operator).Percentage = value;
				}
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

