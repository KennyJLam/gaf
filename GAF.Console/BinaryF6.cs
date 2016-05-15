using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF.Operators;

namespace GAF.ConsoleTest
{
    internal class BinaryF6
    {
        private int _runCount;
        private const int PopulationSize = 100;
        private const int ChromosomeLength = 44;
        private const int MaxEvaluationCount = 80000;
        private Results _results;
        private int _currentRun;
        private string _resultFilePath;
        private int _generations;

		public BinaryF6()
		{
			_resultFilePath = string.Empty;
			_runCount = 1;
		}

        public BinaryF6(string resultFilePath, int runCount)
        {
            _resultFilePath = resultFilePath;
            _runCount = runCount;
        }

        public string ResultFilePath { get { return _resultFilePath; } }
        public int RunCount { get { return _runCount; } }

		public void Test_BinaryF6CrossoverMutateElitismParallel(bool evaluateInParallel)
		{
			_results = new Results();

			const double crossoverProbability = 0.65;
			const double mutationProbability = 0.08;
			const int elitismPercentage = 5;

			string summaryText = String.Empty;

			for (int run = 1; run <= RunCount; run++)
			{
				_currentRun = run;

				//Create a Population of random chromosomes of length 44
				var population = new Population(PopulationSize, ChromosomeLength, true, false);
				population.EvaluateInParallel = evaluateInParallel;
				if (population.EvaluateInParallel)
					Console.WriteLine ("Parallel Evaluations");
				else
					Console.WriteLine ("Serial Evaluations");

				//create a couple of operators
				var elite = new Elite(elitismPercentage);
				var crossover = new Crossover(crossoverProbability, true)
				{
					CrossoverType = CrossoverType.SinglePoint
				};
				var mutation = new BinaryMutate(mutationProbability, true);

				summaryText =
					string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
						crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
						mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
						elitismPercentage.ToString(CultureInfo.InvariantCulture),
						PopulationSize.ToString(CultureInfo.InvariantCulture),
						MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

				Console.WriteLine(summaryText);

				var ga = new GeneticAlgorithm(population, CalculateFitness);

				ga.OnGenerationComplete += ga_OnGenerationComplete;
				ga.OnRunComplete += ga_OnRunComplete;

				ga.Operators.Add(elite);
				ga.Operators.Add(crossover);
				ga.Operators.Add(mutation);

				ga.Run(Terminate);

			}
			string filename = string.Format(@"{0}{1}_Test_Ca.csv", ResultFilePath, DateTime.Now.Ticks);
			_results.SaveResults(filename, summaryText);

		}


        public void Test_A_BinaryF6CrossoverMutate()
        {
            _results = new Results();
            
            const double crossoverProbability = 0.65;
            const double mutationProbability = 0.08;

            string summaryText = String.Empty;

            for (var run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(populationSize: PopulationSize, chromosomeLength: ChromosomeLength, reEvaluateAll: false, useLinearlyNormalisedFitness: false);

                //create a couple of operators
                var crossover = new Crossover(crossOverProbability: crossoverProbability)
                    {
                        CrossoverType = CrossoverType.SinglePoint
                    };
                var mutation = new BinaryMutate(mutationProbability: mutationProbability, allowDuplicates: true);


                summaryText =
                    string.Format(
                        "Crossover Probability: {0}; Mutation Probability: {1}; Population Count: {2}; Maximum Evaluations: {3}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);

                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }

            string filename = string.Format(@"{0}{1}_Test_A.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_B_BinaryF6CrossoverMutateLinearNormalisation()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            const double mutationProbability = 0.08;

            var summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, true);

                //create a couple of operators
                var crossover = new Crossover(crossoverProbability) {CrossoverType = CrossoverType.SinglePoint};
                var mutation = new BinaryMutate(mutationProbability, true);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Population Count: {2}; Max Evaluations: {3}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);

                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_B.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Ca_BinaryF6CrossoverMutateElitism()
        {
            _results = new Results();

			const double crossoverProbability = 0.65;
			const double mutationProbability = 0.08;
			const int elitismPercentage = 5;

			//create a Population of 100 random chromosomes of length 44

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
				var population = new Population(PopulationSize, ChromosomeLength, true, false);

                //create a couple of operators
                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, true)
                {
                    CrossoverType = CrossoverType.SinglePoint
                };
                var mutation = new BinaryMutate(mutationProbability, true);

                summaryText =
                    string.Format(
                        "Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_Ca.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Cb_BinaryF6CrossoverMutateLinearNormalisationElitism()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.08;
            const int elitismPercentage = 5;

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, true);

                //create a couple of operators
                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, true)
                {
                    CrossoverType = CrossoverType.SinglePoint
                };
                var mutation = new BinaryMutate(mutationProbability, true);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_Cb.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Da_BinaryF6CrossoverMutateLinearNormalisationElitismNoDuplicates()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.008;
            const int elitismPercentage = 5;
            //const int copyPercentage = 5;

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(populationSize: PopulationSize, chromosomeLength: ChromosomeLength, reEvaluateAll: false, useLinearlyNormalisedFitness: true);

                //create a couple of operators
                var elite = new Elite(elitismPercentage: elitismPercentage);
                //var copy = new Copy(copyPercentage);
                var crossover = new Crossover(crossOverProbability: crossoverProbability, allowDuplicates: false)
                {
                    CrossoverType = CrossoverType.SinglePoint
                };
                var mutation = new BinaryMutate(mutationProbability: mutationProbability, allowDuplicates: false);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                //ga.Operators.Add(copy);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);


            }
            string filename = string.Format(@"{0}{1}_Test_Da.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Db_BinaryF6CrossoverMutateElitismNoDuplicates()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.008;
            const int elitismPercentage = 5;

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, false);

                //create a couple of operators
                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, false)
                {
                    CrossoverType = CrossoverType.SinglePoint
                };
                var mutation = new BinaryMutate(mutationProbability, false);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);


            }
            string filename = string.Format(@"{0}{1}_Test_Db.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Ea_BinaryF6CrossoverMutateLinearNormalisationSteadyState()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.008;
            const int elitismPercentage = 98;
            //this means with a population of 100 that only two chomosomes are replaced

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(populationSize: PopulationSize, chromosomeLength: ChromosomeLength, reEvaluateAll: false, useLinearlyNormalisedFitness: true);

                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, true)
                {
                    CrossoverType = CrossoverType.SinglePoint,
                    ReplacementMethod = ReplacementMethod.DeleteLast
                };
                var mutation = new BinaryMutate(mutationProbability, true);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);


            }
            string filename = string.Format(@"{0}{1}_Test_E.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Eb_BinaryF6CrossoverMutateLinearSteadyState()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.008;
            const int elitismPercentage = 98;
            //this means with a population of 100 that only two chomosomes are replaced

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, false);

                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, true)
                {
                    CrossoverType = CrossoverType.SinglePoint,
                    ReplacementMethod = ReplacementMethod.DeleteLast
                };
                var mutation = new BinaryMutate(mutationProbability, true);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);


            }
            string filename = string.Format(@"{0}{1}_Test_Eb.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Fa_BinaryF6CrossoverMutateLinearNormalisationSteadyStateNoDuplicates()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.008;
            const int elitismPercentage = 98;
            //this means with a population of 100 that only two chomosomes are replaced

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(populationSize: PopulationSize, chromosomeLength: ChromosomeLength, reEvaluateAll: false, useLinearlyNormalisedFitness: true);

                var elite = new Elite(elitismPercentage: elitismPercentage);
                var crossover = new Crossover(crossoverProbability, false)
                {
                    CrossoverType = CrossoverType.SinglePoint,
                    ReplacementMethod = ReplacementMethod.DeleteLast
                };
                var mutation = new BinaryMutate(mutationProbability, false);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_Fa.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Fb_BinaryF6CrossoverMutateSteadyStateNoDuplicates()
        {
            _results = new Results();
            const double crossoverProbability = 0.65;
            var mutationProbability = 0.008;
            const int elitismPercentage = 98;
            //this means with a population of 100 that only two chomosomes are replaced

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, false);

                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, false)
                {
                    CrossoverType = CrossoverType.SinglePoint,
                    ReplacementMethod = ReplacementMethod.DeleteLast
                };
                var mutation = new BinaryMutate(mutationProbability, false);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_Fb.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Ga_BinaryF6CrossoverMutateLinearNormalisationSteadyStateNoDupliactes()
        {
            _results = new Results();
            const double crossoverProbability = 0.8;
            var mutationProbability = 0.04;
            const int elitismPercentage = 98;
            //this means with a population of 100 that only two chomosomes are replaced

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, true);

                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, false)
                {
                    CrossoverType = CrossoverType.SinglePoint,
                    ReplacementMethod = ReplacementMethod.DeleteLast
                };
                var mutation = new BinaryMutate(mutationProbability, false);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_Ga.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }

        public void Test_Gb_BinaryF6CrossoverMutateSteadyStateNoDupliactes()
        {
            _results = new Results();
            const double crossoverProbability = 0.8;
            var mutationProbability = 0.04;
            const int elitismPercentage = 98;
            //this means with a population of 100 that only two chomosomes are replaced

            string summaryText = String.Empty;

            for (int run = 1; run <= RunCount; run++)
            {
                _currentRun = run;

                //Create a Population of random chromosomes of length 44
                var population = new Population(PopulationSize, ChromosomeLength, false, false);

                var elite = new Elite(elitismPercentage);
                var crossover = new Crossover(crossoverProbability, false)
                {
                    CrossoverType = CrossoverType.SinglePoint,
                    ReplacementMethod = ReplacementMethod.DeleteLast
                };
                var mutation = new BinaryMutate(mutationProbability, false);

                summaryText =
                    string.Format(
						"Crossover Probability: {0}; Mutation Probability: {1}; Elitism: {2}% Population Count: {3}; Max Evaluations: {4}",
                        crossover.CrossoverProbability.ToString(CultureInfo.InvariantCulture),
                        mutation.MutationProbability.ToString(CultureInfo.InvariantCulture),
                        elitismPercentage.ToString(CultureInfo.InvariantCulture),
                        PopulationSize.ToString(CultureInfo.InvariantCulture),
                        MaxEvaluationCount.ToString(CultureInfo.InvariantCulture));

                Console.WriteLine(summaryText);
                
                var ga = new GeneticAlgorithm(population, CalculateFitness);

                ga.OnGenerationComplete += ga_OnGenerationComplete;
                ga.OnRunComplete += ga_OnRunComplete;

                ga.Operators.Add(elite);
                ga.Operators.Add(crossover);
                ga.Operators.Add(mutation);

                ga.Run(Terminate);

            }
            string filename = string.Format(@"{0}{1}_Test_Gb.csv", ResultFilePath, DateTime.Now.Ticks);
            _results.SaveResults(filename, summaryText);

        }


        private void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            Console.WriteLine(string.Format("Run {0} complete.", _currentRun));
            _currentRun++;
        }

        private void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
			//var stats = new PopulationStatistics (e.Population, true);
			//Console.WriteLine ("UpQ: {0}, Avg:{1}, Ham:{2}, Max:{3}", stats.UpperQuartileCount, stats.AverageFitness, stats.AverageHammingDistance, stats.MaximumFitness);

			//get the best chromosome
            var chromosome = e.Population.GetTop(1)[0];
            double rangeConst = 200/(System.Math.Pow(2, chromosome.Count/2) - 1);

			var f = e.Population.MaximumFitness;
            //get x and y from the solution
            int x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count/2), 2);
            int y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count/2, chromosome.Count/2), 2);

            //Adjust range to -100 to +100
            double x = (x1*rangeConst) - 100;
            double y = (y1*rangeConst) - 100;
            //Debug.WriteLine(string.Format("Generation {0} is complete. Maximum Fitness = {1}.", e.Generation,
            //e.Population.MaximumFitness));

			Console.WriteLine("Generation: {0} Evaluation: {1} x:{2} y:{3} Max Fitness:{4}", 
				e.Generation.ToString(CultureInfo.InvariantCulture),
				e.Evaluations.ToString(CultureInfo.InvariantCulture),
				x, 
				y, 
				e.Population.MaximumFitness);
            //Console.WriteLine("x:{0} y:{1} Fitness:{2}", 0, 0, TestProblems.BinaryF6(0,0));

//            Console.WriteLine(string.Format("Evaluation: {0}, Fitness: {1}",
//                                          e.Evaluations.ToString(CultureInfo.InvariantCulture),
//                                          result.ToString(CultureInfo.InvariantCulture)));

            _results.Add(new Result(_currentRun, e.Generation, e.Evaluations, f));

            //string duplicate;
            _generations++;

        }

		public static double CalculateFitness(Chromosome chromosome) 
		{
			
			double fitnessValue = -1;
			if (chromosome != null)
			{
				//this is a range constant that is used to keep the x/y range between -100 and +100
				var rangeConst = 200 / (System.Math.Pow(2, chromosome.Count / 2) - 1);

				//get x and y from the solution
				var x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count / 2), 2);
				var y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count / 2, chromosome.Count / 2), 2);

				//Adjust range to -100 to +100
				var x = (x1 * rangeConst) - 100;
				var y = (y1 * rangeConst) - 100;

				//using binary F6 for fitness.
				var temp1 = System.Math.Sin(System.Math.Sqrt(x * x + y * y));
				var temp2 = 1 + 0.001 * (x * x + y * y);
				var result = 0.5 + (temp1 * temp1 - 0.5) / (temp2 * temp2);

				fitnessValue = 1 - result;
			}
			else
			{
				//chromosome is null
				throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
			}

			return fitnessValue;
		}

        /// <summary>
        /// This is our delegated fitness function. It can contain any code that is required for
        ///  evaluation however it must accept a Chomosome and return a double precision number. 
        /// The returned value should be proportional to the fitness. I.e. the fitter the 
        /// solution, the higer the value returned.
        /// </summary>
        private double CalculateFitness_old(Chromosome chromosome)
        {
			
            //const double rangeConst = 0.00004768372718899898;
            double fitnessValue = -1;
            if (chromosome != null)
            {
                double rangeConst = 200 / (System.Math.Pow(2, chromosome.Count / 2) - 1);

                //for this test we are using a binary chomosome of 44 bits
                if (chromosome.Count == ChromosomeLength)
                {
                    //get x and y from the solution
                    int x1 = Convert.ToInt32(chromosome.ToBinaryString(0, chromosome.Count / 2), 2);
                    int y1 = Convert.ToInt32(chromosome.ToBinaryString(chromosome.Count / 2, chromosome.Count / 2), 2);

                    //Adjust range to -100 to +100
                    double x = (x1 * rangeConst) - 100;
                    double y = (y1 * rangeConst) - 100;

                    //using binary F6 for fitness.
                    fitnessValue = TestProblems.BinaryF6(x, y);

                }
                else
                {
                    throw new ApplicationException("The Chromosome length is incorrect.");
                }
            }
            else
            {
                //chromosome is null
                throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
            }

            //Debug.WriteLine(string.Format("Evaluation: {0} - Fitness: {1}", _evaluations.Count, fitnessValue.ToString()));

            return fitnessValue;

        }

        private bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
			//return true; //test for one Generation.
            return currentEvaluation >= MaxEvaluationCount;
        }

    }
}
