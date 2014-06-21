using System;
using System.Collections.Generic;
using System.Threading;
using Xunit.Abstractions;

namespace Xunit.KRunner
{
    internal class ProxyMessageSink : IMessageSink, IDisposable
    {
        private readonly IMessageSink _inner;

        public ProxyMessageSink(IMessageSink inner)
        {
            _inner = inner;
            Finished = new ManualResetEvent(initialState: false);
            DiscoveryComplete = new ManualResetEvent(initialState: false);
            TestCases = new List<ITestCase>();
        }

        /// <summary>
        /// This event is trigged when the test assembly finished message has been seen.
        /// </summary>
        public ManualResetEvent Finished { get; private set; }

        /// <summary>
        /// This event is trigged when the discovery complete message has been seen.
        /// </summary>
        /// <returns></returns>
        public ManualResetEvent DiscoveryComplete { get; private set; }

        /// <summary>
        /// The discovered test cases
        /// </summary>
        /// <returns></returns>
        public List<ITestCase> TestCases { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            _inner.Dispose();
            ((IDisposable)Finished).Dispose();
            ((IDisposable)DiscoveryComplete).Dispose();
        }

        /// <inheritdoc/>
        public bool OnMessage(IMessageSinkMessage message)
        {
            var result = _inner.OnMessage(message);

            if (message is ITestAssemblyFinished)
                Finished.Set();
            else if (message is IDiscoveryCompleteMessage)
                DiscoveryComplete.Set();
            else if (message is ITestCaseDiscoveryMessage)
                TestCases.Add(((ITestCaseDiscoveryMessage)message).TestCase);

            return result;
        }
    }
}
