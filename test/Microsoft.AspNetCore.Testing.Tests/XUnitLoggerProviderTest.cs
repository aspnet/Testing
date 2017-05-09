﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Testing.Tests
{
    public class XUnitLoggerProviderTest
    {
        [Fact]
        public void LoggerProviderWritesToTestOutputHelper()
        {
            var testTestOutputHelper = new TestTestOutputHelper();
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddXUnit(testTestOutputHelper);

            var logger = loggerFactory.CreateLogger("TestCategory");
            logger.LogInformation("This is some {0} information", "great");
            logger.LogTrace("This is some {0} information", "unimportant");

            var expectedOutput =
                "| TestCategory Information: This is some great information" + Environment.NewLine +
                "| TestCategory Trace: This is some unimportant information" + Environment.NewLine;

            Assert.Equal(expectedOutput, testTestOutputHelper.Output);
        }

        [Fact]
        public void LoggerProviderDoesNotWriteLogMessagesBelowMinimumLevel()
        {
            var testTestOutputHelper = new TestTestOutputHelper();
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddXUnit(testTestOutputHelper, LogLevel.Error);

            var logger = loggerFactory.CreateLogger("TestCategory");
            logger.LogInformation("This is some {0} information", "great");
            logger.LogError("This is a {0} error", "bad");

            Assert.Equal("| TestCategory Error: This is a bad error" + Environment.NewLine, testTestOutputHelper.Output);
        }

        [Fact]
        public void LoggerProviderPrependsPrefixToEachLine()
        {
            var testTestOutputHelper = new TestTestOutputHelper();
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddXUnit(testTestOutputHelper);

            var logger = loggerFactory.CreateLogger("TestCategory");
            logger.LogInformation("This is a" + Environment.NewLine + "multi-line" + Environment.NewLine + "message");

            var expectedOutput =
                "| TestCategory Information: This is a" + Environment.NewLine +
                "|                           multi-line" + Environment.NewLine +
                "|                           message" + Environment.NewLine;

            Assert.Equal(expectedOutput, testTestOutputHelper.Output);
        }

        private class TestTestOutputHelper : ITestOutputHelper
        {
            private StringBuilder _output = new StringBuilder();

            public string Output => _output.ToString();

            public void WriteLine(string message)
            {
                _output.AppendLine(message);
            }

            public void WriteLine(string format, params object[] args)
            {
                _output.AppendLine(string.Format(format, args));
            }
        }
    }
}
