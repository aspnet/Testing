using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Testing.Tracing;
using Xunit;

namespace Microsoft.AspNetCore.Testing.Tests
{
    public class CollectingEventListenerTest
    {
        [Fact]
        public void ListenerCollectsEventsWrittenDuringTest()
        {
            var listener = new CollectingEventListener("Microsoft-AspNetCore-Testing-Test");

            TestEventSource.Log.Test();
            TestEventSource.Log.TestWithPayload(42, 4.2);

            // Get the events written to the listener
            var events = listener.GetEventsWrittenTo("Microsoft-AspNetCore-Testing-Test");

            Assert.Collection(events,
                evt =>
                {
                    Assert.Equal(1, evt.EventId);
                    Assert.Equal("Test", evt.EventName);
                },
                evt =>
                {
                    Assert.Equal(2, evt.EventId);
                    Assert.Equal("TestWithPayload", evt.EventName);
                    Assert.Equal(evt.PayloadNames, new[] {
                        "payload1",
                        "payload2"
                    });
                    Assert.Equal(evt.Payload, new object[]
                    {
                        42,
                        4.2
                    });
                });
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

        [Event(eventId: 2, Level = EventLevel.Informational, Message = "Test")]
        public void TestWithPayload(int payload1, double payload2) => WriteEvent(2, payload1, payload2);
    }
}
