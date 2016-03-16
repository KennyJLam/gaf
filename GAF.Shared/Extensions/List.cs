﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GAF.Extensions
{
    /// <summary>
    /// Extension class for the List&lt;T&gt; type.
    /// </summary>

    public static class List
    {
        /// <summary>
        /// Creates a CSV formatted string.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToCsv(this List<Chromosome> source )
        {
            var report = new StringBuilder();
            report.Append("Solution, Fitness \n");

            int index = 0;
            foreach (Chromosome solution in source)
            {
                report.Append(index.ToString(CultureInfo.InvariantCulture));
                report.Append(",");
                report.Append(solution.Fitness);
                report.Append("\n");

                index++;
            }

            return report.ToString();
        }

		/// <summary>
		/// Clones and adds the specified range.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="genes">Genes.</param>
		public static void AddRangeCloned(this List<Gene> source, IEnumerable<Gene> genes)
		{
			foreach (var gene in genes) {

				source.Add(gene.DeepClone());
			}
		}
		/// <summary>
		/// Clones and adds the specified range.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="gene">Gene.</param>
		public static void AddCloned(this List<Gene> source, Gene gene)
		{
				source.Add(gene.DeepClone());
		}

//        /// <summary>
//        /// Performs a dep clone of a Gene using a memory stream.
//        /// </summary>
//        /// <param name="source"></param>
//        /// <returns></returns>
//        public static List<Gene> DeepClone(this List<Gene> source)
//        {
//
//			//return source;
//
//			var result = new List<Gene> ();
//			foreach(var gene in source)
//			{
//				var clone = gene.DeepClone ();
//				result.Add (clone);
//
////				clone.BinaryValue = gene.BinaryValue;
////				clone.GeneType = gene.GetType;
////				clone.ObjectValue = gene.ObjectValue;
////				clone.RealValue = gene.RealValue;
//
//			}
//				
//            return result;
//        }
			
    }
}


