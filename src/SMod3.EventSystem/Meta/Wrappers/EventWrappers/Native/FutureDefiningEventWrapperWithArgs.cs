using System.Reflection;

using SMod3.Core;
using SMod3.Module.EventSystem.Background;

namespace SMod3.Module.EventSystem.Meta.Wrappers.Native
{
    public delegate bool FutureDefiningDelegateWithArgs<T>(T data) where T : EventArg;

    /// <summary>
    ///     A wrapper that has the ability to stop an event by blocking other subscribers from accessing it with arguments.
    /// </summary>
    /// <typeparam name="T">
    ///     Event arg type.
    /// </typeparam>
    public sealed class FutureDefiningEventWrapperWithArgs<T> : BaseEventWrapper<FutureDefiningDelegateWithArgs<T>> where T : EventArg
    {
        public FutureDefiningEventWrapperWithArgs(Plugin partOwner, Priority priority, FutureDefiningDelegateWithArgs<T> @delegate)
            : base(partOwner, priority, @delegate)
        { }
        public FutureDefiningEventWrapperWithArgs(Assembly owner, Priority priority, FutureDefiningDelegateWithArgs<T> @delegate)
            : base(owner, priority, @delegate)
        { }
    }
}
