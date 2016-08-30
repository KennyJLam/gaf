using System;
using GAF;

namespace Example.IFitness.BinaryF6
{
	public class BinaryF6Functions : GAF.IFitness
	{
		#region IFitness implementation

		public double EvaluateFitness (Chromosome chromosome)
		{
			double fitnessValue = -1;
			if (chromosome != null) {
				//this is a range constant that is used to keep the x/y range between -100 and +100
				var rangeConst = 200 / (System.Math.Pow (2, chromosome.Count / 2) - 1);

				//get x and y from the solution
				var x1 = Convert.ToInt32 (chromosome.ToBinaryString (0, chromosome.Count / 2), 2);
				var y1 = Convert.ToInt32 (chromosome.ToBinaryString (chromosome.Count / 2, chromosome.Count / 2), 2);

				//Adjust range to -100 to +100
				var x = (x1 * rangeConst) - 100;
				var y = (y1 * rangeConst) - 100;

				//using binary F6 for fitness.
				var temp1 = System.Math.Sin (System.Math.Sqrt (x * x + y * y));
				var temp2 = 1 + 0.001 * (x * x + y * y);
				var result = 0.5 + (temp1 * temp1 - 0.5) / (temp2 * temp2);

				fitnessValue = 1 - result;
			} else {
				//chromosome is null
				throw new ArgumentNullException (nameof(chromosome), "The specified Chromosome is null.");
			}

			return fitnessValue;
		}

		#endregion

	}
}


