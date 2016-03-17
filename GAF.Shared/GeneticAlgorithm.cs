/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GAF.Operators;

namespace GAF
{
    /// <summary>
    /// Delegate definition for the Terminate function.
    /// </summary>
    /// <param name="population"></param>
    /// <param name="currentGeneration"></param>
    /// <param name="currentEvaluation"></param>
    /// <returns></returns>
    public delegate bool TerminateFunction(Population population, int currentGeneration, long currentEvaluation);

    /// <summary>
    /// Main Generic Algorithm controller class.
    /// </summary>
    public class GeneticAlgorithm
    {
        //private readonly object _syncLock = new object();

        /// <summary>
        /// Delegate definition for the GenerationComplete event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void InitialEvaluationCompleteHandler(object sender, GaEventArgs e);

        /// <summary>
        /// Event definition for the GenerationComplete event handler.
        /// </summary>
		public event InitialEvaluationCompleteHandler OnInitialEvaluationComplete;

		/// <summary>
		/// Delegate definition for the GenerationComplete event handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void GenerationCompleteHandler(object sender, GaEventArgs e);

		/// <summary>
		/// Event definition for the GenerationComplete event handler.
		/// </summary>
		public event GenerationCompleteHandler OnGenerationComplete;


		/// <summary>
        /// Delegate definition for the RunException event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RunExceptionHandler(object sender, GaExceptionEventArgs e);

        /// <summary>
        /// Event definition for the RunException event handler.
        /// </summary>
        public event RunExceptionHandler OnRunException;

        /// <summary>
        /// Delegate definition for the RunComplete event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RunCompleteHandler(object sender, GaEventArgs e);

        /// <summary>
        /// Event definition for the RunComplete event handler.
        /// </summary>
        public event RunCompleteHandler OnRunComplete;
        
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _task;
        private Population _population;
        //private Population _newPopulation;
        private int _currentGeneration;
        private readonly FitnessFunction _fitnessFunctionDelegate;

        private long _evaluations;

        #region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.GeneticAlgorithm"/> class.
		/// </summary>
		public GeneticAlgorithm()
		{
			this.Operators = new List<IOperator>();
		}

        /// <summary>
        /// Constuctor, requires a configured Population object.
        /// </summary>
        public GeneticAlgorithm(Population population, FitnessFunction fitnessFunctionDelegate)
        {
            _population = population;

            _fitnessFunctionDelegate = fitnessFunctionDelegate;
			this.Operators = new List<IOperator>();
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Main method for executing the GA. The GA runs until the number of evaluations 
        /// have reached the value specified in the maxEvaluations parameter.
        /// This method runs syncronously.
        /// </summary>
        public void Run(long maxEvaluations)
        {
            RunTask(maxEvaluations, null, CancellationToken.None);
        }
        /// <summary>
        /// Main method for executing the GA. The GA runs until the
        /// Terminate function returns true. 
        /// This method runs syncronously.
        /// </summary>
        public void Run(TerminateFunction terminateFunction)
        {
            RunTask(long.MaxValue, terminateFunction, CancellationToken.None);
        }
        /// <summary>
        /// Set to true to force the GA to stop running. The GA will halt in a clean fashion.
        /// This method is typically used to control the RunAsync methods.
        /// </summary>
        public void Halt()
        {
            _tokenSource.Cancel();
        }

        /// <summary>
        /// Runs the algorithn for the specified number of generations.
        /// </summary>
        /// <param name="maxEvaluations"></param>
        public void RunAsync(int maxEvaluations)
        {
            RunAsync(maxEvaluations, null);
        }

        /// <summary>
        /// Runs the algorithm the specified number of times. Each run executes until the specified delegate function returns true.
        /// </summary>
        /// <param name="terminateFunction"></param>
        public void RunAsync(TerminateFunction terminateFunction)
        {
            RunAsync(long.MaxValue, terminateFunction);
        }

        private void RunAsync(long maxEvaluations, TerminateFunction terminateFunction)
        {
            
            var token = _tokenSource.Token;
            _task = new Task(() => RunTask(maxEvaluations, terminateFunction, token), token);
            _task.Start();

            _task.ContinueWith(t =>
            {
                 /* error handling */
                var exception = t.Exception;
                IsRunning = false;

                if (this.OnRunException != null && t.Exception != null)
                {
                    var message = new StringBuilder();
                    foreach (var ex in t.Exception.InnerExceptions)
                    {
                        message.Append(ex.Message);
                        message.Append("\r\n");
                    }

                    var eventArgs = new GaExceptionEventArgs(message.ToString());
                    this.OnRunException(this, eventArgs);
                }

            }, TaskContinuationOptions.OnlyOnFaulted);

            //try
            //{
            //    _task.Wait();
            //}
            //catch (AggregateException aex)
            //{
                
            //    throw;
            //}
            
        }

        /// <summary>
        /// Main run routine of genetic algorithm.
        /// </summary>
        private void RunTask(long maxEvaluations, TerminateFunction terminateFunction, CancellationToken token)
        {
            IsRunning = true;

            //validate the population
            if (this.Population == null || this.Population.Solutions.Count == 0)
            {
                throw new NullReferenceException(
                    "Either the Population is null, or there are no solutions within the population.");
            }

            //perform the initial evaluation
            _evaluations += _population.Evaluate(_fitnessFunctionDelegate);

            //raise the Generation Complete event
			if (this.OnInitialEvaluationComplete != null)
            {
                var eventArgs = new GaEventArgs(_population, 0, _evaluations);
                this.OnGenerationComplete(this, eventArgs);
            }

			//var maxFittest = 0.0;
            //main run loop for GA
            for (int generation = 0; _evaluations < maxEvaluations; generation++)
            {

                //Note: Selection handled by the operator(s)
                _currentGeneration = generation;

				var newPopulation = RunGeneration (_population, _fitnessFunctionDelegate);

				_population.Solutions.Clear ();
				_population.Solutions.AddRange (newPopulation.Solutions);

                //raise the Generation Complete event
                if (this.OnGenerationComplete != null)
                {
                    var eventArgs = new GaEventArgs(_population, generation + 1, _evaluations);
                    this.OnGenerationComplete(this, eventArgs);
                }

                if (terminateFunction != null)
                {
                    if (terminateFunction.Invoke(_population, generation + 1, _evaluations))
                    {
                        break;
                    }
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }


            }

            IsRunning = false;


            //raise the Run Complete event
            if (this.OnRunComplete != null)
            {
                var eventArgs = new GaEventArgs(_population, _currentGeneration + 1, _evaluations);
                this.OnRunComplete(this, eventArgs);
            }

        }

		internal Population RunGeneration(Population currentPopulation, FitnessFunction fitnessFunctionDelegate)
		{

			//long evaluations = 0;

			//create a new empty population for processing 
			var tempPopulation = new Population(
				0,
				0,
				currentPopulation.ReEvaluateAll,
				currentPopulation.LinearlyNormalised,
				currentPopulation.ParentSelectionMethod);
			
			var processedPopulation = new Population(
				0,
				0,
				tempPopulation.ReEvaluateAll,
				tempPopulation.LinearlyNormalised,
				tempPopulation.ParentSelectionMethod);

			tempPopulation.Solutions.AddRange (currentPopulation.Solutions);

			//clear the current population, this keeps it simple as the solutions could
			//easily get corrupted by genetic operators as from v 2.04 on, the genes are no longer cloned
			currentPopulation.Solutions.Clear();

			//invoke the operators
			//Note: Each operator will contribute to populating the New Population. It is not untill all operators are
			//invoked the the new population is complete. For example the Elite operator with 10% will return a
			//_newPopulation object containing only the top 10% of the solutions that exist in _population.
			//Deletions handled by the operator(s)
			foreach (var op in this.Operators)
			{
				var enabled = true;
				//maxFitnessPre = tempPopulation.MaximumFitness;


				var genOp = op as IGeneticOperator; //this is the new interface
				if (genOp != null)
				{
					//using new interface so check enabled
					enabled = genOp.Enabled;
				}

				//note that each internal operator will adhere to the enabled flag itself, 
				//however the check is made here as this cannot be guaranteed with third party
				//operators.
				if (enabled)
				{
					op.Invoke(tempPopulation, ref processedPopulation, fitnessFunctionDelegate);
					_evaluations += op.GetOperatorInvokedEvaluations();
											
					tempPopulation.Solutions.Clear();
					tempPopulation.Solutions.AddRange (processedPopulation.Solutions);
					processedPopulation.Solutions.Clear();

					//TODO: Fix this. Some solutions will have been evaluated during the operator invocations
					//Some solutions will have been evaluated during the operator invocations e.g. 
					//crossover, however, they may have also been mutated. By tracking/evaluating
					//mutated we could get here only needing to evaluate those that wern't touched 
					//by the operators. The Chromosome.EvaluatedByOperator flag could help here...
					//evaluate in case it affects the next operators selection

					_evaluations += tempPopulation.Evaluate (fitnessFunctionDelegate);

				}

			}

			return tempPopulation;
		}

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets/Gets a list of the Operators that are applied.
        /// </summary>
#pragma warning disable 618
        public List<IOperator> Operators { set; get; }
#pragma warning restore 618

        /// <summary>
        /// Returns the current population.
        /// </summary>
        public Population Population
        {
            get { return _population; }
        }

        /// <summary>
        /// Gets set the option to use internally memorised solutions.
        /// </summary>
        public bool UseMemory { get; set; }

        /// <summary>
        /// Gets the running state of the GA.
        /// </summary>
        public bool IsRunning { get; set; }


        #endregion

        #region Private Methods

        #endregion

    }
    /// <summary>
    /// Event arguments used within the main GA exeption events.
    /// </summary>
    public class GaExceptionEventArgs : EventArgs
    {
        private readonly string _message;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        public GaExceptionEventArgs(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Returns the list of Exception messages.
        /// </summary>
        public string Message
        {
            get { return _message; }
        }
    }
    /// <summary>
    /// Event arguments used within the main GA events.
    /// </summary>
    public class GaEventArgs : EventArgs
    {
        private readonly int _generation;
        private readonly Population _population;
        private readonly long _evaluations;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="population"></param>
        /// <param name="generation"></param>
        /// <param name="evaluations"></param>
        public GaEventArgs(Population population, int generation, long evaluations)
        {
            _generation = generation;
            _population = population;
            _evaluations = evaluations;
        }

        /// <summary>
        /// Returns the population.
        /// </summary>
        public Population Population
        {
            get { return _population; }
        }

        /// <summary>
        /// Returns the number of the current generation.
        /// </summary>
        public int Generation
        {
            get { return _generation; }
        }

        /// <summary>
        /// Returns the number of the evaluations undertaken so far.
        /// </summary>
        public long Evaluations
        {
            get { return _evaluations; }
        }
    }
}
