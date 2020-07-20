using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SMod3.Core.Misc
{
    /// <summary>
    ///     Low-level thread-safe event buffer.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of event to announce.
    /// </typeparam>
    public class EventBuffer<T> : ConcurrentQueue<T>
    {
        /// <summary>
        ///     Delegates.
        /// </summary>
        public IList<CustomDelegate<T>> Subscribers { get; }

        /// <summary>
        ///     Pointer to the namespace inclusion event names and the class where it's located.
        /// </summary>
        public string? EventName { get; }

        public EventBuffer(string eventName)
        {
            Subscribers = new List<CustomDelegate<T>>();
            EventName = eventName;
        }
    }
}
