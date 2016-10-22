using System;
namespace GAF.Operators
{
	public abstract class OperatorBase : IGeneticOperator
	{
		public  OperatorBase () 
		{
			Enabled = true;			
		}

		/// <summary>
		/// Enabled property. Diabling this operator will cause the population to 'pass through' unaltered.
		/// </summary>
		public bool Enabled { set; get; }

		/// <summary>
		/// Gets or sets the new population.
		/// </summary>
		/// <value>The new population.</value>
		protected Population NewPopulation { set; get; }

		/// <summary>
		/// Gets or sets the current population.
		/// </summary>
		/// <value>The current population.</value>
		protected Population CurrentPopulation { set; get; }

		/// <summary>
		/// Gets or sets the fitness function.
		/// </summary>
		/// <value>The fitness function.</value>
		protected FitnessFunction FitnessFunction { set; get; }

		/// <summary>
		/// Returns the number of evaluations performed by this operator.
		/// </summary>
		/// <returns></returns>
		public virtual int GetOperatorInvokedEvaluations ()
		{
			return 0;
		}

		public virtual void Invoke (Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate)
		{
			if (currentPopulation.Solutions == null || currentPopulation.Solutions.Count == 0) {
				throw new ArgumentException ("There are no Solutions in the current Population.");
			}

			if (newPopulation == null) {
				newPopulation = currentPopulation.CreateEmptyCopy ();
			}

			CurrentPopulation = currentPopulation;
			NewPopulation = newPopulation;
			FitnessFunction = fitnesFunctionDelegate;

			if (!Enabled)
				return;

		}
	}
}
