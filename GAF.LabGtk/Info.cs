using System;
using System.Diagnostics;

namespace GAF.LabGtk
{
	public static class Info
	{
		public static FileVersionInfo GetFileVersionInfo ()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly ();
			return FileVersionInfo.GetVersionInfo (assembly.Location);
		}

	}
}

