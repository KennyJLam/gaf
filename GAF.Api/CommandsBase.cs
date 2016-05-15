using System;
using System.Collections.Generic;

namespace GAF.Commands
{
	public class CommandsBase
	{
		protected GAF.GeneticAlgorithm _ga;

		//private Dictionary<string, Func<List<string>, GeneticAlgorithm, string>> _commands = new Dictionary<string, Func<List<string>, GeneticAlgorithm, string>> ();

		public delegate void PropertyChangedHandler (object sender, PropertyChangedEventArgs e);
		public event PropertyChangedHandler OnPropertyChanged;

		public delegate void ExceptionHandler (object sender, ExceptionEventArgs e);
		public event ExceptionHandler OnException;

		public CommandsBase (GAF.GeneticAlgorithm ga)
		{
			ga = _ga;
		}

		public void OnGaFPropertyChanged (object sender, PropertyChangedEventArgs args)
		{
			//we have had a property change event raised in one of the GAF objects so
			//re-raise this here for consumers of the Api
			if (OnPropertyChanged != null) {
				this.OnPropertyChanged (this, args);
			}
		}

//		public Dictionary<string, Func<List<string>, GeneticAlgorithm, string>> Commands {
//			get {
//				return _commands;
//			}
//		}

		protected void RaiseExceptionEvent (string exceptionMessage)
		{
			//we have had a property change event raised in one of the GAF objects so
			//re-raise this here for consumers of the Api
			if (OnException != null) {
				var args = new ExceptionEventArgs (exceptionMessage);
				this.OnException (this, args);
			}
		}


//		/// <summary>
//		/// Gets the command to be executed. The return value can be called as follows e.g.
//		///     var result = func (args, ga);
//		/// </summary>
//		/// <returns>The command.</returns>
//		/// <param name="commandName">Command name.</param>
//		public Func<List<string>, GeneticAlgorithm, string> GetCommand (string commandName)
//		{
//			Func<List<string>, GeneticAlgorithm, string> func = null;
//			if (this.Commands.TryGetValue (commandName, out func)) {
//				return func;
//			}
//			return null;
//		}
	}
}

