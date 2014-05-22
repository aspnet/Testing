using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

namespace Xunit.KRunner
{
    internal static class ConfigurationExtensions
    {
        public static string Get(this IConfiguration configuration, string key, string defaultValue)
        {
            string val;
            return configuration.TryGet(key, out val) ? val : defaultValue;
        }

        public static T Get<T>(this IConfiguration configuration, string key, string defaultValue, Func<string, T> convert)
        {
            string val;
            return convert(configuration.TryGet(key, out val) ? val : defaultValue);
        }

        public static T Get<T>(this IConfiguration configuration, string key, Func<string, T> convert)
        {
            return Get(configuration, key, null, convert);
        }
    }
}
