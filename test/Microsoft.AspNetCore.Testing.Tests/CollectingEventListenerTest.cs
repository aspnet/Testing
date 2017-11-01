using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Testing.Tracing;
using Xunit;

namespace Microsoft.AspNetCore.Testing.Tests
{
    public class CollectingEventListenerTest
    {
        [Fact]
        public async Task ListenerCollectsEventsWrittenInCurrentAsyncContextDuringTest()
        {
            // We create a bunch of tasks in order to verify that when multiple async tasks are running,
            // the collector only captures events from the async context that initiated capturing of events.

            var tasks = new Task<IReadOnlyList<EventWrittenEventArgs>>[10];
            for (var i = 0; i < tasks.Length; i += 1)
            {
                tasks[i] = Task.Run(() =>
                {
                    return CollectingEventListener.CollectEvents(() =>
                    {
                        TestEventSource.Log.Test();
                        TestEventSource.Log.TestWithPayload(42, 4.2);
                    }, "Microsoft-AspNetCore-Testing-Test");
                });
            }

            var eventses = await Task.WhenAll(tasks);

            for (var i = 0; i < eventses.Length; i += 1)
            {
                EventAssert.Collection(eventses[i],
                    EventAssert.Event(1, "Test", EventLevel.Informational),
                    EventAssert.Event(2, "TestWithPayload", EventLevel.Verbose)
                        .Payload("payload1", 42)
                        .Payload("payload2", 4.2));
            }
        }
    }

    [EventSource(Name = "Microsoft-AspNetCore-Testing-Test")]
    public class TestEventSource : EventSource
    {
        public static readonly TestEventSource Log = new TestEventSource();

        private TestEventSource()
        {
        }

        [Event(eventId: 1, Level = EventLevel.Informational, Message = "Test")]
        public void Test() => WriteEvent(1);

        [Event(eventId: 2, Level = EventLevel.Verbose, Message = "Test")]
        public void TestWithPayload(int payload1, double payload2) => WriteEvent(2, payload1, payload2);
    }
}
