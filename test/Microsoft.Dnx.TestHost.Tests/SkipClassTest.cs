// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Testing.xunit;
using Xunit;

namespace Microsoft.Dnx.TestHost.Tests
{
    [FrameworkSkipCondition(RuntimeFrameworks.CLR | RuntimeFrameworks.CoreCLR | RuntimeFrameworks.Mono)]
    public class SkipClassTest
    {
        [ConditionalFact]
        public void Fact_always_skipped()
        {
            Assert.True(false, "This should always be skipped");
        }

        [ConditionalTheory]
        [InlineData(1)]
        public void Theory_always_skipped(int number)
        {
            Assert.True(false, "This should always be skipped");
        }
    }
}