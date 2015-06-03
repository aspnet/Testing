// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Common.DependencyInjection;

namespace Microsoft.Framework.TestHost
{
    public class ProjectCommand
    {
        public static async Task<int> Execute(
            IServiceProvider services, 
            Project project,
            string[] args)
        {
            var oldEnvironment = (IApplicationEnvironment)services.GetService(typeof(IApplicationEnvironment));

            var environment = new ApplicationEnvironment(
                project, 
                oldEnvironment.RuntimeFramework, 
                oldEnvironment.Configuration);

            var newServices = new ServiceProvider(services);
            newServices.Add(typeof(IApplicationEnvironment), environment);

            var applicationHost = new Microsoft.Framework.ApplicationHost.Program(
                (IAssemblyLoaderContainer)services.GetService(typeof(IAssemblyLoaderContainer)),
                environment,
                newServices);

            var options = (IRuntimeOptions)services.GetService(typeof(IRuntimeOptions));
            if (options.CompilationServerPort.HasValue)
            {
                args = new string[]
                {
                    "--port",
                    options.CompilationServerPort.Value.ToString()
                }
                .Concat(args).ToArray();
            }

            return await applicationHost.Main(args);
        }
    }
}