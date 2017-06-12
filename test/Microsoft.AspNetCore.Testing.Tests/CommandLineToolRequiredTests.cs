// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Testing.xunit;
using Xunit;

namespace Microsoft.AspNetCore.Testing
{
    public class CommandLineToolRequiredTests
    {
        [ConditionalFact]
        [CommandLineToolRequired("uname")]
        public void FindsLinuxCommandLineTool()
        {
            Assert.False(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        }

        [ConditionalFact]
        [CommandLineToolRequired("regedit")]
        public void FindsRegEditOnWindows()
        {
            Assert.True(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        }
    }
}
