using System;
using System.Collections.Generic;

namespace GAF.Commands
{
	public class Population : CommandsBase, ICommands
	{
		

		public Population (GAF.GeneticAlgorithm ga) : base (ga)
		{
			
			//Commands.Add ("size", this.Size);
			//Commands.Add ("foo", this.Foo);
			//Commands.Add ("bar", this.Bar);
		}

		public string Help { 
			get { 
				return "population size\npopulation foo\npopulation bar";
			}
		}

		public bool Size (int populationSize)
		{
			var result = false;

			try {

				//if one parameters then return population size
				if (_ga != null && _ga.Population != null) {
							

					if (!_ga.IsRunning) {
						var pop = new GAF.Population (populationSize,
							          _ga.Population.ChromosomeLength,
							          _ga.Population.ReEvaluateAll,
							          _ga.Population.LinearlyNormalised,
							          _ga.Population.ParentSelectionMethod,
							          _ga.Population.EvaluateInParallel);

						_ga = new GAF.GeneticAlgorithm (pop, _ga.FitnessFunction);
						result = true;
					} else {
						result = false;
					}
				}
			} catch {
				result = false;
			}

			return result;
		}

		//		public string Foo(List<string> args, GeneticAlgorithm ga)
		//		{
		//			return "3.14159271";
		//		}
		//		public string Bar(List<string> args, GeneticAlgorithm ga)
		//		{
		//			return "3.14159272";
		//		}
	}
}

