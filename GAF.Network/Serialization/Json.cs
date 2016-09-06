using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace GAF.Network.Serialization
{
	public static class Json
	{

		/// <summary>
		/// Deserializes specified json.
		/// </summary>
		/// <returns>The serialize.</returns>
		/// <param name="json">Json.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeSerialize<T> (string json)
		{
			return DeSerialize<T> (json, new List<Type> ());
		}

		/// <summary>
		/// Deserializes specified json.
		/// </summary>
		/// <returns>The serialize.</returns>
		/// <param name="json">Json.</param>
		/// <param name="knownTypes">Known types.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeSerialize<T> (string json, List<Type> knownTypes)
		{
			using (var memoryStream = new MemoryStream (System.Text.Encoding.UTF8.GetBytes (json))) {

				try {
					var serializer = new DataContractJsonSerializer (typeof (T), knownTypes);
					return (T)serializer.ReadObject (memoryStream);

				} catch (Exception ex) {
					Log.Error (ex);
					Log.Error (json);
				}

				memoryStream.Close ();
				return default (T);
			}
		}

		/// <summary>
		/// Serialises to json and returns a UTF8 Json string.
		/// </summary>
		/// <returns>The to json.</returns>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string Serialize<T> (object obj)
		{
			return Serialize<T> (obj, new List<Type> ());
		}

		/// <summary>
		/// Serialises to json and returns a UTF8 Json string.
		/// </summary>
		/// <returns>The to json.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="knownTypes">Known types.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string Serialize<T> (object obj, List<Type> knownTypes)
		{
			try {
				using (MemoryStream memoryStream = new MemoryStream ()) {

					DataContractJsonSerializer serializer = new DataContractJsonSerializer (typeof (T), knownTypes);
					serializer.WriteObject (memoryStream, (T)obj);

					memoryStream.Position = 0;

					using (var sr = new StreamReader (memoryStream, System.Text.Encoding.UTF8)) {

						var json = sr.ReadToEnd ();

						memoryStream.Close ();

						return json;
					}
				}
			} catch (Exception) {
				throw;
			}

		}
	}
}

