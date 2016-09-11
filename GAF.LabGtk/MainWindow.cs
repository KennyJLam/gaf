using System;
using Gtk;
using GAF;
using GAF.Api;
using System.Diagnostics;
using GAF.Api.Operators;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using GAF.LabGtk;

public partial class MainWindow : Gtk.Window
{
	private GAF.Api.GeneticAlgorithm _ga;
	private GAF.LabGtk.Bindings _bindings = new GAF.LabGtk.Bindings ();
	private GAF.LabGtk.PopulationHistoryView _populationHistoryView;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{

		Build ();

		//these two could call the same handler...
		_ga = new GeneticAlgorithm ();
		_ga.PropertyChanged += OnGafPropertyChanged;
		_ga.OnException += OnGafException;
		_ga.OnLogging += OnGafLogging;

		_ga.PopulationStatistics.PropertyChanged += OnGafPropertyChanged;

		RegisterBindings ();
		InitialiseUI ();
		UpdateUI (string.Empty);

	}

	/// <summary>
	/// Initialises the UI.
	/// </summary>
	private void InitialiseUI ()
	{
		Gtk.Application.Invoke (delegate {

			var statusText = string.Format ("{0} Version: {1} - {2}",
				_ga.FileVersionInfo.ProductName,
				_ga.FileVersionInfo.FileVersion,
				_ga.FileVersionInfo.LegalCopyright);

			statusBar.Push (1, statusText);

			mainNotebook.Page = 0;

			//var columnNames = new string [0];
			_populationHistoryView = new GAF.LabGtk.PopulationHistoryView (new string [] { string.Empty, "Fitness" });
			_populationHistoryView.BorderWidth = 1;
			_populationHistoryView.ItemSelected += populationHistoryViewItemSelected;

			populationScrolledWindow.Add (_populationHistoryView);
			populationScrolledWindow.ShowAll ();


		});
	}

	void populationHistoryViewItemSelected (GAF.LabGtk.PopulationViewItem [] obj)
	{
	}

	private void UpdateUI (string propertyName)
	{
		//invoke on UI thread
		Gtk.Application.Invoke (delegate {

			//can't bind this as something else is bound to the IsRunning and HasPopulation properties.
			startButton.Sensitive = !_ga.IsRunning && _ga.HasPopulation;
			pauseButton.Sensitive = _ga.IsRunning && !_ga.IsPaused;


			if (propertyName == "GAF.Api.GeneticAlgorithm.IsPaused") {
				//if (propertyName == "GAF.Api.PopulationStatistics.PopulationHistory") {

				_populationHistoryView.Sensitive = _ga.IsPaused;

				if (_ga.IsPaused) {

					var history = _ga.PopulationStatistics.PopulationHistory;

					_populationHistoryView.ClearItems ();

					//we need to pivot the data as we are adding a row to the list which takes from
					//multiple populations

					/*
					//get the number of columns and rows
					var columns = history.PopulationHistoryItems.Count;
					var item = history.PopulationHistoryItems [0];
					var rows = item.SolutionData.Count;

					for (int row = 0; row < rows; row++) {

						var rowData = new string [20];

						for (int column = 0; column < columns; column++) {

							var generation = history.PopulationHistoryItems [column].SourceGeneration;
							_populationHistoryView.Columns [column].Title = string.Format ("Generation {0}", generation);
							rowData [column] = history.PopulationHistoryItems [column].SolutionData [row].ToString ();
						}

						var rowDataItem = new GAF.LabGtk.PopulationViewItem (rowData.ToList ());

						//row complete so append to the list
						_populationHistoryView.AddItem (rowDataItem);
					}
					*/
					var populationHistoryItem = history.PopulationHistoryItems.LastOrDefault ();
					var rows = populationHistoryItem.SolutionData.Count;

					for (int row = 0; row < rows; row++) {

						var rowData = new List<string> ();

						var generation = populationHistoryItem.SourceGeneration;
						_populationHistoryView.Columns [0].Title = string.Format ("Generation {0}", generation);

						rowData.Add (populationHistoryItem.SolutionData [row].Data);
						rowData.Add (populationHistoryItem.SolutionData [row].Fitness.ToString ());

						//row complete so append to the list
						var rowDataItem = new GAF.LabGtk.PopulationViewItem (rowData);
						_populationHistoryView.AddItem (rowDataItem);

					}

				}
			}


		});
	}


	private void AppendToOutput (string text)
	{
		//invoke on UI thread
		Gtk.Application.Invoke (delegate {

			text = text.TrimEnd ("\n".ToCharArray ());

			var ti = outputTextView.Buffer.EndIter;

			var message = string.Format ("> {0}:{1}:{2} {3}\n",
							  DateTime.Now.Hour.ToString ("00"),
							  DateTime.Now.Minute.ToString ("00"),
							  DateTime.Now.Second.ToString ("00"), text);

			Debug.Print (message);

			outputTextView.Buffer.Insert (ref ti, message);
			outputTextView.ScrollToIter (ti, 0.0, false, 0, 0);

		});
	}

	#region Misc Control Events

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void MenuFileQuit (object sender, EventArgs e)
	{
		if (_ga.IsRunning)
			_ga.Stop ();

		Application.Quit ();
	}

	protected void MenuFileOpen (object sender, EventArgs e)
	{
		// create the dialog
		// remember to choose correct FileChooserAction from these self-explanatory values:
		// Open, Save, SelectFolder, CreateFolder
		FileChooserDialog Fcd = new FileChooserDialog ("Open file", null, FileChooserAction.Open);

		// add buttons you wish to see in the dialog
		Fcd.AddButton (Stock.Cancel, ResponseType.Cancel);
		Fcd.AddButton (Stock.Open, ResponseType.Ok);

		// then create a filter for files. For example .gif:
		// filter is not necessary if you wish to see all files in the dialog
		Fcd.Filter = new FileFilter ();
		Fcd.Filter.AddPattern ("*.dll");

		// run the dialog
		ResponseType RetVal = (ResponseType)Fcd.Run ();

		// handle the dialog's exit value
		// Read the file name from Fcd.Filename
		if (RetVal == ResponseType.Ok) {

			var filename = Fcd.Filename;

			// load the consumer functions
			try {

				_ga.LoadFitnessAndTerminateFunctions (filename);

				//population now exists so hook up the property changed event
				_ga.Population.PropertyChanged += OnGafPropertyChanged;

				AppendToOutput (string.Format ("Loaded '{0}'.", filename));

				UpdateUI (string.Empty);

			} catch (Exception ex) {

				while (ex.InnerException != null) {
					ex = ex.InnerException;
				}
				AppendToOutput (ex.Message);
			}

		} else {
			// do something else
		}
		Fcd.Destroy ();
	}

	protected void MenuHelpAbout (object sender, EventArgs e)
	{
		var ico = new Gdk.Pixbuf ("GAF128.ico");
		var about = new AboutDialog ();
		about.Title = "About";
		about.HasSeparator = true;
		about.License = _ga.Licence; //TODO: this could come from the UI Assembly if different from GAF.Api
		about.Logo = ico;
		about.Icon = ico;
		about.Version = GAF.LabGtk.Info.GetFileVersionInfo ().FileVersion;
		about.ProgramName = GAF.LabGtk.Info.GetFileVersionInfo ().ProductName;
		about.Comments = GAF.LabGtk.Info.GetFileVersionInfo ().Comments;
		about.Copyright = GAF.LabGtk.Info.GetFileVersionInfo ().LegalCopyright;
		about.Authors = new string [] { GAF.LabGtk.Info.GetFileVersionInfo ().CompanyName };

		about.Run ();
		about.Destroy ();
	}

	protected void ClearOutputText (object sender, EventArgs e)
	{
		outputTextView.Buffer.Clear ();
	}

	protected void Stop (object sender, EventArgs e)
	{
		//start GA
		_ga.Stop ();
	}

	protected void Start (object sender, EventArgs e)
	{
		_ga.Start ();
	}

	protected void Pause (object sender, EventArgs e)
	{
		_ga.Pause ();
	}

	protected void Resume (object sender, EventArgs e)
	{
		_ga.Resume ();
	}

	protected void Step (object sender, EventArgs e)
	{
		_ga.Step ();
	}

	#endregion

	#region Events

	private void OnGafPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		var propertyName = string.Format ("{0}.{1}", sender.GetType ().ToString (), e.PropertyName);

		//Debug.WriteLine ("Property Changed: {0}.", new[] { propertyName });
		UpdateUI (propertyName);

	}

	private void OnGafException (object sender, ExceptionEventArgs e)
	{
		AppendToOutput (e.Message);

		//we have to do this to reset things like the start button etc.
		UpdateUI (string.Empty);
	}

	private void OnGafLogging (object sender, LoggingEventArgs e)
	{
		AppendToOutput (e.Message);
	}

	#endregion

	#region Crossover Control Events

	protected void CrossoverEnabled (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.Crossover.Enabled = chkBtn.Active;
	}

	protected void CrossoverAllowDuplicates (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.Crossover.AllowDuplicates = chkBtn.Active;
	}

	protected void CrossoverTypeSingle (object sender, EventArgs e)
	{
		var rb = (Gtk.RadioButton)sender;
		if (rb.Active) {
			_ga.GeneticOperators.Crossover.CrossoverType = GAF.Api.Operators.CrossoverType.SinglePoint;
		}
	}

	protected void CrossoverTypeDouble (object sender, EventArgs e)
	{
		var rb = (Gtk.RadioButton)sender;
		if (rb.Active) {
			_ga.GeneticOperators.Crossover.CrossoverType = GAF.Api.Operators.CrossoverType.DoublePoint;
		}
	}

	protected void CrossoverTypeDoubleOrdered (object sender, EventArgs e)
	{
		var rb = (Gtk.RadioButton)sender;
		if (rb.Active) {
			_ga.GeneticOperators.Crossover.CrossoverType = GAF.Api.Operators.CrossoverType.DoublePointOrdered;
		}
	}


	protected void CrossoverReplacementGeneric (object sender, EventArgs e)
	{
		var rb = (Gtk.RadioButton)sender;
		if (rb.Active) {
			_ga.GeneticOperators.Crossover.ReplacementMethod = GAF.Api.Operators.ReplacementMethod.GenerationalReplacement;
		}
	}

	protected void CrossoverReplacementDeleteLast (object sender, EventArgs e)
	{
		var rb = (Gtk.RadioButton)sender;
		if (rb.Active) {
			_ga.GeneticOperators.Crossover.ReplacementMethod = GAF.Api.Operators.ReplacementMethod.DeleteLast;
		}
	}

	protected void CrossoverProbability (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.Crossover.CrossoverProbability = hscale.Value;
	}

	#endregion

	#region BinaryMutate Control Events

	protected void BinaryMutateEnabled (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.BinaryMutate.Enabled = chkBtn.Active;
	}

	protected void BinaryMutateProbability (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.BinaryMutate.MutationProbability = hscale.Value;
	}

	protected void BinaryMutateAllowDuplicates (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.BinaryMutate.AllowDuplicates = chkBtn.Active;
	}
	#endregion

	#region Elite Control Events

	protected void EliteEnabled (object sender, EventArgs e)
	{
		var cb = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.Elite.Enabled = cb.Active;
	}

	protected void ElitePercentage (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.Elite.Percentage = (int)hscale.Value;
	}

	#endregion

	#region RandomReplace Control Events

	protected void RandomReplaceEnabled (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.RandomReplace.Enabled = chkBtn.Active;
	}

	protected void RandomReplacePercentage (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.RandomReplace.Percentage = (int)hscale.Value;
	}

	protected void RandomReplaceAllowDuplicates (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.RandomReplace.AllowDuplicates = chkBtn.Active;
	}

	#endregion

	#region SwapMutate Control Events

	protected void SwapMutateEnabled (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.SwapMutate.Enabled = chkBtn.Active;
	}

	protected void SwapMutateProbability (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.SwapMutate.Probability = hscale.Value;
	}

	#endregion

	#region Memory Control Events

	protected void MemoryEnabled (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.GeneticOperators.Memory.Enabled = chkBtn.Active;
	}

	protected void MemoryUpdatePeriod (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.Memory.GenerationalUpdatePeriod = (int)hscale.Value;
	}

	protected void MemorySize (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.GeneticOperators.Memory.MemorySize = (int)hscale.Value;
	}

	#endregion

	#region Population

	protected void FitnessProportionalRadioButton (object sender, EventArgs e)
	{
		if (_ga.HasPopulation) {
			var rb = (Gtk.RadioButton)sender;
			if (rb.Active) {
				_ga.Population.ParentSelectionMethod = ParentSelectionMethod.FitnessProportionateSelection;
			}
		}
	}

	protected void StochasticUniversalRadioButton (object sender, EventArgs e)
	{
		if (_ga.HasPopulation) {
			var rb = (Gtk.RadioButton)sender;
			if (rb.Active) {
				_ga.Population.ParentSelectionMethod = ParentSelectionMethod.StochasticUniversalSampling;
			}
		}
	}

	protected void TournamentRadioButtor (object sender, EventArgs e)
	{
		if (_ga.HasPopulation) {
			var rb = (Gtk.RadioButton)sender;
			if (rb.Active) {
				_ga.Population.ParentSelectionMethod = ParentSelectionMethod.TournamentSelection;
			}
		}
	}

	protected void LinearlyNormalised (object sender, EventArgs e)
	{
		if (_ga.HasPopulation) {
			var chkBtn = (Gtk.CheckButton)sender;
			_ga.Population.LinearlyNormalised = chkBtn.Active;
		}
	}

	protected void ReEvaluateAll (object sender, EventArgs e)
	{
		if (_ga.HasPopulation) {
			var chkBtn = (Gtk.CheckButton)sender;
			_ga.Population.ReEvaluateAll = chkBtn.Active;
		}
	}

	protected void EvaluateInParallel (object sender, EventArgs e)
	{
		if (_ga.HasPopulation) {
			var chkBtn = (Gtk.CheckButton)sender;
			_ga.Population.EvaluateInParallel = chkBtn.Active;
		}
	}

	#endregion

	#region Statistics

	protected void StatisticsEnable (object sender, EventArgs e)
	{
		var chkBtn = (Gtk.CheckButton)sender;
		_ga.PopulationStatistics.Enabled = chkBtn.Active;
	}


	protected void DiversityGraphScale (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.PopulationStatistics.DiversityGraphScale = hscale.Value / 10.0;
	}

	#endregion

	#region Delay

	protected void ThreadDelay (object sender, EventArgs e)
	{
		var hscale = (Gtk.HScale)sender;
		_ga.ThreadDelayMilliseconds = (int)System.Math.Round (hscale.Value);
	}

	#endregion

}
