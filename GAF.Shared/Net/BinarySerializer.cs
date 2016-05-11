using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GAF.SocketServer
{
	public static class BinarySerializer
	{

		public static T DeSerialize<T> (byte[] byteData)
		{

			var binaryFormatter = new BinaryFormatter ();
			var memoryStream = new MemoryStream (byteData, 0, byteData.Length);

			var chromosome = (T)binaryFormatter.Deserialize (memoryStream);
			memoryStream.Close ();

			return chromosome;

		}

		public static byte[] Serialize<T> (object obj)
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
			var memoryStream = new MemoryStream ();

			binaryFormatter.Serialize (memoryStream, (T)obj);
			memoryStream.Seek (0, SeekOrigin.Begin);

			var buffer = memoryStream.GetBuffer ();
			memoryStream.Close ();

			return buffer;
		}
	}
}

