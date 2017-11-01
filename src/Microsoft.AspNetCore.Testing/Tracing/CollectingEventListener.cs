using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
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

        // Used to track if the event was written on the active async context.
        // We may want to allow this to be disabled for some tests (as in allow events from all async contexts to be collected).
        // That's pretty easy to add later though.
        private AsyncLocal<bool> _activeAsyncContext = new AsyncLocal<bool>();

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

            // Mark this async context as "active" (will be ignored if isolateAsync is false)
            _activeAsyncContext.Value = true;
        }

        public static IReadOnlyList<EventWrittenEventArgs> CollectEvents(Action action, params string[] eventSourceNames) => CollectEvents(action, true, eventSourceNames);

        public static IReadOnlyList<EventWrittenEventArgs> CollectEvents(Action action, bool isolateAsync, params string[] eventSourceNames)
        {
            var listener = new CollectingEventListener(eventSourceNames);
            action();
            return listener.EventsWritten;
        }

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
            if (_activeAsyncContext.Value)
            {
                _events.Enqueue(eventData);
            }
        }
    }
}
