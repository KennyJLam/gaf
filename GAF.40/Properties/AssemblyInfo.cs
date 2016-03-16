using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Genetic Algorithm Framework for .Net")]
[assembly: AssemblyDescription("A simple to use GA framework for .Net (4.0)")]
[assembly: AssemblyConfiguration("6th July 2015")]
[assembly: AssemblyCompany("AI Frameworks")]
[assembly: AssemblyProduct("Genetic Algorithm Framework for .Net")]
[assembly: AssemblyCopyright("Copyright © John Newcombe 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//used to give unit test access
[assembly: InternalsVisibleTo("GAF.Test")]
[assembly: InternalsVisibleTo("GAF.UnitTests")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c488de07-8a6f-4a10-b6b3-05a00d866c8b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.1.1")]
[assembly: AssemblyFileVersion("2.1.1")]

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
