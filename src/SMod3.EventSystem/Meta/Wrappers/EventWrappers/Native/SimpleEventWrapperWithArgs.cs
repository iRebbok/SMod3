using System.Reflection;

using SMod3.Core;
using SMod3.Module.EventSystem.Background;

namespace SMod3.Module.EventSystem.Meta.Wrappers.Native
{
    public delegate void SimpleDelegateWithArgs<T>(T arg) where T : EventArg;

    /// <summary>
    ///     Simple event wrapper with args.
    /// </summary>
    /// <typeparam name="T">
    ///     Event arg type.
    /// </typeparam>
    public sealed class SimpleEventWrapperWithArgs<T> : BaseEventWrapper<SimpleDelegateWithArgs<T>> where T : EventArg
    {
        public SimpleEventWrapperWithArgs(Plugin partOwner, Priority priority, SimpleDelegateWithArgs<T> @delegate)
: base(partOwner, priority, @delegate)
        { }
        public SimpleEventWrapperWithArgs(Assembly owner, Priority priority, SimpleDelegateWithArgs<T> @delegate)
            : base(owner, priority, @delegate)
        { }
    }
}
