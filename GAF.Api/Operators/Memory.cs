using System;

namespace GAF.Api.Operators
{
	public sealed class Memory : OperatorBase, IOperator
	{
		private bool _enabled;
		private int _updatePeriod;
		private int _memorySize;
		private string _description;

		public Memory (GAF.Operators.Memory memory) : base (memory)
		{
			//see base class
			this.Description = HelpText.MemoryDescription;

			//set initial state
			this.Enabled = _operator.Enabled;
			this.MemorySize = ((GAF.Operators.Memory)_operator).MemorySize;
			this.GenerationalUpdatePeriod = ((GAF.Operators.Memory)_operator).GenerationalUpdatePeriod;

		}

		public int MemorySize {
			get {
				return ((GAF.Operators.Memory)_operator).MemorySize;
			}
			set {
				if (UpdateField (ref _memorySize, value, "MemorySize")) {
					((GAF.Operators.Memory)_operator).MemorySize = value;
				}
			}
		}

		public int GenerationalUpdatePeriod {
			get {
				return ((GAF.Operators.Memory)_operator).GenerationalUpdatePeriod;
			}
			set {
				if (UpdateField (ref _updatePeriod, value, "GenerationalUpdatePeriod")) {
					((GAF.Operators.Memory)_operator).GenerationalUpdatePeriod = value;
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

