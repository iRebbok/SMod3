using System;
using System.Collections.Generic;
using System.Linq;

using SMod3.Core;
using SMod3.Core.Fundamental;
using SMod3.Core.Misc;
using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events.Meta;
using SMod3.Module.EventSystem.Meta;

namespace SMod3.Module.EventSystem
{
    public class EventManager : BaseManager
    {
        public static EventManager Manager { get; } = new EventManager();

        public override string LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(EventManager));

        private static readonly PriorityComparator priorityComparator = new PriorityComparator();
        private readonly Dictionary<Type, List<EventHandlerWrapper>> eventMeta = new Dictionary<Type, List<EventHandlerWrapper>>();

        public void HandleEvent<T>(Event ev) where T : IEventHandler
        {
            var handlers = GetEventHandlers<T>();
            if (handlers is null)
                return;

            foreach (T handler in handlers)
            {
                try
                {
                    ev.ExecuteHandler(handler);
                }
                catch (Exception e)
                {
                    Error($"Handler {handler.GetType().FullName} failed to handler event {ev.GetType()}: {e.InnerException} {e.Message}");
                    Error(e.StackTrace);
                }
            }
        }

        public void AddEventHandlers(Plugin plugin, IEventHandler handler, Priority priority = Priority.NORMAL)
        {
            if (plugin == null || handler == null)
            {
                return;
            }

            foreach (Type intfce in handler.GetType().GetInterfaces())
            {
                if (typeof(IEventHandler).IsAssignableFrom(intfce))
                {
                    plugin.Debug($"Adding event handler for {intfce.Name}");
                    AddEventHandler(plugin, intfce, handler, priority);
                }
            }
        }

        public void AddEventHandler(Plugin plugin, Type eventType, IEventHandler handler, Priority priority = Priority.NORMAL)
        {
            if (!typeof(IEventHandler).IsAssignableFrom(eventType))
            {
                Error($"{plugin} is trying to use an unsupported type {eventType} of event {handler}");
                return;
            }
            plugin.Debug($"Adding event handler from {plugin} type: {eventType} priority {priority} handler {handler.GetType().FullName}");
            EventHandlerWrapper wrapper = new EventHandlerWrapper(plugin, priority, handler);

            AddEventMeta(eventType, wrapper);
        }

        private void AddEventMeta(Type eventType, EventHandlerWrapper wrapper)
        {
            if (!eventMeta.TryGetValue(eventType, out var wrappers))
            {
                eventMeta.Add(eventType, new List<EventHandlerWrapper> { wrapper });
            }
            else
            {
                wrappers.Add(wrapper);
                wrappers.Sort(priorityComparator);
            }
        }

        public override void Dispose(Plugin owner)
        {
            if (owner == null) return;

            foreach (var eventType in eventMeta.Keys)
            {
                eventMeta[eventType] = eventMeta[eventType].Where(wrapper => wrapper.Owner != null).ToList();
            }
        }

        public IEnumerable<T>? GetEventHandlers<T>() where T : IEventHandler
        {
            if (!eventMeta.TryGetValue(typeof(T), out var wrappers))
                return null;

            return wrappers.Select(wrapper => (T)wrapper.Handler);
        }
    }
}
