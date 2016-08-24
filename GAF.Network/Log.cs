using System;
namespace GAF.Network
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;

	public static class Log
	{
		private const string _logLine = "------------------------------------------------------------------";
		private static TraceSwitch _traceLevelSwitch;

		private static void InitialiseTraceSwitch ()
		{
			_traceLevelSwitch = new TraceSwitch ("TraceLevelSwitch", "General Trace Level Switch.");
		}

		/// <summary>
		/// Writes a header to the Trace File which contains diagnostic information. Typically this would
		/// be called only once at the begining of the program execution.
		/// </summary>
		public static void WriteHeader ()
		{

			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();

			if (_traceLevelSwitch.TraceVerbose) {

				var header = new StringBuilder ();
				header.AppendLine ();
				header.AppendLine (_logLine);
				header.AppendLine (DateTime.Now.ToString ("dd-mmm-yyyy hh:mm:ss.ss"));
				header.AppendLine (string.Format ("Application Version: {0}", Environment.Version.ToString ()));
				header.AppendLine (string.Format ("Current Path: {0}", Environment.CommandLine));
				header.AppendLine (string.Format ("Machine Name: {0}", Environment.MachineName));
				header.AppendLine (string.Format ("Operating System: {0}", Environment.OSVersion.ToString ()));
				header.AppendLine (string.Format ("Working Set: {0}", Environment.WorkingSet.ToString (CultureInfo.InvariantCulture)));
				header.AppendLine (string.Format ("Domain: {0}", Environment.UserDomainName.ToString (CultureInfo.InvariantCulture)));
				header.AppendLine (string.Format ("User: {0}", Environment.UserName.ToString (CultureInfo.InvariantCulture)));
				header.AppendLine (_logLine);

				Trace.WriteLine (header.ToString ());
			}
		}
		public static void Error (string message)
		{
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			var formattedMessage = FormatMessage (message, "ERR");
			Trace.WriteLineIf (_traceLevelSwitch.TraceError, formattedMessage);
		}

		public static void Error (Exception exception)
		{
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();

			var formattedMessage = FormatMessage (exception.Message, "ERR");
			Trace.WriteLineIf (_traceLevelSwitch.TraceError, formattedMessage);

			if (_traceLevelSwitch.TraceVerbose) {
				var errorText = new StringBuilder ();

				errorText.AppendLine ();
				errorText.AppendLine (_logLine);
				errorText.AppendLine ("Exception: ");
				errorText.AppendLine (_logLine);
				errorText.AppendLine (string.Format("Error: {0}", exception.Message));
				errorText.AppendLine (_logLine);
				errorText.AppendLine (exception.ToString ());

				errorText.AppendLine (_logLine);
				errorText.AppendLine ("Base Exception: ");
				errorText.AppendLine (_logLine);
				errorText.AppendLine (string.Format("Error: {0}", exception.GetBaseException ().Message));
				errorText.AppendLine (_logLine);
				errorText.AppendLine (exception.GetBaseException ().ToString ());

				errorText.AppendLine (exception.GetBaseException ().TargetSite.ToString ());
				errorText.AppendLine (_logLine);
				errorText.AppendLine ("StackTrace: ");
				errorText.AppendLine (_logLine);
				errorText.AppendLine (exception.StackTrace);

				errorText.AppendLine (_logLine);

				Trace.WriteLine (errorText.ToString ());
			}
		}

		public static void Warning (string message)
		{
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			Trace.WriteLineIf (_traceLevelSwitch.TraceWarning, FormatMessage (message, "WARN"));
		}

		public static void Info (string message)
		{
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			Trace.WriteLineIf (_traceLevelSwitch.TraceInfo, FormatMessage (message, "INFO"));
		}

		public static void Debug (string message)
		{
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			Trace.WriteLineIf (_traceLevelSwitch.TraceVerbose, FormatMessage (message, "DEBUG"));
		}

		public static void Debug (byte [] byteData)
		{
			const int cols = 16;

			if (_traceLevelSwitch.TraceVerbose) {

				//storage for bytes and ascii representation
				var bytes = new StringBuilder ();
				var ascii = new StringBuilder ();

				//add the byte cout at the begining of the line
				bytes.AppendLine ();
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
						bytes.AppendLine ();

						//add the byte count at the begining of the next line
						bytes.Append ((index + 1).ToString ("X6"));
						bytes.Append (": ");

						ascii.Clear ();
					}
				}

				//tidy up the last line
				for (var index = 0; index < cols - ascii.Length; index++) {
					bytes.Append ("-- ");
				}

				bytes.Append (ascii.ToString ());
				bytes.AppendLine ();

				if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
				var formattedMessage = FormatMessage (bytes.ToString (), "DEBUG");
				Trace.WriteLine (formattedMessage);
			}
		}

		private static string FormatMessage (string message, string type)
		{
			return string.Format ("{0} [{1}] {2}",
					  DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"),
					  type, message);
		}
	}
}

