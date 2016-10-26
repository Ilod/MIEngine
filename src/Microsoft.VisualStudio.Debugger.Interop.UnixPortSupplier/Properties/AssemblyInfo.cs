// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.VisualStudio.Debugger.Interop.UnixPortSupplier.DesignTime")]
[assembly: AssemblyDescription("Design time (reference) assembly for the Microsoft.VisualStudio.Debugger.Interop.UnixPortSupplier interfaces")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en")]

[assembly: Guid("c8266803-1eff-4f07-8c5e-f600e6a7001c")]

// Add the PrimaryInteropAssemblyAttribute so that this assembly can be used with 'Embed Interop Types'
[assembly: System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute(1, 0)]

#if CORECLR
// There is no portable definition for PrimaryInteropAssemblyAttribute, so define our own here to make the C# compiler happy
namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    [System.Runtime.InteropServices.ComVisible(true)]
    internal sealed class PrimaryInteropAssemblyAttribute : Attribute
    {
        internal int _major;
        internal int _minor;

        public PrimaryInteropAssemblyAttribute(int major, int minor)
        {
            _major = major;
            _minor = minor;
        }

        public int MajorVersion { get { return _major; } }
        public int MinorVersion { get { return _minor; } }
    }
}
#endif
