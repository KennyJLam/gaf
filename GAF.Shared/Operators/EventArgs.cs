using System;

namespace GAF.Operators
{
	/// <summary>
	/// Event arguments for the Crossover events.
	/// </summary>
	public class CrossoverEventArgs : EventArgs
	{
		private readonly CrossoverData _crossoverResult;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="crossoverResult"></param>
		public CrossoverEventArgs (CrossoverData crossoverResult)
		{
			_crossoverResult = crossoverResult;
		}

		/// <summary>
		/// Returns the crossover result.
		/// </summary>
		public CrossoverData CrossoverData {
			get { return _crossoverResult; }
		}
	}
}

