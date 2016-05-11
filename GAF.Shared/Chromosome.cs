/*
	Genetic Algorithm Framework for .Net
	Copyright (C) 2016  John Newcombe

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

		You should have received a copy of the GNU Lesser General Public License
		along with this program.  If not, see <http://www.gnu.org/licenses/>.

	http://johnnewcombe.net
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GAF.Threading;
using GAF.Exceptions;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GAF
{
	/// <summary>
	/// This is the delegate definition for the Fitness function.
	/// </summary>
	/// <param name="solution"></param>
	/// <returns></returns>
	public delegate double FitnessFunction (Chromosome solution);

	/// <summary>
	/// This clas represents a chromosome.
	/// </summary>
	#if !PCL
	[Serializable]
	#endif
	public class Chromosome : IEnumerable, IComparable<Chromosome>
	{
		/// <summary>
		/// This is the delegate type that is passed to the Evaluate function.
		/// </summary>
		/// <returns></returns>
		private Guid _id = Guid.NewGuid ();
		private List<Gene> _genes = new List<Gene> ();

		/// <summary>
		/// Constructor. Create a Chromosome with no Genes.
		/// </summary>
		public Chromosome ()
		{
            
		}

		/// <summary>
		/// Constructor. Specify the chromosome length, i.e. the number of genes.
		/// </summary>
		/// <param name="length"></param>
		public Chromosome (int length)
		{
			//creates a random chromosome with values between -1 and +1
			//this gives a randomly created chromosome for both real and
			//binary construction
			for (var index = 0; index < length; index++) {
				var value = RandomProvider.GetThreadRandom ().NextDouble ();

				//change range to -1 to +1
				value = (value - 0.5) * 2;

				_genes.Add (new Gene () { ObjectValue = value > 0 });
			}

		}

		/// <summary>
		/// Constructor that accepts a binary string.
		/// </summary>
		/// <param name="binaryString"></param>
		public Chromosome (string binaryString)
		{
			try {
				foreach (Char digit in binaryString) {
					this.Add (new Gene () { ObjectValue = digit == '1' ? 1 : -1 });
				}
			} catch (Exception ex) {
				throw new ArgumentException ("Invalid string.", ex);
			}
		}

		/// <summary>
		/// Constructor that acceps a list or array of real numbers.
		/// </summary>
		/// <param name="reals"></param>
		public Chromosome (IEnumerable<double> reals)
		{
			try {
				foreach (var digit in reals) {
					this.Add (new Gene () { ObjectValue = digit });
				}
			} catch (Exception ex) {
				throw new ArgumentException ("Invalid range.", ex);
			}
		}

		/// <summary>
		/// Constructor that acceps list or array of integer numbers.
		/// </summary>
		/// <param name="ints"></param>
		public Chromosome (IEnumerable<int> ints)
		{
			try {
				foreach (var digit in ints) {
					this.Add (new Gene () { ObjectValue = digit });
				}
			} catch (Exception ex) {
				throw new ArgumentException ("Invalid range.", ex);
			}
		}

		/// <summary>
		/// Constructor that acceps list or array of Genes.
		/// </summary>
		/// <param name="genes"></param>
		public Chromosome (IEnumerable<Gene> genes)
		{
			try {
				_genes.AddRange (genes);
			} catch (Exception ex) {
				throw new ArgumentException ("Invalid range.", ex);
			}

		}

		/// <summary>
		/// Add the specified gene.
		/// </summary>
		/// <param name="gene">Gene.</param>
		public void Add (Gene gene)
		{
			_genes.Add (gene);
		}

		/// <summary>
		/// Adds a range of genes to the Chromosome.
		/// </summary>
		/// <param name="genes"></param>
		public void AddRange (List<Gene> genes)
		{
			_genes.AddRange (genes);
		}

		/// <summary>
		/// Removes all the Genes from the Chromosome.
		/// </summary>
		public void Clear ()
		{
			_genes.Clear ();
		}

		/// <summary>
		/// Returns the Last gene in the chromosome. 
		/// Helper method that is typically used when Non-pheno type genes exist within the chromosome.
		/// </summary>
		/// <returns></returns>
		public Gene LastGene ()
		{
			return Genes.Last ();
		}

		/// <summary>
		/// Returns the Last gene in the chromosome.
		/// Helper method that is typically used when Non-pheno type genes exist within the chromosome.
		/// </summary>
		/// <returns></returns>
		public Gene FirstGene ()
		{
			return Genes.First ();
		}

		/// <summary>
		/// Returns the number of genes in the chromosome.
		/// </summary>
		public int Count {
			get { return _genes.Count; }
		}

		/// <summary>
		/// Returns the enumerator.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return _genes.GetEnumerator ();
		}

		/// <summary>
		/// Internal property used to indicate whether this chromosome was evaluated by an operator.
		/// This is used to prevent unnessesary evaluations occuring.
		/// </summary>
		internal bool EvaluatedByOperator { set; get; }

		/// <summary>
		/// Stores the fitness value of the most recent evaluation.
		/// </summary>
		public double Fitness { get; internal set; }

		/// <summary>
		/// Stores the linearly normalised fitness value of the most recent evaluation.
		/// </summary>
		public double FitnessNormalised { get; internal set; }

		/// <summary>
		/// Returns the globally unique ID for this chromosome.
		/// </summary>
		public Guid Id { 
			get { return _id; }
			internal set { _id = value; }
		}

		/// <summary>
		/// Returns the genes.
		/// </summary>
		public List<Gene> Genes {
			get { return _genes; }
			internal set { _genes = value; }
		}

		/// <summary>
		/// This gets set by the selection process and is available for use by
		/// custom Genetic Operators to determine if the Chromosome was selected 
		/// as an Elite chromosome.
		/// </summary>
		public bool IsElite { set; get; }

		/// <summary>
		/// Returns a binary string representation of the Chromosome.
		/// </summary>
		/// <returns></returns>
		public string ToBinaryString ()
		{
			var binaryString = new StringBuilder ();

			foreach (var gene in _genes) {
				binaryString.Append (gene.BinaryValue.ToString (CultureInfo.InvariantCulture));
			}

			return binaryString.ToString ();
		}

		/// <summary>
		/// Returns a string representation of the Chromosome.
		/// </summary>
		/// <returns></returns>
		public new string ToString ()
		{
			var realNumberString = new StringBuilder ();

			foreach (var gene in _genes) {
				if (gene.GeneType == GeneType.Object) {
					realNumberString.Append (gene.ObjectValue.ToString ());
				} else {
					realNumberString.Append (gene.RealValue.ToString (CultureInfo.InvariantCulture));
				}
				realNumberString.Append (" ");
			}
			var length = realNumberString.Length;
			if (length > 0) {
				realNumberString.Remove (length - 1, 1);
			}
			return realNumberString.ToString ();
		}

		/// <summary>
		/// Returns a binary string representation of the Chromosome. 
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public string ToBinaryString (int startIndex, int length)
		{
			return this.ToBinaryString ().Substring (startIndex, length);
		}
			
		/// <summary>
		/// Evaluates the Chromosome by invoking the specified delegate method.
		/// The fitness function should return a higher 
		/// value for those chromosomes that are deemed fitter.
		/// </summary>
		/// <param name="fitnessFunctionDelegate"></param>
		/// <returns></returns>
		public double Evaluate (FitnessFunction fitnessFunctionDelegate)
		{			
            var fitness = fitnessFunctionDelegate.Invoke(this);
            if (fitness < 0 || fitness > 1.0)
                throw new EvaluationException("The fitness value must be within the range 0.0 to 1.0.");

            Fitness = fitness;
			return fitness;
		}

		/// <summary>
		/// Evaluates the Chromosome by invoking the specified delegate method.
		/// The fitness function should return a higher 
		/// value for those chromosomes that are deemed fitter.
		/// </summary>
		/// <param name="fitnessFunctionDelegate"></param>
		/// <param name = "state"></param>
		/// <returns></returns>
		public double Evaluate (FitnessFunction fitnessFunctionDelegate, object state)
		{			
			var fitness = fitnessFunctionDelegate.Invoke(this);
			if (fitness < 0 || fitness > 1.0)
				throw new EvaluationException("The fitness value must be within the range 0.0 to 1.0.");

			Fitness = fitness;
			return fitness;
		}
		/// <summary>
		/// Creates a new GUID for the Chromosome
		/// </summary>
		internal void NewId ()
		{
			_id = Guid.NewGuid ();
		}

		/// <summary>
		/// IComparable implementation.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo (Chromosome other)
		{
			return other.Fitness.CompareTo (this.Fitness);
		}

		/// <summary>
		/// Gets or sets the tag property. The tag property is designed to store chromosome meta data
		/// and can be used for any purpose.
		/// Typically the tag property would be used to pass any chromosome related data to the fitness function.
		/// </summary>
		/// <value>The tag.</value>
		public object Tag { get; set; }
	}
}

