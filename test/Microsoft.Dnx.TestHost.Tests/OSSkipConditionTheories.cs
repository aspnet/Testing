// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Testing.xunit;
using Microsoft.Dnx.Runtime;
using Microsoft.Extensions.PlatformAbstractions;
using Xunit;

namespace Microsoft.Dnx.TestHost.Tests
{
    public class OSSkipConditionTheories
    {
        private IRuntimeEnvironment RuntimeEnvironment
        {
            get
            {
                return PlatformServices.Default.Runtime;
            }
        }

        [ConditionalTheory]
        [InlineData("linux")]
        [OSSkipCondition(OperatingSystems.Linux)]
        public void TestSkipLinux(string os)
        {
            Assert.False(
                os == RuntimeEnvironment.OperatingSystem.ToLowerInvariant(),
                "Test should not be running on Linux");
        }

        [ConditionalTheory]
        [InlineData("darwin")]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public void TestSkipMacOSX(string os)
        {
            Assert.False(
                os == RuntimeEnvironment.OperatingSystem.ToLowerInvariant(),
                "Test should not be running on MacOSX.");
        }

        [ConditionalTheory]
        [InlineData(1)]
        [OSSkipCondition(OperatingSystems.Windows, WindowsVersions.Win7, WindowsVersions.Win2008R2)]
        public void RunTest_DoesNotRunOnWin7OrWin2008R2(int number)
        {
            var osVersion = Environment.OSVersion.Version;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT
                && osVersion.Major == 6 && osVersion.Minor == 1)
            {
                Assert.False(true, "Test should not be running on Win7 or Win2008R2.");
            }
        }

        [ConditionalTheory]
        [InlineData("windows")]
        [OSSkipCondition(OperatingSystems.Windows)]
        public void TestSkipWindows(string os)
        {
            Assert.False(
                os == RuntimeEnvironment.OperatingSystem.ToLowerInvariant(),
                "Test should not be running on Windows.");
        }
    }
}
