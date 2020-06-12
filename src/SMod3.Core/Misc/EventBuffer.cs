using System;
using System.Collections.Generic;

namespace SMod3.Core.Misc
{
    /// <summary>
    ///     An event buffer that stores all events up to the desired number of subscribers.
    /// </summary>
    public sealed class EventBuffer<T>
    {
        /// <summary>
        ///     Delegates.
        /// </summary>
        public List<Delegate> Delegates { get; }

        /// <summary>
        ///     A buffer containing data to call.
        /// </summary>
        public Queue<T> Buffer { get; }

        /// <summary>
        ///     Required number of subscribers to call.
        /// </summary>
        public uint RequiredSubscribers { get; set; }

        /// <summary>
        ///     Pointer to the namespace inclusion event names and the class where it's located.
        /// </summary>
        public string EventName { get; }

        /// <param name="eventName"><inheritdoc cref="EventName"/></param>
        public EventBuffer(string eventName)
        {
            Delegates = new List<Delegate>();
            Buffer = new Queue<T>();
            RequiredSubscribers = 1;
            EventName = eventName;
        }

        /// <summary>
        ///     Inserts data into a queue or raises an event if it meets the requirements.
        /// </summary>
        public void Enqueue(T item)
        {
            if (Delegates.Count < RequiredSubscribers)
                Buffer.Enqueue(item);
            else
            {
                while (Buffer.Count > 0)
                {
                    var curItem = Buffer.Dequeue();
                    EventMisc.InvokeSafely(EventName, Delegates, curItem!);
                }
                EventMisc.InvokeSafely(EventName, Delegates, item!);
            }
        }
    }
}
