using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;
using Xunit.Abstractions;
using Xunit.ConsoleClient;

namespace Xunit.KRunner
{
    public class TestingContext
    {
        readonly object _lock = new object();

        private bool? _parallelize;
        private int? _maxThreads;

        public object ConsoleLock { get { return _lock; } }

        public string ApplicationName { get; set; }

        public IServiceProvider Services { get; set; }
        public IConfiguration Configuration { get; set; }

        public string VisitorName { get; set; }
        public IMessageSink Visitor { get; set; }

        public bool Parallelize
        {
            get
            {
                if (!_parallelize.HasValue)
                    _parallelize = Configuration.Get("parallel", ParseParallelize);
                return _parallelize.Value;
            }

            set
            {
                _parallelize = value;
            }
        }

        public int MaxThreads
        {
            get
            {
                if (!_maxThreads.HasValue)
                    _maxThreads = Configuration.Get("maxthreads", "0", int.Parse);
                return _maxThreads.Value;
            }
        }

        static bool ParseParallelize(string value)
        {
            if (value == null)
                return true;

            ParallelismOption parallelismOption;
            if (!Enum.TryParse<ParallelismOption>(value, out parallelismOption))
                throw new ArgumentException("incorrect argument value for parallel");

            switch (parallelismOption)
            {
                case ParallelismOption.all:
                case ParallelismOption.collections:
                    return true;

                case ParallelismOption.none:
                default:
                    return false;
            }
        }
    }
}
