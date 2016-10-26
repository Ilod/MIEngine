// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.MIDebugEngine
{
    static public class EngineConstants
    {
        /// <summary>
        /// This is the engine GUID of the engine. It needs to be changed here and in the registration
        /// when creating a new engine.
        /// </summary>
        public static readonly Guid EngineId = new Guid("{d8c0e0a5-20b9-46fd-a041-14853ecaadba}");

        public static readonly Guid GdbEngine = new Guid("{7767d3e7-873a-4ba9-86f7-527a2776c453}");
        public static readonly Guid ClrdbgEngine = new Guid("{8af0c4d0-55a6-4b51-87cb-d7a533035913}");
    }
}
