using System;
using GAF.Network;
using System.Collections.Generic;
using System.Linq;
using GAF.Extensions;
using GAF.Operators;
using GAF;
using Example.IRemoteFitness;
using System.Net;

namespace Example.DistributedEvaluation
{
	public class Program
	{

		private const int RunCount = 1;
		//private const int populationSize = 100;
		private const int populationSize = 100;

		private static void Main (string [] args)
		{
			//get our cities 
			var cities = CreateCities ().ToList ();

			//Each city is an object the chromosome is a special case as it needs 
			//to contain each city only once. Therefore, our chromosome will contain 
			//all the cities with no duplicates

			//we can create an empty population as we will be creating the 
			//initial solutions manually.
			//FIXME: Why is ReEvaluateAll required.
			var population = new Population (true, false);

			//create the initial solutions (chromosomes)
			for (var p = 0; p < populationSize; p++) {

				var chromosome = new Chromosome ();
				foreach (var city in cities) {
					chromosome.Genes.Add (new Gene (city));
				}

				chromosome.Genes.ShuffleFast ();
				population.Solutions.Add (chromosome);
			}

			//create the elite operator 
			var elite = new Elite (5);

			//create crossover operator 
			var crossover = new Crossover (0.85) { CrossoverType = CrossoverType.DoublePointOrdered };

			//create the SwapMutate operator 
			var mutate = new SwapMutate (0.02);

			//note that for network fitness evaluation we simply pass null instead of a fitness
			//function.
			var ga = new GeneticAlgorithm (population, null);

			//subscribe to the generation and run complete events 
			ga.OnGenerationComplete += ga_OnGenerationComplete;
			ga.OnRunComplete += ga_OnRunComplete;

			//add the operators 
			ga.Operators.Add (elite);
			ga.Operators.Add (crossover);
			ga.Operators.Add (mutate);

			/****************************************************************************************
			 * Up until now the GA is configured as if it were a non-distributed example except,
			 * the fitness function is not specified (see note above)
			 * 
			 * The NetworkWrapper (below) adds the networking and  the Consul class adds service 
			 * discovery functionality.
			 * 
			 ***************************************************************************************/

			//we can point the service discovery client at any node running a consul service, typically
			//this would be the localhost. An explicit IP/Port is stated here for clarity, see the 
			//constructor overloads for more details.
			IServiceDiscovery serviceDiscovery = new GAF.ServiceDiscovery.Consul.Client ("192.168.1.90", 8500);

			/****************************************************************************************
			 * The StaticServices class is a IServiceDiscovery implementation that can be used to access
			 * the specified endpoints. Use this is no specific discovery service is available.
			 * 
			 *    var endpoints = new List<IPEndPoint> ();
			 * 
			 *    endpoints.Add (NetworkWrapper.CreateEndpoint ("192.168.1.91:11000"));
			 *    endpoints.Add (NetworkWrapper.CreateEndpoint ("192.168.1.92:11000"));
			 *    endpoints.Add (NetworkWrapper.CreateEndpoint ("192.168.1.93:11000"));
			 *    endpoints.Add (NetworkWrapper.CreateEndpoint ("192.168.1.94:11000"));

			 *  IServiceDiscovery serviceDiscovery = new GAF.ServiceDiscovery.ServiceEndpoints (endpoints);
			 * 
			 ***************************************************************************************/

			var networkWrapper = new NetworkWrapper (ga, serviceDiscovery, "Example.IRemoteFitness.dll", false);

			//locally declared terminate function
			networkWrapper.GeneticAlgorithm.Run (TerminateAlgorithm);

		}

		private static bool TerminateAlgorithm (Population population, int currentGeneration, long currentEvaluation)
		{
			return currentGeneration >= 350;
		}

		private static IEnumerable<City> CreateCities ()
		{
			var cities = new List<City> ();
			cities.Add (new City ("Birmingham", 52.486125, -1.890507));
			cities.Add (new City ("Bristol", 51.460852, -2.588139));
			cities.Add (new City ("London", 51.512161, -0.116215));
			cities.Add (new City ("Leeds", 53.803895, -1.549931));
			cities.Add (new City ("Manchester", 53.478239, -2.258549));
			cities.Add (new City ("Liverpool", 53.409532, -3.000126));
			cities.Add (new City ("Hull", 53.751959, -0.335941));
			cities.Add (new City ("Newcastle", 54.980766, -1.615849));
			cities.Add (new City ("Carlisle", 54.892406, -2.923222));
			cities.Add (new City ("Edinburgh", 55.958426, -3.186893));
			cities.Add (new City ("Glasgow", 55.862982, -4.263554));
			cities.Add (new City ("Cardiff", 51.488224, -3.186893));
			cities.Add (new City ("Swansea", 51.624837, -3.94495));
			cities.Add (new City ("Exeter", 50.726024, -3.543949));
			cities.Add (new City ("Falmouth", 50.152266, -5.065556));
			cities.Add (new City ("Canterbury", 51.289406, 1.075802));
			return cities;
		}

		private static void ga_OnRunComplete (object sender, GaEventArgs e)
		{
			var fittest = e.Population.GetTop (1) [0];
			foreach (var gene in fittest.Genes) {
				Console.WriteLine (((City)gene.ObjectValue).Name);
			}
		}

		private static void ga_OnGenerationComplete (object sender, GaEventArgs e)
		{
			var fittest = e.Population.GetTop (1) [0];

			var distanceToTravel = TravellingSalesman.CalculateDistance (fittest);
			Console.WriteLine (String.Format ("Generation: {0}, Evaluations: {1}, Fitness: {2}, Distance: {3}",
				e.Generation,
				e.Evaluations,
				fittest.Fitness,
				distanceToTravel)
			);
		}


	}
}
