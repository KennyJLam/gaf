using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace GAF.Network.Serialization
{
	public class Xml
	{

		/// <summary>
		/// Deserializes specified xml.
		/// </summary>
		/// <returns>The serialize.</returns>
		/// <param name="xml">Xml.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeSerialize<T> (string xml)
		{
			return DeSerialize<T> (xml, new List<Type> ());
		}

		/// <summary>
		/// Deserializes specified xml.
		/// </summary>
		/// <returns>The serialize new.</returns>
		/// <param name="byteData">Byte data.</param>
		/// <param name="knownTypes">Known types.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeSerialize<T> (string xml, List<Type> knownTypes)
		{
			using (var memoryStream = new MemoryStream (System.Text.Encoding.UTF8.GetBytes (xml))) {

				try {
					var serializer = new DataContractSerializer (typeof (T), knownTypes);
					return (T)serializer.ReadObject (memoryStream);

				} catch (Exception ex) {
					Log.Error (ex);
					Log.Error (xml);
				}

				memoryStream.Close ();
				return default (T);
			}

		}

		/// <summary>
		/// Serialize the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string Serialize<T> (T obj)
		{
			return Serialize<T> (obj, new List<Type> ());
		}

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="knownTypes">Known types.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string Serialize<T> (T obj, List<Type> knownTypes)
		{
			try {

				using (MemoryStream memStm = new MemoryStream ()) {

					var serializer = new DataContractSerializer (typeof (T), knownTypes);

					serializer.WriteObject (memStm, obj);

					memStm.Seek (0, SeekOrigin.Begin);

					using (var streamReader = new StreamReader (memStm)) {
						var xml = streamReader.ReadToEnd ();

						return xml;
					}
				}

			} catch (Exception) {
				throw;
			}
		}
	}
}

