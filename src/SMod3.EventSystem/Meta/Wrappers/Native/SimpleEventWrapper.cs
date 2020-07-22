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
        public SimpleEventWrapper(Plugin partOwner, Priority priority, SimpleDelegate @delegate)
    : base(partOwner, priority, @delegate)
        { }
        public SimpleEventWrapper(Assembly owner, Priority priority, SimpleDelegate @delegate)
            : base(owner, priority, @delegate)
        { }
    }
}
