// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !NET46

using System;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;

namespace Microsoft.AspNet.Testing.xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FrameworkSkipConditionAttribute : Attribute, ITestCondition
    {
        private readonly RuntimeFrameworks _excludedFrameworks;

        public FrameworkSkipConditionAttribute(RuntimeFrameworks excludedFrameworks)
        {
            _excludedFrameworks = excludedFrameworks;
        }

        public bool IsMet
        {
            get
            {
                return CanRunOnThisFramework(_excludedFrameworks);
            }
        }

        public string SkipReason
        {
            get
            {
                return "Test cannot run on this runtime framework.";
            }
        }

        private bool CanRunOnThisFramework(RuntimeFrameworks excludedFrameworks)
        {
            if (excludedFrameworks == RuntimeFrameworks.None)
            {
                return true;
            }

            if (excludedFrameworks.HasFlag(RuntimeFrameworks.Mono) &&
                TestPlatformHelper.IsMono)
            {
                return false;
            }

            if (excludedFrameworks.HasFlag(RuntimeFrameworks.CLR) &&
                TestPlatformHelper.RuntimeEnvironment.RuntimeType.Equals("CLR", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (excludedFrameworks.HasFlag(RuntimeFrameworks.CoreCLR) &&
                TestPlatformHelper.RuntimeEnvironment.RuntimeType.Equals("CoreCLR", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}

#endif
