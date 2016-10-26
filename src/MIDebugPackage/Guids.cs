// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// MUST match guids.h

using System;

namespace Microsoft.MIDebugPackage
{
    internal static class GuidList
    {
        public const string guidMIDebugPackagePkgString = "6f32ce3b-080d-4165-a2c7-93bf58fccc6c";
        public const string guidMIDebugPackageCmdSetString = "cc8243ff-d5d9-47db-9e96-3fd6b94075da";

        public static readonly Guid guidMIDebugPackageCmdSet = new Guid(guidMIDebugPackageCmdSetString);
    };
}
