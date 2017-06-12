// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Testing.xunit;
using Xunit;

namespace Microsoft.AspNetCore.Testing
{
    public class DockerTests
    {
        [ConditionalFact]
        [DockerOnly]
        public void DoesNotRunOnWindows()
        {
            Assert.False(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        }

        [ConditionalFact]
        [DockerOnly]
        public void RunsFromDockerHost()
        {
            Assert.False(true, "'docker' should not be available within the docker container.");
        }
    }
}
