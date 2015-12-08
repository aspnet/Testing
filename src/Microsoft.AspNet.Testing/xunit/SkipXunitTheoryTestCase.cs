// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNet.Testing.xunit
{
    internal class SkipXunitTheoryTestCase : XunitTheoryTestCase
    {
        public SkipXunitTheoryTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, ITestMethod testMethod)
            : base(diagnosticMessageSink, defaultMethodDisplay, testMethod)
        {
        }

        protected override string GetSkipReason(IAttributeInfo factAttribute) => SkipXunitTestCase.EvaluateSkipConditions(TestMethod) ?? base.GetSkipReason(factAttribute);
    }
}
