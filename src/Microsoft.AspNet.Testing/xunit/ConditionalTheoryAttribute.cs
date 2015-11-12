// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.AspNet.Testing.xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Microsoft.AspNet.Testing.xunit.ConditionalTheoryDiscoverer", "Microsoft.AspNet.Testing")]
    public class ConditionalTheoryAttribute : TheoryAttribute
    {
    }
}
