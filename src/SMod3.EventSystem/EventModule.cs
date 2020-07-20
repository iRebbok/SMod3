using System;
using System.Collections.Generic;

using SMod3.Core;
using SMod3.Core.Misc;
using SMod3.Module.EventSystem.Handlers;
using SMod3.Module.EventSystem.Meta.Wrappers;

namespace SMod3.Module.EventSystem
{
    /// <summary>
    ///     Event execution status.
    /// </summary>
    public enum HandlingStatus
    {
        /// <summary>
        ///     The module is free to handle the event.
        /// </summary>
        Free,
        /// <summary>
        ///     The module is handling the event.
        /// </summary>
        Handling
    }

    public sealed class EventModule : Core.Module
    {
        #region Fields & Properties

        private static EventModule? _instance;
#pragma warning disable CS8603 // Possible null reference return.
        public static EventModule Module { get => _instance ??= ModuleManager.Manager.FindModule<EventModule>(); }
#pragma warning restore CS8603 // Possible null reference return.

        public override string LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(EventModule));

        /// <summary>
        ///     Module status.
        /// </summary>
        public HandlingStatus Status { get; private set; }

        /// <summary>
        ///     Handling type handler.
        ///     Available only with <see cref="HandlingStatus.Handling"/> status.
        /// </summary>
        public Type? HandlingHandler { get; private set; }

        private static readonly DuplicateKeyOrderByDescendingComparator<BaseEventWrapper<Delegate>> _priorityComparator = new DuplicateKeyOrderByDescendingComparator<BaseEventWrapper<Delegate>>();
        // Key means event handler type, don't confuse with event args 
        private readonly Dictionary<Type, SortedSet<BaseEventWrapper<Delegate>>> _eventMeta = new Dictionary<Type, SortedSet<BaseEventWrapper<Delegate>>>();

        // Blocked events for a generic event
        private readonly Type[] _blockedEventsForGenericEvent;

        #endregion

        private EventModule()
        {
            _blockedEventsForGenericEvent = new[]
            {
                typeof(IEventHandlerUpdate),
                typeof(IEventHandlerFixedUpdate),
                typeof(IEventHandlerLateUpdate),
            };
            _blockedForGenericPredicate = (e) => e == HandlingHandler;
        }

        /// <summary>
        ///     Updates current status.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Status cannot be marked as handling during event handling.
        /// </exception>
        private void UpdateStatus(HandlingStatus status)
        {
            if (status == HandlingStatus.Handling && Status == HandlingStatus.Handling)
                throw new InvalidOperationException("Trying to duplicate an event call in an event, make sure that the plugins don't trigger the event while it's handling");

            Status = status;
        }

        // Such a solution is justified by creating a new Predicate every method call,
        // with Update/FixedUpdate/LateUpdate it'll create a lot of extra garbage,
        // so we just cache it
        readonly Predicate<Type> _blockedForGenericPredicate;
        /// <summary>
        ///     Indicates whether a type is forbidden for a generic type.
        /// </summary>
        private bool IsBlockedForGenericEvent()
            => Array.Exists(_blockedEventsForGenericEvent, _blockedForGenericPredicate);
    }
}
