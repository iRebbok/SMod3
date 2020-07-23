using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SMod3.Core;
using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Handlers;
using SMod3.Module.EventSystem.Meta.Wrappers;
using SMod3.Module.EventSystem.Meta.Wrappers.Native;

namespace SMod3.Module.EventSystem
{
    /// <summary>
    ///     Wrappers to handle the event.
    /// </summary>
    [Flags]
#pragma warning disable RCS1135 // Declare enum member with zero value (when enum has FlagsAttribute).
    internal enum HandleWrappersFilter
#pragma warning restore RCS1135 // Declare enum member with zero value (when enum has FlagsAttribute).
    {
        #region Wrappers

        /// <inheritdoc cref="AsyncEventWrapper" />
        ASYNC = 1 << 0,
        /// <inheritdoc cref="AsyncEventWrapperWithArgs{T}" />
        ASYNC_WITH_ARGS = 1 << 1,

        /// <inheritdoc cref="FutureDefiningEventWrapper" />
        FUTUREDEFINING = 1 << 2,
        /// <inheritdoc cref="FutureDefiningEventWrapperWithArgs{T}" />
        FUTUREDEFINING_WITH_ARGS = 1 << 3,

        /// <inheritdoc cref="SimpleEventWrapper" />
        SIMPLE = 1 << 4,
        /// <inheritdoc cref="SimpleEventWrapperWithArgs{T}" />
        SIMPLE_WITH_ARGS = 1 << 5,

        #endregion

        ALL_EXCEPT_ASYNC = FUTUREDEFINING | FUTUREDEFINING_WITH_ARGS | SIMPLE | SIMPLE_WITH_ARGS,
        ALL_INCLUDE_ASYNC = ALL_EXCEPT_ASYNC | ASYNC | ASYNC_WITH_ARGS,
    }

    public sealed partial class EventModule
    {
        /// <summary>
        ///     Handles the event with args.
        ///     Also calls subscribers with no args.
        /// </summary>
        internal void HandleEvent<TEvent, TArg>(TArg arg)
            where TEvent : IEventHandler where TArg : EventArg, new()
        {
            try
            {
                GetBusy(typeof(TEvent), arg);

                // Commented because there are no events with arguments that shouldn't fire a generic event
                //var allowGenericCall = IsBlockedForGenericEvent();

                // Preventing async calls for events that shouldn't be triggered by calling this event
                InternalHandleEvent<TEvent, TArg>(arg /* , allowGenericCall ? HandleWrappersFilter.ALL_INCLUDE_ASYNC : HandleWrappersFilter.ALL_EXCEPT_ASYNC */);
                //if (allowGenericCall)
                InternalHandleEvent<IEventHandlerGenericEvent, TArg>(arg);

                GetFree();
            }
            catch (Exception ex)
            {
                // In this place,
                // there can only be exceptions of the double event call level
                // or something else that violates the main event call,
                // so it is marked as critical.
                Critical($"Exception while handling an event of type '{typeof(TEvent).FullName}' with arguments");
                Critical(ex.ToString());
            }
        }

        /// <summary>
        ///     Handles the event without any args.
        /// </summary>
        internal void HandleEvent<TEvent>()
            where TEvent : IEventHandler
        {
            try
            {
                GetBusy(typeof(TEvent), null);

                var allowGenericCall = IsBlockedForGenericEvent();

                // Preventing async calls for events that shouldn't be triggered by calling this event
                InternalHandleEvent<TEvent>(allowGenericCall ? HandleWrappersFilter.ALL_INCLUDE_ASYNC : HandleWrappersFilter.ALL_EXCEPT_ASYNC);
                if (allowGenericCall)
                    InternalHandleEvent<IEventHandlerGenericEvent>();

                GetFree();
            }
            catch (Exception ex)
            {
                // In this place,
                // there can only be exceptions of the double event call level
                // or something else that violates the main event call,
                // so it is marked as critical.
                Critical($"Exception while handling an event of type '{typeof(TEvent).FullName}' without arguments");
                Critical(ex.ToString());
            }
        }

        #region Low-level

        private void InternalHandleEvent<TEvent, TArg>(TArg arg, HandleWrappersFilter filter = HandleWrappersFilter.ALL_INCLUDE_ASYNC)
            where TEvent : IEventHandler where TArg : EventArg, new()
        {
            if (!_eventMeta.TryGetValue(typeof(TEvent), out SortedSet<BaseEventWrapper> eventList))
                return;

            // When clearing events of a certain type,
            // we don't remove the dictionary key,
            // the original SortedSet remains, but empty
            if (eventList.Count == 0)
                return;

            foreach (var eventWrapper in eventList)
            {
                if (!InternalHandleWrapperSafe<TEvent, TArg>(eventWrapper, arg, filter))
                {
                    break;
                }
            }
        }

        private void InternalHandleEvent<TEvent>(HandleWrappersFilter filter = HandleWrappersFilter.ALL_INCLUDE_ASYNC)
            where TEvent : IEventHandler
        {
            if (!_eventMeta.TryGetValue(typeof(TEvent), out SortedSet<BaseEventWrapper> eventList))
                return;

            // When clearing events of a certain type,
            // we don't remove the dictionary key,
            // the original SortedSet remains, but empty
            if (eventList.Count == 0)
                return;

            foreach (var eventWrapper in eventList)
            {
                if (!InternalHandleWrapperSafe<TEvent>(eventWrapper, filter))
                {
                    break;
                }
            }
        }

        /// <returns>
        ///     true if handling is allowed to continue, otherwise false.
        ///     (<see cref="FutureDefiningDelegate"/> and <see cref="FutureDefiningDelegateWithArgs{T}"/>).
        /// </returns>
        private bool InternalHandleWrapperSafe<TEvent, TArg>(BaseEventWrapper wrapper, TArg arg, HandleWrappersFilter filter)
            where TEvent : IEventHandler where TArg : EventArg, new()
        {
            try
            { return InternalHandleWrapperUnsafe<TEvent, TArg>(wrapper, arg, filter); }
            catch (Exception ex)
            { HandleException(wrapper, ex); }

            return true;
        }

        /// <returns><inheritdoc cref="InternalHandleEvent{TEvent, TArg}(TArg, HandleWrappersFilter)" /></returns>
        private bool InternalHandleWrapperSafe<TEvent>(BaseEventWrapper wrapper, HandleWrappersFilter filter)
            where TEvent : IEventHandler
        {
            try
            { return InternalHandleWrapperUnsafe<TEvent>(wrapper, filter); }
            catch (Exception ex)
            { HandleException(wrapper, ex); }

            return true;
        }

        private bool InternalHandleWrapperUnsafe<TEvent, TArg>(BaseEventWrapper wrapper, TArg arg, HandleWrappersFilter filter)
            where TEvent : IEventHandler where TArg : EventArg, new()
        {
            CheckPlugin(wrapper.PartOwner);

            if ((HandleWrappersFilter.ASYNC_WITH_ARGS & filter) != 0 && wrapper is AsyncEventWrapperWithArgs<TArg> asyncWrapper)
            {
                var argsCopy = EventArgsPool<TArg>.Get();
                arg.CopyTo(argsCopy.Arg);
                Task.Run(async () =>
                {
                    try
                    { await asyncWrapper.Delegate(argsCopy.Arg).ConfigureAwait(false); }
                    catch (Exception ex)
                    { HandleException(wrapper, ex); }
                    EventArgsPool<TArg>.Recycle(argsCopy);
                }).ConfigureAwait(false);
            }
            else if ((HandleWrappersFilter.FUTUREDEFINING_WITH_ARGS & filter) != 0 && wrapper is FutureDefiningEventWrapperWithArgs<TArg> futureDefiningWrapper)
            {
                return futureDefiningWrapper.Delegate(arg);
            }
            else if ((HandleWrappersFilter.SIMPLE_WITH_ARGS & filter) != 0 && wrapper is SimpleEventWrapperWithArgs<TArg> simpleWrapper)
            {
                simpleWrapper.Delegate(arg);
            }
            else
            {
                return InternalHandleWrapperUnsafe<TEvent>(wrapper, filter);
            }

            return true;
        }

        private bool InternalHandleWrapperUnsafe<TEvent>(BaseEventWrapper wrapper, HandleWrappersFilter filter)
            where TEvent : IEventHandler
        {
            CheckPlugin(wrapper.PartOwner);

            if ((HandleWrappersFilter.ASYNC & filter) != 0 && wrapper is AsyncEventWrapper asyncWrapper)
            {
                Task.Run(async () =>
                {
                    try
                    { await asyncWrapper.Delegate().ConfigureAwait(false); }
                    catch (Exception ex)
                    { HandleException(wrapper, ex); }
                }).ConfigureAwait(false);
            }
            else if ((HandleWrappersFilter.FUTUREDEFINING & filter) != 0 && wrapper is FutureDefiningEventWrapper futureDefiningWrapper)
            {
                return futureDefiningWrapper.Delegate();
            }
            else if ((HandleWrappersFilter.SIMPLE & filter) != 0 && wrapper is SimpleEventWrapper simpleWrapper)
            {
                simpleWrapper.Delegate();
            }

            return true;
        }

        /// <summary>
        ///     Checks the plugin for event handling,
        ///     otherwise throws an exception.
        /// </summary>
        private void CheckPlugin(Plugin? plugin)
        {
            if (!(plugin is null) && plugin.Status != PluginStatus.ENABLED)
                throw new InvalidOperationException("Plugin isn't enabled");
        }

        #endregion

        /// <summary>
        ///     Handles exceptions thrown by event handlers.
        /// </summary>
        private void HandleException(BaseEventWrapper wrapper, Exception ex)
        {
            Error($"{(wrapper.PartOwner == null ? "Assembly" : "Plugin")} '{wrapper.PartOwner?.ToString() ?? wrapper.Owner.FullName}'{(wrapper.PartOwner == null ? string.Empty : $" [{wrapper.PartOwner.GetType().FullName}]")} caused an exception when handling the event '{HandlingHandler}'");
            Error(ex.ToString());
        }

        private void GetBusy(Type handlerType, EventArg? arg = null)
        {
            // `UpdateStatus` throws an exception on duplicate event call during event handling,
            // so we call it before setting properties
            UpdateStatus(HandlingStatus.Handling);
            HandlingHandler = handlerType;
            HandlingArg = arg;
        }

        private void GetFree()
        {
            UpdateStatus(HandlingStatus.Free);
            HandlingHandler = null;
            HandlingArg = null;
        }
    }
}
