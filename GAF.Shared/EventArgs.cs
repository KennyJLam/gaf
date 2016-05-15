using System;
using System.Collections.Generic;

namespace GAF
{

	/// <summary>
	/// Event arguments used within the main GA exeption events.
	/// </summary>
	public class ExceptionEventArgs : EventArgs
	{
		private readonly string _message;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public ExceptionEventArgs(string message)
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
	/// Event arguments used within the main GA exeption events.
	/// </summary>
	public class GaExceptionEventArgs : ExceptionEventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public GaExceptionEventArgs(string message) : base(message)
		{
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

	/// <summary>
	/// Event arguments used within the Property Changed events of all objects that support them.
	/// </summary>
	public class PropertyChangedEventArgs : EventArgs
	{
		private readonly string _propertyName;

		/// <summary>
		/// Initializes a new instance of the <see cref="GAF.PropertyChangedEventArgs"/> class.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		public PropertyChangedEventArgs(string propertyName)
		{
			_propertyName = propertyName;
		}

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName
		{
			get { return _propertyName; }
		}

	}

	/// <summary>
	/// Evaluation event arguments.
	/// </summary>
	public class EvaluationEventArgs : EventArgs
	{
		private readonly List<Chromosome> _solutionsToEvaluate;
		private readonly FitnessFunction _fitnessFunction;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name = "solutionsToEvaluate"></param>
		/// <param name = "fitnessFunctionDelegate"></param>
		public EvaluationEventArgs(List<Chromosome> solutionsToEvaluate, FitnessFunction fitnessFunctionDelegate)
		{
			_solutionsToEvaluate = solutionsToEvaluate;
			_fitnessFunction = fitnessFunctionDelegate;
			Evaluations = 0;
		}

		/// <summary>
		/// Returns the population.
		/// </summary>
		public List<Chromosome> SolutionsToEvaluate
		{
			get { return _solutionsToEvaluate; }
		}

		/// <summary>
		/// Gets the fitness function.
		/// </summary>
		/// <value>The fitness function.</value>
		public FitnessFunction FitnessFunctionDelegate
		{
			get { return _fitnessFunction; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to cancel further evaluations. This property
		/// can be used by consumers of the event to stop further evaluations.
		/// </summary>
		/// <value><c>true</c> to cancel; otherwise, <c>false</c>.</value>
		public bool Cancel { set; get; }

		/// <summary>
		/// Gets or sets the evaluation count. This property should be used by consumers of the event
		/// to update the evaluation count if appropriate.
		/// count
		/// </summary>
		/// <value>The numbe of evaluations undertaken eithin the event.</value>
		public int Evaluations { set; get; }
	}

}

