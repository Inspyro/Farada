using System;
using System.Reflection;
# if !DISABLE_NULL_GUARD
using NullGuard;

#endif

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//

[assembly: AssemblyConfiguration ("Debug")]
[assembly: AssemblyCompany ("rubicon IT GmbH")]
[assembly: AssemblyProduct ("Register Nova Shared: Farada")]
[assembly: AssemblyCopyright ("Copyright (c) rubicon IT GmbH, www.rubicon.eu")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyInformationalVersion ("0.0.0.1")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion ("0.0.0.1")]
[assembly: AssemblyFileVersion ("0.0.0.1")]

//

# if !DISABLE_NULL_GUARD

[assembly: NullGuard (
#if DEBUG
    ValidationFlags.Arguments | ValidationFlags.NonPublic
#else
  ValidationFlags.Arguments
#endif
    )]

#endif