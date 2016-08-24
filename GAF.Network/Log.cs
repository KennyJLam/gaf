using System;
namespace GAF.Network
{
	using System;
	using System.Diagnostics;
	using System.Text;

	public static class Logger
	{
		public static void Error (string message)
		{
			WriteEntry (message, "ERR");
		}

		public static void Error (Exception ex)
		{
			WriteEntry (ex.Message, "ERR");
		}

		public static void Warning (string message)
		{
			WriteEntry (message, "WARN");
		}

		public static void Info (string message)
		{
			WriteEntry (message, "INFO");
		}

		public static void Debug (string message)
		{
			WriteEntry (message, "DEBUG");
		}

		public static void Debug (byte [] byteData)
		{
			const int cols = 16;

			//storage for bytes and ascii representation
			var bytes = new StringBuilder ();
			var ascii = new StringBuilder ();

			//add the byte cout at the begining of the line
			bytes.Append (0.ToString ("X6"));
			bytes.Append (": ");

			for (int index = 0; index < byteData.Length; index++) {

				var byt = byteData [index];
				bytes.Append (byt.ToString ("X2"));
				bytes.Append (" ");

				//add the ascii representation if it is printable
				ascii.Append (byt > 32 ? Encoding.UTF8.GetString (byteData, index, 1) : ".");

				//check for the end of line
				if (index > 0 && (index + 1) % cols == 0) {

					//add the ascii to the line
					bytes.Append (ascii.ToString ());
					bytes.Append ("\r\n");

					//add the byte count at the begining of the next line
					bytes.Append (index.ToString ("X6"));
					bytes.Append (": ");

					ascii.Clear ();
				}

			}

			//tidy up the last line
			for (var index = 0; index < cols - ascii.Length; index++) {
				bytes.Append ("-- ");
			}

			bytes.Append (ascii.ToString ());
			bytes.Append ("\r\n");

			WriteEntry (bytes.ToString (), "DEBUG");
		}

		private static void WriteEntry (string message, string type)
		{
			Trace.WriteLine (
				string.Format ("{0} [{1}] {2}",
								  DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"),
								  type,
								  message));
		}
	}
}

