using System;
using System.Collections.Generic;

namespace GAF.Extensions
{
	/// <summary>
	/// Chromosome extensions.
	/// </summary>
	public static class ChromosomeExtensions
	{
		/// <summary>
		/// Clones and adds the specified range.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="genes">Genes.</param>
		public static void AddRangeCloned(this Chromosome source, IEnumerable<Gene> genes)
		{
			foreach (var gene in genes) {
			
				source.Genes.Add(gene.DeepClone());
			}
		}
	}
}

