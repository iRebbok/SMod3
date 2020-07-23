using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using SMod3.Core;
using SMod3.Core.Fundamental;
using SMod3.Core.Misc;
using SMod3.Module.EventSystem;
using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Handlers;
using SMod3.Module.EventSystem.Meta;
using SMod3.Module.EventSystem.Meta.Wrappers;
using SMod3.Module.EventSystem.Meta.Wrappers.Native;

[assembly: ModuleDefine(typeof(EventModule))]

namespace SMod3.Module.EventSystem
{
    public struct RegistrationPreferences
    {
        /// <summary>
        ///     Allows the creation of an instance
        ///     of an object that it operates on to register a subscription.
        /// </summary>
        public bool AllowCreateInstance { get; set; }

        /// <summary>
        ///     Allows registering a static method to handle an event.
        /// </summary>
        public bool AllowStaticMethods { get; set; }
    }

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

    public sealed partial class EventModule : Core.Module
    {
        #region Fields & Properties

        private static EventModule? _instance;
#pragma warning disable CS8603 // Possible null reference return.
        public static EventModule Module { get => _instance ??= ModuleManager.Manager.FindModule<EventModule>(); }
#pragma warning restore CS8603 // Possible null reference return.

        public override string LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(EventModule));

        /// <summary>
        ///     Default preferences for subscriber registration.
        /// </summary>
        public RegistrationPreferences DefaultPreferences => new RegistrationPreferences
        {
            AllowCreateInstance = false,
            AllowStaticMethods = true
        };

        /// <summary>
        ///     Module status.
        /// </summary>
        public HandlingStatus Status { get; private set; }

        /// <summary>
        ///     Handling type handler.
        ///     Available only with <see cref="HandlingStatus.Handling"/> status.
        /// </summary>
        public Type? HandlingHandler { get; private set; }

        /// <summary>
        ///     Handling event args.
        ///     Can be null if the event being handled doesn't need any arguments.
        ///     Available only with <see cref="HandlingStatus.Handling"/> status.
        /// </summary>
        public EventArg? HandlingArg { get; private set; }

        /// <summary>
        ///     Available wrappers and delegates for handling events.
        ///     TKey - the type of the delegate; TValue - type of wrapper.
        /// </summary>
        public static ReadOnlyDictionary<Type, Type> AvailableWrappers { get; }

        private static readonly DuplicateKeyOrderByDescendingComparator<BaseEventWrapper> _priorityComparator = new DuplicateKeyOrderByDescendingComparator<BaseEventWrapper>();
        // Key means event handler type, don't confuse with event args 
        private readonly Dictionary<Type, SortedSet<BaseEventWrapper>> _eventMeta = new Dictionary<Type, SortedSet<BaseEventWrapper>>();
        // Instances of objects for subscribers, for example when 'AllowCreateInstance' is true
        private readonly Dictionary<IdWrapper, List<InstanceWrapper>> _subscribersInstances = new Dictionary<IdWrapper, List<InstanceWrapper>>();

        // Blocked events for a generic event
        private readonly Type[] _blockedEventsForGenericEvent;

        #endregion

        #region Constructors

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

        static EventModule()
        {
            AvailableWrappers = new ReadOnlyDictionary<Type, Type>(new Dictionary<Type, Type>
            {
                [typeof(AsyncDelegate)] = typeof(AsyncEventWrapper),
                [typeof(AsyncDelegateWithArgs<>)] = typeof(AsyncEventWrapperWithArgs<>),

                [typeof(FutureDefiningDelegate)] = typeof(FutureDefiningDelegate),
                [typeof(FutureDefiningDelegateWithArgs<>)] = typeof(FutureDefiningDelegateWithArgs<>),

                [typeof(SimpleDelegate)] = typeof(SimpleEventWrapper),
                [typeof(SimpleDelegateWithArgs<>)] = typeof(SimpleEventWrapperWithArgs<>)
            });
        }

        #endregion

        #region Module events

        public override void Dispose(Plugin owner)
        {
            DisposeInstances(owner, null);
            DisposeHandlers(owner, null);
        }

        #endregion

        #region Handling methods

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
        private readonly Predicate<Type> _blockedForGenericPredicate;
        /// <summary>
        ///     Indicates whether a type is forbidden for a generic type.
        /// </summary>
        private bool IsBlockedForGenericEvent()
            => Array.Exists(_blockedEventsForGenericEvent, _blockedForGenericPredicate);

        #endregion

        #region Registration management

        private void RegisterHandler(Type handlerType, MethodInfo method, object? instance, Plugin? partOwner, Assembly? owner, RegistrationPreferences preferences, Priority priority = Priority.NORMAL)
        {
            if (!TypeExtension.IsMethodCompatibleWithEventHandler(handlerType, method))
                throw new InvalidOperationException("Method isn't compatible with handler");

            KeyValuePair<Type, Type> target = default;
            foreach (var aWrapPair in AvailableWrappers)
            {
                if (TypeExtension.IsMethodCompatibleWithDelegate(aWrapPair.Key, method))
                {
                    target = aWrapPair;
                    break;
                }
            }

            // Accordingly, if we're dealing with generic parameters,
            // then we set the generic parameters as method arguments,
            // because they were validated
            if (target.Key.IsGenericType)
                target = new KeyValuePair<Type, Type>(target.Key.MakeGenericType(method.GetParameters().Select(p => p.ParameterType).ToArray()), target.Value);

            if (target.Key is null || target.Value is null)
                throw new InvalidOperationException("Method isn't compatible with one delegate");

            if (method.IsStatic && !preferences.AllowStaticMethods)
            {
                throw new InvalidOperationException("Registration of static methods isn't allowed by preferences");
            }
            else if (!method.IsStatic && instance is null)
            {
                if (!preferences.AllowCreateInstance)
                    throw new InvalidOperationException("Instantiation isn't allowed by preferences");
                else
                    instance = FindInstanceOrCreate(method.ReflectedType, partOwner, owner);
            }

            var @delegate = Delegate.CreateDelegate(target.Key, instance, method, true);
            var wrapper = (BaseEventWrapper)Activator.CreateInstance(target.Value, partOwner is null ? (owner!) : (object)partOwner, priority, @delegate);
            if (!_eventMeta.TryGetValue(handlerType, out var set))
            {
                set = new SortedSet<BaseEventWrapper>(_priorityComparator);
                _eventMeta.Add(handlerType, set);
            }

            set.Add(wrapper);
        }

        #endregion

        #region Handler management

        /// <summary>
        ///     Destroys plugin or assembly handlers.
        /// </summary>
        private void DisposeHandlers(Plugin? partOwner, Assembly? owner)
        {
            foreach (var set in _eventMeta.Values)
            {
                set.RemoveWhere(w => CheckWrapperOwner(w, partOwner, owner));
            }
        }

        /// <summary>
        ///     Returns all owner wrappers.
        /// </summary>
        private IEnumerable<BaseEventWrapper> FindEventWrappers(Plugin? partOwner, Assembly? owner)
        {
            foreach (var set in _eventMeta.Values)
            {
                foreach (var wrapper in set)
                {
                    if (CheckWrapperOwner(wrapper, partOwner, owner))
                    {
                        yield return wrapper;
                    }
                }
            }
        }

        #endregion

        #region Instance management

        private void DisposeInstances(Plugin? partOwner, Assembly? owner)
        {
            if (TryFindInstancePair(partOwner, owner, out var pair))
                _subscribersInstances.Remove(pair.Key);
        }

        /// <summary>
        ///     Finds for an instance by type or creates a new one.
        /// </summary>
        private object FindInstanceOrCreate(Type intanceType, Plugin? partOwner, Assembly? owner)
        {
            if (TryFindInstance(intanceType, partOwner, owner, out var instanceWrapper))
                return instanceWrapper!.Instance;

            var instance = Activator.CreateInstance(intanceType);
            InsertInsanceWrapper(partOwner, owner, new InstanceWrapper(instance));
            return instance;
        }

        /// <summary>
        ///     Inserts a wrapper with an instance.
        /// </summary>
        private void InsertInsanceWrapper(Plugin? partOwner, Assembly? owner, InstanceWrapper wrapper)
        {
            if (TryFindInstancePair(partOwner, owner, out var pair))
            {
                pair.Value.Add(wrapper);
                return;
            }

            _subscribersInstances.Add(GenerateIdWrapper(partOwner, owner), new List<InstanceWrapper>(2) { wrapper });
        }

        /// <summary>
        ///     Generates <see cref="IdWrapper"/> for <paramref name="partOwner"/> and <paramref name="owner"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="TryFindInstancePair(Plugin?, Assembly?, out KeyValuePair{IdWrapper, List{InstanceWrapper}})" /></exception>
        private IdWrapper GenerateIdWrapper(Plugin? partOwner, Assembly? owner)
        {
            if (!(partOwner is null))
                return new IdWrapper(partOwner);
            else if (!(owner is null))
                return new IdWrapper(owner);
            else
                throw new ArgumentNullException("One of the arguments must not be null");
        }

        /// <summary>
        ///     Tries to find an instance by type.
        /// </summary>
        private bool TryFindInstance(Type instanceType, Plugin? partOwner, Assembly? owner, out InstanceWrapper? output)
        {
            if (TryFindInstancePair(partOwner, owner, out var pair))
            {
                var instanceList = pair.Value;
                for (int z = 0; z < instanceList.Count; z++)
                {
                    var instanceWrapper = instanceList[z];
                    if (instanceWrapper.InstanceType == instanceType)
                    {
                        output = instanceWrapper;
                        return true;
                    }
                }
            }

            output = null;
            return false;
        }

        /// <summary>
        ///     Finds a instance pair by assembly.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="TryFindInstancePair(Plugin?, Assembly?, out KeyValuePair{IdWrapper, List{InstanceWrapper}})" /></exception>
        private KeyValuePair<IdWrapper, List<InstanceWrapper>> FindInstancePair(Assembly owner)
        {
            return FindInstancePair(null, owner);
        }

        /// <summary>
        ///     Finds a instance pair by the plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="TryFindInstancePair(Plugin?, Assembly?, out KeyValuePair{IdWrapper, List{InstanceWrapper}})" /></exception>
        private KeyValuePair<IdWrapper, List<InstanceWrapper>> FindInstancePair(Plugin partOwner)
        {
            return FindInstancePair(partOwner, null);
        }

        /// <summary>
        ///     Finds a instance pair by the plugin or assembly.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="TryFindInstancePair(Plugin?, Assembly?, out KeyValuePair{IdWrapper, List{InstanceWrapper}})" /></exception>
        /// <exception cref="KeyNotFoundException">
        ///     No matching KeyValuePair found.
        /// </exception>
        private KeyValuePair<IdWrapper, List<InstanceWrapper>> FindInstancePair(Plugin? partOwner, Assembly? owner)
        {
            if (!TryFindInstancePair(partOwner, owner, out var output))
                throw new KeyNotFoundException($"No matching KeyValuePair found for {nameof(partOwner)} or {nameof(owner)}");
            return output;
        }

        /// <summary>
        ///     Tries to find a instance pair.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="partOwner"/> and <paramref name="owner"/> are null.
        /// </exception>
        private bool TryFindInstancePair(Plugin? partOwner, Assembly? owner, out KeyValuePair<IdWrapper, List<InstanceWrapper>> output)
        {
            if (partOwner is null && owner is null)
                throw new ArgumentNullException("One of the arguments must not be null");

            foreach (var pair in _subscribersInstances)
            {
                var idWrapper = pair.Key;
                if (CheckWrapperOwner(idWrapper, partOwner, owner))
                {
                    output = pair;
                    return true;
                }
            }

            output = default;
            return false;
        }

        /// <summary>
        ///     Checks the owner of the wrapper.
        /// </summary>
        private bool CheckWrapperOwner(BaseWrapper baseWrapper, Plugin? partOwner, Assembly? owner)
        {
            return partOwner is null ? baseWrapper.Owner == owner : baseWrapper.PartOwner == partOwner;
        }

        #endregion
    }
}
