﻿using System.Collections.Generic;
using Xunit.Abstractions;

namespace Xunit
{
    public class TestFrameworkOptions : ITestFrameworkOptions
    {
        readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public TValue GetValue<TValue>(string name, TValue defaultValue)
        {
            object result;
            if (properties.TryGetValue(name, out result))
                return (TValue)result;

            return defaultValue;
        }

        public void SetValue<TValue>(string name, TValue value)
        {
            properties[name] = value;
        }
    }
}
