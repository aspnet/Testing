using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Xunit;

namespace Microsoft.AspNetCore.Testing.Tracing
{
    // REVIEW: Also to go to some testing or shared-source assembly
    // This can only collect from EventSources created AFTER this listener is created
    public class CollectingEventListener : EventListener
    {
        private ConcurrentQueue<EventWrittenEventArgs> _events = new ConcurrentQueue<EventWrittenEventArgs>();
        private HashSet<string> _eventSources;

        private object _lock = new object();
        private List<EventSource> _existingSources = new List<EventSource>();

        public IReadOnlyList<EventWrittenEventArgs> EventsWritten => _events.ToArray();

        public CollectingEventListener(params string[] eventSourceNames)
        {
            lock (_lock)
            {
                _eventSources = new HashSet<string>(eventSourceNames, StringComparer.Ordinal);

                // Enable the sources that were created before we were constructed.
                foreach (var existingSource in _existingSources.Where(s => _eventSources.Contains(s.Name)))
                {
                    EnableEvents(existingSource, EventLevel.Verbose, EventKeywords.All);
                }
            }
        }

        // REVIEW: I was going to do a Dictionary<string, List<EventWrittenEventArgs>> to group by event source name,
        // but sometimes you might want to verify the interleaving of events from different sources. This is testing code that
        // won't ship though so we can fiddle with this as we need.
        public IReadOnlyList<EventWrittenEventArgs> GetEventsWrittenTo(string eventSourceName) =>
            EventsWritten.Where(e => e.EventSource.Name.Equals(eventSourceName, StringComparison.Ordinal)).ToList();

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (_eventSources == null)
            {
                lock (_lock)
                {
                    if (_eventSources == null)
                    {
                        _existingSources.Add(eventSource);
                        return;
                    }
                }
            }

            if (_eventSources.Contains(eventSource.Name))
            {
                EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            _events.Enqueue(eventData);
        }
    }
}
