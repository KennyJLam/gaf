using System.Reflection;
using System.Runtime.CompilerServices;

// Information about this assembly is defined by the following attributes.
// Change them to the values specific to your project.

[assembly: AssemblyTitle("Genetic Algorithm Framework for .Net")]
[assembly: AssemblyDescription("A simple to use GA framework for .Net (4.5 pcl)")]
[assembly: AssemblyConfiguration("6th July 2015")]
[assembly: AssemblyCompany("AI Frameworks")]
[assembly: AssemblyProduct("Genetic Algorithm Framework for .Net")]
[assembly: AssemblyCopyright("Copyright © John Newcombe 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion("2.1.0")]
[assembly: AssemblyFileVersion("2.1.0")]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

//used to give unit test access
[assembly: InternalsVisibleTo("GAF.UnitTests")]
[assembly: InternalsVisibleTo("GAF.Lab")]
[assembly: InternalsVisibleTo("GAF.NUnit")]

// ======================================================================================
// HISTORY
// ======================================================================================
// Version 2    Update to IOperator interface to include the 'Enabed' Property.
//              Change to the RandomReplace opertor to change from NumberToReplace to 
//              Percentage to replace.
// 2.0.1		Fixes (See NuGet)
// 2.0.2		Fixes (See NuGet)
// 2.0.3		Fixes (See NuGet)
// 2.1.0		Fixes
//				Refactoring of RandomReplace
//				Removal of percentages resolving to even numbers.
//2.1.1			Update of Assembly info.