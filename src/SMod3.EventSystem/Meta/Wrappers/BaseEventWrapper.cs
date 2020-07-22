using System;
using System.Reflection;

using SMod3.Core;
using SMod3.Core.Fundamental;

namespace SMod3.Module.EventSystem.Meta.Wrappers
{
    public abstract class BaseEventWrapper : BaseWrapper, IComparable<BaseEventWrapper>
    {
        public Priority Priority { get; set; }

        protected BaseEventWrapper(Priority priority, Plugin partOwner) : base(partOwner)
        {
            Priority = priority;
        }

        protected BaseEventWrapper(Priority priority, Assembly owner) : base(owner)
        {
            Priority = priority;
        }

        public int CompareTo(BaseEventWrapper other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }

    /// <summary>
    ///     Wrapping abstraction for types of subscribers.
    /// </summary>
    public abstract class BaseEventWrapper<T> : BaseEventWrapper where T : Delegate
    {
        public T Delegate { get; }

        protected BaseEventWrapper(Plugin partOwner, Priority priority, T @delegate) : base(priority, partOwner)
        {
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
        }

        protected BaseEventWrapper(Assembly owner, Priority priority, T @delegate) : base(priority, owner)
        {
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
        }
    }
}
