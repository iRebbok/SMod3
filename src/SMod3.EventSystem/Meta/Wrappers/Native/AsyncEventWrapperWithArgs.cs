using System.Reflection;
using System.Threading.Tasks;

using SMod3.Core;
using SMod3.Module.EventSystem.Background;

namespace SMod3.Module.EventSystem.Meta.Wrappers.Native
{
    public delegate Task AsyncDelegateWithArgs<T>(T arg) where T : EventArg;

    /// <summary>
    ///     An async wrapper that allows the interaction with events from asynchronous code with args.
    /// </summary>
    /// <typeparam name="T">
    ///     Event arg type.
    /// </typeparam>
    public sealed class AsyncEventWrapperWithArgs<T> : BaseEventWrapper<AsyncDelegateWithArgs<T>> where T : EventArg
    {
        public AsyncEventWrapperWithArgs(Plugin partOwner, Priority priority, AsyncDelegateWithArgs<T> @delegate)
: base(partOwner, priority, @delegate)
        { }
        public AsyncEventWrapperWithArgs(Assembly owner, Priority priority, AsyncDelegateWithArgs<T> @delegate)
            : base(owner, priority, @delegate)
        { }
    }
}
