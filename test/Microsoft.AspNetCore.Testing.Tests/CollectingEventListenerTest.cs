using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Testing.Tracing;
using Xunit;

namespace Microsoft.AspNetCore.Testing.Tests
{
    // We are verifying here that when event listener tests are spread among multiple classes, they still
    // work, even when run in parallel, as long as one
    public class CollectingEventListenerTests
    {
        public abstract class TestBase : EventSourceTestBase
        {
            [Fact]
            public void CollectingEventListenerTest()
            {
                CollectFrom("Microsoft-AspNetCore-Testing-Test");

                TestEventSource.Log.Test();
                TestEventSource.Log.TestWithPayload(42, 4.2);

                var events = GetEvents();
                EventAssert.Collection(events,
                    EventAssert.Event(1, "Test", EventLevel.Informational),
                    EventAssert.Event(2, "TestWithPayload", EventLevel.Verbose)
                        .Payload("payload1", 42)
                        .Payload("payload2", 4.2));
            }
        }

        public class A : TestBase { }
        public class B : TestBase { }
        public class C : TestBase { }
        public class D : TestBase { }
        public class E : TestBase { }
        public class F : TestBase { }
        public class G : TestBase { }
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
