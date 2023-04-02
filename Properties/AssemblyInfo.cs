using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using TheLongDebugThing;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TheLongFlipper")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("TheLongFlipper")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: MelonInfo(typeof(DebugThing), "TheLongDebugThing", "0.0.1", "Filigrani")]
[assembly: MelonGame("Hinterland", "TheLongDark")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f5807e6c-14a5-42bb-b753-438ec8863f18")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
//This is a C# comment. Comments have no impact on compilation.

[assembly: AssemblyVersion(BuildInfo.ModVersion)]
[assembly: AssemblyFileVersion(BuildInfo.ModVersion)]

internal static class BuildInfo
{
	internal const string ModName = "Name";
	internal const string ModAuthor = "Author";
	/// <summary>
	/// Version numbers in C# are a set of 1 to 4 positive integers separated by periods.
	/// Mods typically use 3 numbers. For example: 1.2.1
	/// </summary>
	internal const string ModVersion = "1.0.0";
}