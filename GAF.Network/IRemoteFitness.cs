using System;
using System.Collections.Generic;

namespace GAF.Network
{
	public interface IRemoteFitness : IFitness
	{
		List<Type> GetKnownTypes ();
	}
}

