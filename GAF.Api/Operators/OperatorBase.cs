using System;
using System.ComponentModel;

namespace GAF.Api.Operators
{
	public class OperatorBase : GafApiBase, IOperator
	{
		protected readonly GAF.IGeneticOperator _operator;

		public OperatorBase(IGeneticOperator geneticOperator)
		{
			if (geneticOperator == null) {
				throw new NullReferenceException ("The IGeneticOperator object is null;");
			}
			_operator = geneticOperator;
		}

		public virtual bool Enabled {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public virtual string Description {
			get {
				throw new NotImplementedException ();
			}
			protected set {
				throw new NotImplementedException ();
			}
		}
	}
}

