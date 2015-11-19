// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Testing.xunit;
using Xunit;

namespace Microsoft.AspNet.Testing.Tests
{
    public class EnvironmentVariableSkipConditionAttributeTest
    {
        [Fact]
        public void Skips_WhenEnvironmentVariableIsSetToValue()
        {
            // Arrange
            var environmentVariableName = "ATTRIBUTE_TEST_ENV_VAR";
            var environmentVariableValue = "Value";
            Environment.SetEnvironmentVariable(environmentVariableName, environmentVariableValue);

            // Act
            var attribute = new EnvironmentVariableSkipConditionAttribute(environmentVariableName, environmentVariableValue);

            // Assert
            Assert.False(attribute.IsMet);

            Environment.SetEnvironmentVariable(environmentVariableName, null);
        }

        [Fact]
        public void DoesNotSkip_WhenEnvironmentVariableIsNotSet()
        {
            // Arrange
            var environmentVariableName = "ATTRIBUTE_TEST_ENV_VAR";
            Environment.SetEnvironmentVariable(environmentVariableName, null);

            // Act
            var attribute = new EnvironmentVariableSkipConditionAttribute(environmentVariableName, "Value");

            // Assert
            Assert.True(attribute.IsMet);

            Environment.SetEnvironmentVariable(environmentVariableName, null);
        }

        [Fact]
        public void DoesNotSkip_WhenEnvironmentVariableIsSetToDifferentValue()
        {
            // Arrange
            var environmentVariableName = "ATTRIBUTE_TEST_ENV_VAR";
            Environment.SetEnvironmentVariable(environmentVariableName, "Other value");

            // Act
            var attribute = new EnvironmentVariableSkipConditionAttribute(environmentVariableName, "Value");
            
            // Assert
            Assert.True(attribute.IsMet);

            Environment.SetEnvironmentVariable(environmentVariableName, null);
        }
    }
}
