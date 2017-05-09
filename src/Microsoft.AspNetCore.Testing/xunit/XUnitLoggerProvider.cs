﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Testing.xunit
{
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private ITestOutputHelper _output;
        private LogLevel _minLevel;

        public XUnitLoggerProvider(ITestOutputHelper output)
            : this(output, LogLevel.Trace)
        {
        }

        public XUnitLoggerProvider(ITestOutputHelper output, LogLevel minLevel)
        {
            _output = output;
            _minLevel = minLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(_output, categoryName, _minLevel);
        }

        public void Dispose()
        {
        }
    }

    public class XUnitLogger : ILogger, IDisposable
    {
        private static readonly char[] NewLineChars = new[] { '\r', '\n' };
        private readonly string _category;
        private readonly LogLevel _minLogLevel;
        private readonly ITestOutputHelper _output;
        private bool _disposed;

        public XUnitLogger(ITestOutputHelper output, string category, LogLevel minLogLevel)
        {
            _minLogLevel = minLogLevel;
            _category = category;
            _output = output;
        }

        public void Log<TState>(
            LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var firstLinePrefix = $"| {_category} {logLevel}: ";
            var lines = formatter(state, exception).Split('\n');
            _output.WriteLine(firstLinePrefix + lines.First().TrimEnd(NewLineChars));

            var additionalLinePrefix = "|" + new string(' ', firstLinePrefix.Length - 1);
            foreach (var line in lines.Skip(1))
            {
                _output.WriteLine(additionalLinePrefix + line.TrimEnd(NewLineChars));
            }
        }

        public bool IsEnabled(LogLevel logLevel)
            => logLevel >= _minLogLevel && !_disposed;

        public IDisposable BeginScope<TState>(TState state)
            => new NullScope();

        private class NullScope : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
