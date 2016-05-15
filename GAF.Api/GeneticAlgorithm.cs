using System;
using System.Collections.Generic;

namespace GAF.Commands
{
	public class GeneticAlgorithm : CommandsBase, ICommands
	{
		public GeneticAlgorithm(GAF.GeneticAlgorithm ga) : base(ga)
		{
			//Commands.Add ("run", this.Run);
			//Commands.Add ("halt", this.Halt);
			//Commands.Add ("status", this.Status);
		}


		public string Help { 
			get{ 
				return "ga run\nga halt\nga status";
			}
		}

//		public string Run(List<string> args, GeneticAlgorithm ga)
//		{
//			return "3.1415927";
//		}
//		public string Halt(List<string> args, GeneticAlgorithm ga)
//		{
//			return "3.14159271";
//		}
//		public string Status(List<string> args, GeneticAlgorithm ga)
//		{
//			return "3.14159272";
//		}




	}
}

