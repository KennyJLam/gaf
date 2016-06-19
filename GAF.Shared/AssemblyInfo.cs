using System.Reflection;
using System.Runtime.CompilerServices;

// Information about this assembly is defined by the following attributes.
// Change them to the values specific to your project.

[assembly: AssemblyTitle("Genetic Algorithm Framework for .Net")]

[assembly: AssemblyConfiguration("2nd April 2016")]
[assembly: AssemblyCompany("AI Frameworks")]
[assembly: AssemblyCopyright("Copyright © John Newcombe 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

// Major: Large refactoring of re-writes.
// Minor: Standard release via NuGet (post v2.2.2, pre v2.2.2 was a little adhoc, sorry!)
// Build: Changes published to Git
// Build: 

[assembly: AssemblyVersion("2.2.4.*")]
[assembly: AssemblyFileVersion("2.2.4")]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

//used to give unit test access
[assembly: InternalsVisibleTo("GAF.UnitTests")]
[assembly: InternalsVisibleTo("GAF.Api")]

