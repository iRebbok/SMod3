using System;
using System.Reflection;

using SMod3.Core;

namespace SMod3.Module.EventSystem.Meta.Wrappers.Native
{
    public delegate void SimpleDelegate();

    /// <summary>
    ///     Simple event wrapper.
    /// </summary>
    public sealed class SimpleEventWrapper : BaseEventWrapper<SimpleDelegate>
    {
        public SimpleEventWrapper(Plugin partOwner, Priority priority, SimpleDelegate @delegate, Type handlerType)
    : base(partOwner, priority, @delegate, handlerType)
        { }
        public SimpleEventWrapper(Assembly owner, Priority priority, SimpleDelegate @delegate, Type handlerType)
            : base(owner, priority, @delegate, handlerType)
        { }
    }
}
