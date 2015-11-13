// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Testing.xunit;

namespace Microsoft.AspNet.Testing.xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EnvironmentVariableSkipConditionAttribute : Attribute, ITestCondition
    {
        private readonly string _environmentVariableName;
        private readonly string _environmentVariableValue;

        public EnvironmentVariableSkipConditionAttribute(string environmentVariableName, string environmentVariableValue)
        {
            _environmentVariableName = environmentVariableName;
            _environmentVariableValue = environmentVariableValue;
            SkipReason = $"Test cannot run when environment variable {environmentVariableName} is set to {environmentVariableValue}";
        }

        public bool IsMet
        {
            get
            {
                var skip = _environmentVariableValue.Equals(Environment.GetEnvironmentVariable(_environmentVariableName));
                return !skip;
            }
        }

        public string SkipReason { get; set; }
    }
}