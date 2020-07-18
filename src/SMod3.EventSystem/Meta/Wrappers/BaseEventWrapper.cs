using System;
using System.Reflection;

using SMod3.Core;
using SMod3.Core.Fundamental;

namespace SMod3.Module.EventSystem.Meta.Wrappers
{
    /// <summary>
    ///     Wrapping abstraction for types of subscribers.
    /// </summary>
    public abstract class BaseEventWrapper<T> : BaseWrapper, IComparable<BaseEventWrapper<T>> where T : Delegate
    {
        public Priority Priority { get; set; }
        public T Delegate { get; }
        public Type HandlerType { get; }

        protected BaseEventWrapper(Plugin partOwner, Priority priority, T @delegate, Type handlerType) : base(partOwner)
        {
            Priority = priority;
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
        }

        protected BaseEventWrapper(Assembly owner, Priority priority, T @delegate, Type handlerType) : base(owner)
        {
            Priority = priority;
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
        }

        public int CompareTo(BaseEventWrapper<T> other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}
