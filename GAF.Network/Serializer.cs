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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace GAF.Network
{
	/// <summary>
	/// Binary serializer.
	/// </summary>
	public static class Serializer
	{

		public static T DeSerialize<T> (byte [] byteData)
		{
			return DeSerialize<T> (byteData, new List<Type> ());
		}

		/// <summary>
		/// Deserializes specified bytes.
		/// </summary>
		/// <returns>The serialize.</returns>
		/// <param name="byteData">Byte data.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeSerialize<T> (byte [] byteData, List<Type> knownTypes)
		{

			DataContractJsonSerializer js = new DataContractJsonSerializer (typeof (T), knownTypes);
			MemoryStream memoryStream = new MemoryStream (byteData);

			var objectData = (T)js.ReadObject (memoryStream);
			memoryStream.Close ();

			return objectData;

		}

		public static T DeSerialize<T> (string json, List<Type> knownTypes)
		{

			DataContractJsonSerializer js = new DataContractJsonSerializer (typeof (T), knownTypes);


			MemoryStream memoryStream = new MemoryStream (System.Text.Encoding.ASCII.GetBytes (json));

			var objectData = (T)js.ReadObject (memoryStream);
			memoryStream.Close ();

			return objectData;

		}

		public static byte [] Serialize<T> (object obj)
		{
			return Serialize<T> (obj, new List<Type> ());
		}
		/// <summary>
		/// Serialize the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static byte [] Serialize<T> (object obj, List<Type> knownTypes)
		{
			using (MemoryStream memoryStream = new MemoryStream ()) {

				DataContractJsonSerializer serializer = new DataContractJsonSerializer (typeof (T), knownTypes);
				serializer.WriteObject (memoryStream, obj);

				memoryStream.Position = 0;
				var bytes = memoryStream.ToArray ();

				memoryStream.Close ();

				return bytes;
			}

		}
		public static string SerialiseToJson<T> (object obj)
		{
			return SerialiseToJson<T> (obj, new List<Type> ());
		}

		public static string SerialiseToJson<T> (object obj, List<Type> knownTypes)
		{
			using (MemoryStream memoryStream = new MemoryStream ()) {

				DataContractJsonSerializer serializer = new DataContractJsonSerializer (typeof (T), knownTypes);
				serializer.WriteObject (memoryStream, obj);

				memoryStream.Position = 0;
				StreamReader sr = new StreamReader (memoryStream);

				var json = sr.ReadToEnd ();

				memoryStream.Close ();

				return json;
			}
		}
	}
}
