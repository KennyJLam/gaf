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

namespace GAF.Network
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Versioning;
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

			if (_traceLevelSwitch.TraceInfo) {

				var header = new StringBuilder ();
				header.AppendLine ();
				header.AppendLine (_logLine);
				header.AppendLine (DateTime.Now.ToString ("dd-mmm-yyyy hh:mm:ss.ss"));
				header.AppendLine (string.Format ("Assembly Version: {0}", Assembly.GetExecutingAssembly ().GetName ().Version));
				header.AppendLine (string.Format ("Runtime Version: {0}", GetFrameworkVersion()));
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
			if (message == null)
				return;

			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			var formattedMessage = FormatMessage (message, "ERR");
			Trace.WriteLineIf (_traceLevelSwitch.TraceError, formattedMessage);
		}

		public static void Error (Exception exception)
		{
			if (exception == null)
				return;

			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();

			var formattedMessage = FormatMessage (exception.Message, "ERR");
			Trace.WriteLineIf (_traceLevelSwitch.TraceError, formattedMessage);

			if (_traceLevelSwitch.TraceVerbose) {
				var errorText = new StringBuilder ();

				errorText.AppendLine ();
				errorText.AppendLine (_logLine);
				errorText.AppendLine ("Exception: ");
				errorText.AppendLine (_logLine);
				errorText.AppendLine (string.Format ("Error: {0}", exception.Message));
				errorText.AppendLine (_logLine);
				errorText.AppendLine (exception.ToString ());

				errorText.AppendLine (_logLine);
				errorText.AppendLine ("Base Exception: ");
				errorText.AppendLine (_logLine);
				errorText.AppendLine (string.Format ("Error: {0}", exception.GetBaseException ().Message));
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
			if (message == null)
				return;
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			Trace.WriteLineIf (_traceLevelSwitch.TraceWarning, FormatMessage (message, "WARN"));
		}

		public static void Info (string message)
		{
			if (message == null)
				return;
			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			Trace.WriteLineIf (_traceLevelSwitch.TraceInfo, FormatMessage (message, "INFO"));
		}

		public static void Debug (string message)
		{
			if (message == null)
				return;

			if (_traceLevelSwitch == null) InitialiseTraceSwitch ();
			Trace.WriteLineIf (_traceLevelSwitch.TraceVerbose, FormatMessage (message, "DEBUG"));
		}

		public static void Debug (byte [] byteData)
		{
			if (byteData == null || byteData.Length == 0)
				return;

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

		private static string GetFrameworkVersion ()
		{
			//TODO: Move this to a resource file.
			var version = Environment.Version.ToString();
			                      
			var friendlyVersion = string.Empty;

			switch (version) {

			//Base 4.0

			case "4.0.30319.1": friendlyVersion = ".NET 4.0 RTM"; break;
			case "4.0.30319.269": friendlyVersion = ".NET 4.0 (with MS12 - 035 GDR security update)"; break;
			case "4.0.30319.276": friendlyVersion = ".NET 4.0 (4.0.3 Runtime update)"; break;
			case "4.0.30319.296": friendlyVersion = ".NET 4.0 (with MS12 - 074 GDR security update)"; break;
			case "4.0.30319.544": friendlyVersion = ".NET 4.0 (with MS12 - 035 LDR security update)"; break;
			case "4.0.30319.1008": friendlyVersion = ".NET 4.0 (with MS13 - 052 GDR security update)"; break;
			case "4.0.30319.1022": friendlyVersion = ".NET 4.0 (with MS14 - 009 GDR security update)"; break;
			case "4.0.30319.1026": friendlyVersion = ".NET 4.0 (with MS14 - 057 GDR security update)"; break;
			case "4.0.30319.2034": friendlyVersion = ".NET 4.0 (with MS14 - 009 LDR security update)"; break;

			//4.5

			case "4.0.30319.17626": friendlyVersion = ".NET 4.5 RC"; break;
			case "4.0.30319.17929": friendlyVersion = ".NET 4.5 RTM"; break;
			case "4.0.30319.18010": friendlyVersion = ".NET 4.5"; break;
			case "4.0.30319.18052": friendlyVersion = ".NET 4.5 [64 bit]"; break;
			case "4.0.30319.18063": friendlyVersion = ".NET 4.5 [64 bit] (with MS14 - 009 security update)"; break;

			//4.5.1

			case "4.0.30319.18408": friendlyVersion = ".NET 4.5.1 [64 bit]"; break;
			case "4.0.30319.18444": friendlyVersion = ".NET 4.5.1 [64 bit] (with MS14 - 009 security update)"; break;
			case "4.0.30319.34014": friendlyVersion = ".NET 4.5.1 [64 bit]"; break;

			//4.5.2

			case "4.0.30319.34209": friendlyVersion = ".NET 4.5.2 [64 bit]"; break;
			//case "4.0.30319.34209": friendlyVersion = ".NET 4.5.2 on Windows 8.1 64 - bit"; break;

			//4.6

			case "4.0.30319.42000": friendlyVersion = ".NET 4.6 [64 bit]"; break;

			}

			return string.Format("{0} {1}",version,friendlyVersion);

		}
	}
}

