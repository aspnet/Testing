// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Framework.ConfigurationModel;

namespace Xunit.ConsoleClient
{
    public class CommandLine : BaseConfigurationSource
    {
        readonly Stack<string> arguments = new Stack<string>();

        protected CommandLine(string[] args)
        {
            for (int i = args.Length - 1; i >= 0; i--)
                arguments.Push(args[i]);

            Parse();
        }

        static void GuardNoOptionValue(KeyValuePair<string, string> option)
        {
            if (option.Value != null)
                throw new ArgumentException(String.Format("error: unknown command line option: {0}", option.Value));
        }

        public static CommandLine Parse(params string[] args)
        {
            return new CommandLine(args);
        }

        protected virtual void Parse()
        {
            while (arguments.Count > 0)
            {
                var option = PopOption(arguments);
                var optionName = option.Key.ToLowerInvariant();

                switch (optionName)
                {
                    case "-maxthreads":
                        if (option.Value == null)
                            throw new ArgumentException("missing argument for -maxthreads");

                        int threadValue;
                        if (!Int32.TryParse(option.Value, out threadValue) || threadValue < 0)
                            throw new ArgumentException("incorrect argument value for -maxthreads");

                        if (Data.ContainsKey("maxthreads"))
                            throw new ArgumentException("-maxthreads specified more than once");

                        Data.Add("maxthreads", option.Value);
                        break;

                    case "-parallel":
                        if (option.Value == null)
                            throw new ArgumentException("missing argument for -parallel");

                        ParallelismOption parallelismOption;
                        if (!Enum.TryParse<ParallelismOption>(option.Value, out parallelismOption))
                            throw new ArgumentException("incorrect argument value for -parallel");

                        if (Data.ContainsKey("parallel"))
                            throw new ArgumentException("-parallel specified more than once");

                        Data.Add("parallel", option.Value);
                        break;

                    case "-teamcity":
                        GuardNoOptionValue(option);

                        if (Data.ContainsKey("teamcity"))
                            throw new ArgumentException("-teamcity specified more than once");

                        Data.Add("teamcity", "true");
                        break;

                    case "-visitor":
                        if (option.Value == null)
                            throw new ArgumentException("missing argument for -visitor");

                        if (Data.ContainsKey("visitor"))
                            throw new ArgumentException("-visitor specified more than once");

                        Data.Add("visitor", option.Value);
                        break;

                    default:
                        throw new ArgumentException(String.Format("unknown command line option: {0}", option.Key));
                }
            }
        }

        static KeyValuePair<string, string> PopOption(Stack<string> arguments)
        {
            string option = arguments.Pop();
            string value = null;

            if (arguments.Count > 0 && !arguments.Peek().StartsWith("-"))
                value = arguments.Pop();

            return new KeyValuePair<string, string>(option, value);
        }
    }
}
