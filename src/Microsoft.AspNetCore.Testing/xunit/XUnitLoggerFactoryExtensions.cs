// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Testing.xunit;
using Xunit.Abstractions;

namespace Microsoft.Extensions.Logging
{
    public static class XUnitLoggerFactoryExtensions
    {
        public static void AddXUnit(this ILoggerFactory self, ITestOutputHelper output)
        {
            self.AddProvider(new XUnitLoggerProvider(output));
        }

        public static void AddXUnit(this ILoggerFactory self, ITestOutputHelper output, LogLevel minLevel)
        {
            self.AddProvider(new XUnitLoggerProvider(output, minLevel));
        }
    }
}