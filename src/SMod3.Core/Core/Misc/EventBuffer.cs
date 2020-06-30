using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using UnityEngine;

namespace SMod3.Core.Misc
{
    /// <summary>
    ///     Low-level thread-safe event buffer in MonoBehavior.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of event to announce.
    /// </typeparam>
    /// <remarks>
    ///     For proper use, need to add a component to the object
    ///     and then assign values to the properties.
    /// </remarks>
    public sealed class EventBuffer<T> : MonoBehaviour
    {
        /// <summary>
        ///     Delegates.
        /// </summary>
        public IList<Delegate>? Subscribers { get; set; }

        /// <summary>
        ///     Required number of subscribers to call.
        /// </summary>
        public uint RequiredSubscribers { get; set; }

        /// <summary>
        ///     Pointer to the namespace inclusion event names and the class where it's located.
        /// </summary>
        public string? EventName { get; set; }

        /// <summary>
        ///     A buffer containing data to call.
        /// </summary>
        public ConcurrentQueue<T> Buffer { get; }

        public EventBuffer()
        {
            Buffer = new ConcurrentQueue<T>();
            RequiredSubscribers = 1;
        }

        /// <summary>
        ///     Inserts data into a queue or raises an event if it meets the requirements.
        /// </summary>
        public void Enqueue(T item)
        {
            Buffer.Enqueue(item);
        }

        private void LateUpdate()
        {
            if (Subscribers is null || Subscribers.Count < RequiredSubscribers)
                return;

            while (Buffer.TryDequeue(out var item))
            {
                EventMisc.InvokeSafely(EventName!, Subscribers!, item!);
            }
        }
    }
}
