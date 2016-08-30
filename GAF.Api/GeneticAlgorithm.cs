using System;
using System.Collections.Generic;
using GAF.Operators;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace GAF.Api
{
	public class GeneticAlgorithm : GafApiBase
	{
		private bool _isRunning;
		private bool _isPaused;
		private ConsumerFunctions _functions;
		private Population _population;
		private int _generations;
		private long _evaluations;
		private double _maxFitness;
		private GAF.GeneticAlgorithm _ga;
		private Stopwatch _stopWatch;
		private FileVersionInfo _fileVersionInfo;
		private bool _hasFitnessFunction;
		private bool _hasTerminateFunction;
		private bool _hasPopulation;
		private string _productDescription;
		private string _licence;
		private int _threadDelayMilliseconds;
		private long _runMilliseconds;
		private long _generationMilliseconds;
		private string _sourceCodeDescription;
		private long _tempGenerationMilliseconds;

		private object _syncLock = new object ();

		public GeneticAlgorithm () : base ()
		{
			// GA API
			this.GeneticOperators = new GAF.Api.GeneticOperators ();
			this.GeneticOperators.OnException += (object sender, Api.ExceptionEventArgs e) => RaisePropertyChangedEvent (e.Message);
			this.GeneticOperators.OnLogging += (object sender, Api.LoggingEventArgs e) => RaiseLoggingEvent (e.Message, e.IsWarning);
			this.GeneticOperators.PropertyChanged += (object sender, PropertyChangedEventArgs e) => SourceCodePropertyChanged (e.PropertyName);

			//Population Api
			this.Population = new Population ();
			this.Population.OnException += (object sender, ExceptionEventArgs e) => RaisePropertyChangedEvent (e.Message);
			this.Population.OnLogging += (object sender, LoggingEventArgs e) => RaiseLoggingEvent (e.Message, e.IsWarning);
			this.Population.PropertyChanged += (object sender, PropertyChangedEventArgs e) => SourceCodePropertyChanged (e.PropertyName);

			//Statistics Api
			this.PopulationStatistics = new PopulationStatistics ();
			this.PopulationStatistics.OnException += (object sender, ExceptionEventArgs e) => RaisePropertyChangedEvent (e.Message);
			this.PopulationStatistics.OnLogging += (object sender, LoggingEventArgs e) => RaiseLoggingEvent (e.Message, e.IsWarning);

			this.Stopwatch = new Stopwatch ();

			this.FileVersionInfo = Info.GetFileVersionInfo ();
			this.Licence = HelpText.Licence;

			this.SourceCodeDescription = HelpText.SourceCodeDescription;
		}

		private void SourceCodePropertyChanged (string propertyName)
		{
			//this will be executed whenever an operator property is changed.
			//it is used to determine a source code change only

			//all properties affect source code except 'Description'.

			if (propertyName != "Description") {
				RaisePropertyChangedEvent ("SourceCode");
			}

		}

		/// <summary>
		/// Gets the stop watch.
		/// </summary>
		/// <value>The stop watch.</value>
		private Stopwatch Stopwatch {
			get {
				lock (_syncLock) {
					return _stopWatch;
				}
			}
			set {
				lock (_syncLock) {
					UpdateField<Stopwatch> (ref _stopWatch, value, "Stopwatch", true);
				}
			}
		}

		#region Properties

		/// <summary>
		/// Gets the thread delay milliseconds.
		/// </summary>
		/// <value>The thread delay milliseconds.</value>
		public int ThreadDelayMilliseconds {
			get { return _threadDelayMilliseconds; }
			set {
				UpdateField (ref _threadDelayMilliseconds, value, "ThreadDelayMilliseconds", true);
			}
		}

		/// <summary>
		/// Gets or sets the time in milliseconds of a GA run.
		/// </summary>
		/// <value>The run milliseconds.</value>
		public long RunMilliseconds {
			get {
				lock (_syncLock) {
					return _runMilliseconds;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField (ref _runMilliseconds, value, "RunMilliseconds", true);
				}
			}
		}

		/// <summary>
		/// Gets or sets the time in milliseconds to complete a generation.
		/// </summary>
		/// <value>The generation milliseconds.</value>
		public long GenerationMilliseconds {
			get {
				lock (_syncLock) {
					return _generationMilliseconds;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField (ref _generationMilliseconds, value, "GenerationMilliseconds", true);
				}
			}
		}

		public Population Population {
			get {
				if (_population != null) {
					return _population;
				} else {
					RaiseExceptionEvent ("Population", "The population has not been defined. Consider checking the 'HasPopulation' before accessing this property.");
				}
				return null;
			}
			private set {
				UpdateField<Population> (ref _population, value, "Population", true);
			}
		}

		public PopulationStatistics PopulationStatistics { set; get; }

		public Api.GeneticOperators GeneticOperators { set; get; }

		public bool IsRunning {
			get {
				lock (_syncLock) {
					return _isRunning;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField<bool> (ref _isRunning, value, "IsRunning");
				}
			}
		}

		public bool IsPaused {
			get {
				lock (_syncLock) {
					return _isPaused;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField<bool> (ref _isPaused, value, "IsPaused");
				}
			}
		}
		public bool HasFitnessFunction {
			get {
				return _hasFitnessFunction;
			}
			private set {
				UpdateField (ref _hasFitnessFunction, value, "HasFitnessFunction");
			}
		}

		public bool HasTerminateFunction {
			get {
				return _hasTerminateFunction;
			}
			private set {
				UpdateField (ref _hasTerminateFunction, value, "HasTerminateFunction");
			}
		}

		public bool HasPopulation {
			get {
				return _hasPopulation;
			}
			private set {
				UpdateField (ref _hasPopulation, value, "HasPopulation");
			}
		}

		public int Generations {
			get {
				lock (_syncLock) {
					return _generations;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField<int> (ref _generations, value, "Generations");
				}
			}
		}

		public long Evaluations {
			get {
				lock (_syncLock) {
					return _evaluations;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField<long> (ref _evaluations, value, "Evaluations");
				}
			}
		}

		public double MaximumFitness {
			get {
				lock (_syncLock) {
					return _maxFitness;
				}
			}
			private set {
				lock (_syncLock) {
					UpdateField<double> (ref _maxFitness, value, "MaximumFitness");
				}
			}
		}

		public FileVersionInfo FileVersionInfo {
			get {
				return _fileVersionInfo;
			}
			private set {
				UpdateField (ref _fileVersionInfo, value, "FileVersionInfo");
			}
		}

		public string Licence {
			get {
				return _licence;
			}
			private set {
				UpdateField (ref _licence, value, "Licence");
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Start the specified populationSize and chromosomeLength.
		/// </summary>
		public void Start ()
		{
			if (_ga != null && _ga.IsRunning) {
				RaiseLoggingEvent ("The GA cannot be started as it is already running.", true);
			} else {

				CreateGeneticAlgorithm ();
				if (ValidateGa (_ga)) {
					if (!_ga.IsRunning) {
						this.RunMilliseconds = 0;

						_tempGenerationMilliseconds = 0;

						Stopwatch = Stopwatch.StartNew ();

						_ga.RunAsync (_functions.TerminateFunction);

						IsRunning = true;

					} else {
						RaiseLoggingEvent ("The GA could not be created.", true);
					}
				}
			}
		}

		public void Stop ()
		{
			if (_ga != null && _ga.IsRunning) {
				_ga.Halt ();
				this.Stopwatch.Stop ();
				IsRunning = false;
				IsPaused = false;
			}
		}

		public void Pause ()
		{
			if (_ga != null && _ga.IsRunning) {
				_ga.Pause ();
				this.Stopwatch.Stop ();
				IsPaused = true;
			}
		}

		public void Resume ()
		{
			if (_ga != null && _ga.IsRunning) {
				_ga.Resume ();
				this.Stopwatch.Start ();
				IsPaused = false;
			}
		}

		public void Step ()
		{
			if (_ga != null && _ga.IsRunning) {
				_ga.Resume ();
				IsPaused = false;

				//TODO: Fix this as it is far to crude! Consider setting pause after the next generation.
				Thread.Sleep (1); //1ms should never be enough for a generation but is enough for the thread to resume.
				_ga.Pause ();
				IsPaused = true;
			}
		}

		private void CreateGeneticAlgorithm ()
		{
			if (_functions == null) {
				RaiseExceptionEvent ("CreateGeneticAlgorithm", "The consumer functions have not been loaded.");
				return;
			}

			var gafPopulation = _functions.CreatePopulation ();
			gafPopulation.OnLogging += (object sender, GAF.LoggingEventArgs e) => RaiseLoggingEvent (e.Message, e.IsWarning);

			if (gafPopulation == null) {
				RaiseExceptionEvent ("CreateGeneticAlgorithm", "The population function has not been specified.");
				return;
			}
			if (_functions.FitnessFunction == null) {
				RaiseExceptionEvent ("CreateGeneticAlgorithm", "The fitness function has not been specified.");
				return;
			}
			if (_functions.TerminateFunction == null) {
				RaiseExceptionEvent ("CreateGeneticAlgorithm", "The terminate function has not been specified.");
				return;
			}

			_ga = new GAF.GeneticAlgorithm (gafPopulation, _functions.FitnessFunction);
			_ga.OnRunException += (object sender, GAF.ExceptionEventArgs e) => {

				//something has gone wrong, update the local 'IsRunning'property.
				this.IsRunning = _ga.IsRunning;
				RaiseExceptionEvent ("CreateGeneticAlgorithm", e.Message);
			};

			_ga.TerminateFunction = _functions.TerminateFunction;
			_ga.OnGenerationComplete += (object sender, GaEventArgs e) => {

				var preStats = this.Stopwatch.ElapsedMilliseconds;
				if (PopulationStatistics.Enabled) {
					this.PopulationStatistics.ProcessPopulation (e.Population, e.Generation);
					this.RaisePropertyChangedEvent ("PopulationStatistics");
				}
				var statsTime = this.Stopwatch.ElapsedMilliseconds - preStats;

				this.Generations = e.Generation;
				this.Evaluations = e.Evaluations;
				this.MaximumFitness = e.Population.MaximumFitness;
				//population will have changed
				//this.RaisePropertyChangedEvent ("Population");
				var message = _functions.CreateGenerationCompleteMessage (e.Population, e.Generation, e.Evaluations);
				this.RaiseLoggingEvent (message, false);

				System.Threading.Thread.Sleep (this.ThreadDelayMilliseconds);

				this.GenerationMilliseconds = this.Stopwatch.ElapsedMilliseconds - _tempGenerationMilliseconds - this.ThreadDelayMilliseconds - statsTime;
				_tempGenerationMilliseconds = this.Stopwatch.ElapsedMilliseconds;
				this.RunMilliseconds += this.GenerationMilliseconds;


			};

			_ga.OnRunComplete += (object sender, GaEventArgs e) => {

				this.Stopwatch.Stop ();

				IsRunning = false;

				var message = _functions.CreateRunCompleteMessage (e.Population, e.Generation, e.Evaluations);
				this.RaiseLoggingEvent (message, false);

			};

			_ga.Operators.AddRange (GeneticOperators.GafOperators);
			RaisePropertyChangedEvent ("GeneticOperators");

			//add the new ga (with its population) and allow 
			//it to be modified in line with the current Api.Population state.
			this.Population.SetGeneticAlgorithm (_ga, false);

			//population has changed
			RaisePropertyChangedEvent ("Population");

			//reset the history
			PopulationStatistics.PopulationHistory.ClearHistory();
		}

		/// <summary>
		/// Loads the fitness and terminate functions from the specified assembly.
		/// </summary>
		/// <param name="assemblyFilename">Assembly filename.</param>
		public void LoadFitnessAndTerminateFunctions (string assemblyFilename)
		{
			_functions = new ConsumerFunctions (assemblyFilename);

			var gafPopulation = _functions.CreatePopulation ();

			if (gafPopulation == null) {
				RaiseExceptionEvent ("LoadFitnessAndTerminateFunctions", "Population not loaded. The Fitness Function is null.");
			}
			if (_functions.FitnessFunction == null) {
				RaiseExceptionEvent ("LoadFitnessAndTerminateFunctions", "Fitness Function not loaded. The Fitness Function is null.");
			}
			if (_functions.FitnessFunction == null) {
				RaiseExceptionEvent ("LoadFitnessAndTerminateFunctions", "Terminate Function not loaded. The Terminate Function is null.");
			}

			this.HasFitnessFunction = true;
			this.HasTerminateFunction = true;
			this.HasPopulation = true;

			//update the api.population with just the state in order that changes can be made before
			//the ga is run etc. See notes in constructor comments
			this.Population.SetState (gafPopulation);

			if (_functions.OperatorOptions != null && _functions.OperatorOptions.PermutationProblem) {
				GeneticOperators.SetOperatorsForPermutationProblem ();
			}

		}

		/// <summary>
		/// Checks the specified GA to see if it is valid to run
		/// </summary>
		/// <returns><c>true</c>, if ga was validated, <c>false</c> otherwise.</returns>
		/// <param name="ga">Ga.</param>
		protected bool ValidateGa (GAF.GeneticAlgorithm ga)
		{
			//check we have an object
			if (ga == null) {
				RaiseExceptionEvent ("ValidateGa", "The GA object is null.");
				return false;
			}

			//check that there are some enabled operators
			if (ga.Operators.All (o => o.Enabled != true)) {
				RaiseExceptionEvent ("ValidateGa", "There are no operators defined.");
				return false;
			}

			//check there is a valid Fitness function
			if (ga.FitnessFunction == null) {
				RaiseExceptionEvent ("ValidateGa", "The fitness function has not been specified.");
				return false;
			}

			if (ga.TerminateFunction == null) {
				RaiseExceptionEvent ("ValidateGa", "The terminate function has not been specified.");
				return false;
			}

			if (ga.Population == null) {
				RaiseExceptionEvent ("ValidateGa", "The population has not been specified.");
				return false;
			}

			return true;
		}

		#endregion

		#region Code Generation

		public string SourceCode {
			get {
				var code = new CodeGeneration (this);
				return code.GeneticAlgorithmCode ();
			}
		}

		public string SourceCodeDescription {
			get {
				return _sourceCodeDescription;
			}
			protected set {
				UpdateField (ref _sourceCodeDescription, value, "SourceCodeDescription");
			}
		}

		//		_genotypicDescription
		#endregion

	}
}

