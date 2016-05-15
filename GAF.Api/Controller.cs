using System;
using GAF.Operators;

namespace GAF
{
	public class Api
	{
		public Api ()
		{
			//create a default ga
			var p = new Population (100, 44);
			var e = new Elite (10);
			var c = new Crossover (0.8);
			var m = new BinaryMutate (0.04);

			var ga = new GeneticAlgorithm (p, null);

			ga.Operators.Add (e);
			ga.Operators.Add (c);
			ga.Operators.Add (m);


			//populate the command properties
			this.GeneticAlgorithm = new Commands.GeneticAlgorithm(ga);
			this.Population = new Commands.Population (ga);
		}

		public Commands.GeneticAlgorithm GeneticAlgorithm { get; private set; }
		public Commands.Population Population { get; private set; }
	}
}

