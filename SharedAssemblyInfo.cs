using System.Reflection;

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

// Shared (File) Version
[assembly: AssemblyFileVersion("1.0.10.1")]
[assembly: AssemblyCompany("Usagirei")]
[assembly: AssemblyCopyright("Copyright © Usagirei 2016")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("")]
#endif

// ThemeEditor.Common
#if COMMON
[assembly: AssemblyVersion("1.0.10.0")]
#endif

// ThemeEditor.WPF
#if WPF_EDITOR
[assembly: AssemblyVersion("1.0.10.1")]
#endif