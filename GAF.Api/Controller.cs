using System;
using GAF.Operators;

namespace GAF.Api
{
	public class Controller
	{
		public Controller ()
		{

		}

		public Api.GeneticAlgorithm GeneticAlgorithm { get; private set; }
		public Api.Population Population { get; private set; }
	}
}

