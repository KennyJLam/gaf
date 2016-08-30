using System;

namespace GAF.Api
{
	[System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class OperatorOptionsAttribute : Attribute
	{
		/// <summary>
		/// This class is provided to support the GAF.Lab GUI application. Please see the product information
		/// for GAF.Lab for further details.
		/// </summary>
		public bool PermutationProblem { get; set; }
		/// <summary>
		/// This class is provided to support the GAF.Lab GUI application. Please see the product information
		/// for GAF.Lab for further details.
		/// </summary>
		public OperatorOptionsAttribute()
		{
			PermutationProblem = false;
		}
	}
}

