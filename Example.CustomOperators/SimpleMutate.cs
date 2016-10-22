using System;
using GAF;
using GAF.Operators;

namespace CustomOperators
{
	public class SimpleMutate : MutateBase
	{
		
		public SimpleMutate (double mutationProbabilty) : base (mutationProbabilty)
		{
		}

		protected override void MutateGene (Gene gene)
		{
			// This example mutates Binary, Real and Integer types and raises 
			// an exception if the Gene is any other type.

			switch (gene.GeneType) {
			case GeneType.Binary: {
					gene.ObjectValue = !(bool)gene.ObjectValue;
					break;
				}
			case GeneType.Real: {
					gene.ObjectValue = (double)gene.ObjectValue * -1;
					break;
				}
			case GeneType.Integer: {
					gene.ObjectValue = (int)gene.ObjectValue * -1;
					break;
				}
			default: {
					throw new OperatorException ("Genes with this GeneType cannot be mutated by this operator.");
				}
			}
		}
	}
}
