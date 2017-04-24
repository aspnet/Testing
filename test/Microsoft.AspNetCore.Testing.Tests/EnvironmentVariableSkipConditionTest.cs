// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace Microsoft.AspNetCore.Testing.xunit
{
    public class EnvironmentVariableSkipConditionTest
    {
        private readonly string _skipReason = "Test skipped on environment variable with name '{0}' and value '{1}'";

        [Theory]
        [InlineData("false")]
        [InlineData("")]
        [InlineData(null)]
        public void IsMet_DoesNotMatch(string environmentVariableValue)
        {
            // Arrange
            var attribute = new EnvironmentVariableSkipConditionAttribute(
                new TestEnvironmentVariable(environmentVariableValue),
                "Run",
                "true");

            // Act
            var isMet = attribute.IsMet;

            // Assert
            Assert.False(isMet);
        }

        [Theory]
        [InlineData("True")]
        [InlineData("TRUE")]
        [InlineData("true")]
        public void IsMet_DoesCaseInsensitiveMatch_OnValue(string environmentVariableValue)
        {
            // Arrange
            var attribute = new EnvironmentVariableSkipConditionAttribute(
                new TestEnvironmentVariable(environmentVariableValue),
                "Run",
                "true");

            // Act
            var isMet = attribute.IsMet;

            // Assert
            Assert.True(isMet);
            Assert.Equal(
                string.Format(_skipReason, "Run", environmentVariableValue),
                attribute.SkipReason);
        }

        [Fact]
        public void IsMet_DoesSuccessfulMatch_OnNull()
        {
            // Arrange
            var attribute = new EnvironmentVariableSkipConditionAttribute(
                new TestEnvironmentVariable(null),
                "Run",
                "true", null); // skip the test when the variable 'Run' is explicitly set to 'true' or is null (default)

            // Act
            var isMet = attribute.IsMet;

            // Assert
            Assert.True(isMet);
            Assert.Equal(
                string.Format(_skipReason, "Run", "(null)"),
                attribute.SkipReason);
        }

        [Theory]
        [InlineData("false")]
        [InlineData("")]
        [InlineData(null)]
        public void IsMet_MatchesOnMultipleSkipValues(string environmentVariableValue)
        {
            // Arrange
            var attribute = new EnvironmentVariableSkipConditionAttribute(
                new TestEnvironmentVariable(environmentVariableValue),
                "Run",
                "false", "", null);

            // Act
            var isMet = attribute.IsMet;

            // Assert
            Assert.True(isMet);
        }

        private struct TestEnvironmentVariable : IEnvironmentVariable
        {
            public TestEnvironmentVariable(string value)
            {
                Value = value;
            }

            public string Value { get; private set; }

            public string Get(string name)
            {
                return Value;
            }
        }
    }
}
