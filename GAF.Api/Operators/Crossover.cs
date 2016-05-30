using System;

namespace GAF.Api
{
	public class Crossover
	{
		private readonly GAF.Operators.Crossover _crossover;

		public Crossover (GAF.Operators.Crossover crossover)
		{
			if (crossover == null) {
				throw new NullReferenceException ("The Crossover object is null;");
			}
			_crossover = crossover;

		}

		public bool Enabled { 
			set {
				
				_crossover.Enabled = value;
			} 
			get {
				return _crossover.Enabled;

			}
		}

		public string Description {
			get {
				return HelpText.CrossoverDescription;
			}
		}
	}
}

