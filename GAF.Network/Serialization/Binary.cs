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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GAF.Network.Serialization
{

	/// <summary>
	/// Binary serializer.
	/// </summary>
	public static class Binary
	{
		/// <summary>
		/// Deserializes specified bytes.
		/// </summary>
		/// <returns>The serialize.</returns>
		/// <param name="byteData">Byte data.</param>
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
			return DeSerializeViaXml<T> (byteData, knownTypes);
		}

		private static T DeSerializeViaJson<T> (byte [] byteData, List<Type> knownTypes)
		{
			try {

				using (var memoryStream = new MemoryStream (byteData)) {

					var js = new DataContractJsonSerializer (typeof (T), knownTypes);
					var objectData = (T)js.ReadObject (memoryStream);
					memoryStream.Close ();

					return objectData;
				}

			} catch (Exception) {

				Log.Debug (byteData);
				throw;
			}
		}

		/// <summary>
		/// Deserializes specified bytes.
		/// </summary>
		/// <returns>The serialize new.</returns>
		/// <param name="byteData">Byte data.</param>
		/// <param name="knownTypes">Known types.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private static T DeSerializeViaXml<T> (byte [] byteData, List<Type> knownTypes)
		{
			try {

				using (var memoryStream = new MemoryStream (byteData)) {

					var serializer = new DataContractSerializer (typeof (T), knownTypes);
					var objectData = (T)serializer.ReadObject (memoryStream);
					memoryStream.Close ();

					return objectData;
				}

			} catch (Exception) {
				Log.Debug (byteData);
				throw;
			}
		}

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static byte [] Serialize<T> (T obj)
		{
			return Serialize<T> (obj, new List<Type> ());
		}

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="knownTypes">Known types.</param>
		/// <typeparam name="T">The 1st type parameter.</typepara
		public static byte [] Serialize<T> (T obj, List<Type> knownTypes)
		{
			return SerializeViaXml<T> (obj, knownTypes);
		}

		private static byte [] SerializeViaJson<T> (T obj, List<Type> knownTypes)
		{
			byte [] bytes = null;

			try {
				using (MemoryStream memoryStream = new MemoryStream ()) {

					DataContractJsonSerializer serializer = new DataContractJsonSerializer (typeof (T), knownTypes);
					serializer.WriteObject (memoryStream, (T)obj);

					//memoryStream.Position = 0;
					bytes = memoryStream.ToArray ();
					memoryStream.Close ();

					return bytes;
				}

			} catch (Exception) {
				throw;
			}
		}

		private static byte [] SerializeViaXml<T> (T obj, List<Type> knownTypes)
		{
			byte [] bytes = null;
			try {
				using (MemoryStream memoryStream = new MemoryStream ()) {
					var serializer = new DataContractSerializer (typeof (T), knownTypes);
					serializer.WriteObject (memoryStream, obj);

					bytes = memoryStream.ToArray ();
					memoryStream.Close ();

					return bytes;

				}
			} catch (Exception) {
				throw;
			}
		}
	}
}
