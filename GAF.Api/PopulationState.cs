using System;

namespace GAF.Api
{
	public class PopulationState
	{
		public PopulationState ()
		{
		}

		public int ChromosomeLength { get; set; }

		public int PopulationSize { get; set; }

		public ParentSelectionMethod ParentSelectionMethod { set; get; }

		public bool EvaluateInParallel { set; get; }

		public bool LinearlyNormalised { set; get; }

		public bool ReEvaluateAll { set; get; }
	}
}

