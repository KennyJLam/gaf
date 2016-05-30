using System;

namespace GAF.Api
{
	public interface IGafLabPopulation
	{
		/// <summary>
		/// This interface is provided to support the GAF.Lab GUI application. Please see the product information
		/// for GAF.Lab for further details.
		/// </summary>
		/// <returns>The population.</returns>
		GAF.Population CreatePopulation();
	}
}

