﻿/*
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

namespace GAF
{
	/// <summary>
	/// Chromosome corrupt exception.
	/// </summary>
	public class ChromosomeCorruptException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeCorruptException"/> class
		/// </summary>
		public ChromosomeCorruptException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeCorruptException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the ChromosomeException </param>
		public ChromosomeCorruptException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChromosomeCorruptException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of thChromosomeExceptionxception. </param>
		public ChromosomeCorruptException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

