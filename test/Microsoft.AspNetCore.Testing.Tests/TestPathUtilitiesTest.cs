﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Xunit;

namespace Microsoft.AspNetCore.Testing
{
    public class TestPathUtilitiesTest
    {
        [Fact]
        public void GetSolutionRootDirectory_ResolvesSolutionRoot()
        {
            // Directory.GetCurrentDirectory() gives:
            // Testing\test\Microsoft.AspNetCore.Testing.Tests\bin\Debug\netcoreapp2.0
            // Testing\test\Microsoft.AspNetCore.Testing.Tests\bin\Debug\net461
            var expectedPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", ".."));

            Assert.Equal(expectedPath, TestPathUtilities.GetSolutionRootDirectory("Testing"));
        }

        [Fact]
        public void GetSolutionRootDirectory_Throws_IfNotFound()
        {
            var exception = Assert.Throws<Exception>(() => TestPathUtilities.GetSolutionRootDirectory("NotTesting"));
            Assert.Equal($"Solution file NotTesting.sln could not be found in {AppContext.BaseDirectory} or its parent directories.", exception.Message);
        }
    }
}
