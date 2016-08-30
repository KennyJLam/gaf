using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace GAF.Api
{
	public static class Info
	{
		public static FileVersionInfo GetFileVersionInfo() {
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly ();
			return FileVersionInfo.GetVersionInfo (assembly.Location);
		}

	}
}

