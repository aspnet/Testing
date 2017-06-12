// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Testing.xunit
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CommandLineToolRequiredAttribute : Attribute, ITestCondition
    {
        private readonly static string[] _searchPaths;
        private readonly static string[] _executableExtensions;

        static CommandLineToolRequiredAttribute()
        {
            _searchPaths = Environment
                .GetEnvironmentVariable("PATH")
                .Split(Path.PathSeparator)
                .Select(p => p.Trim('"'))
                .ToArray();

            _executableExtensions = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Environment.GetEnvironmentVariable("PATHEXT")
                    .Split(';')
                    .Select(e => e.ToLower().Trim('"'))
                    .ToArray()
                : new [] { string.Empty };
        }

        public CommandLineToolRequiredAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; set; }

        public string SkipReason => $"The command line tool '{CommandName}' could not be found on the environment PATH.";

        public bool IsMet =>
            _searchPaths.Join(_executableExtensions,
                p => true, s => true,
                (p, s) => Path.Combine(p, CommandName + s))
            .Any(File.Exists);
    }
}
