using System.Reflection;
using System.Threading.Tasks;

using SMod3.Core;

namespace SMod3.Module.EventSystem.Meta.Wrappers.Native
{
    public delegate Task AsyncDelegate();

    /// <summary>
    ///     An async wrapper that allows the interaction with events from asynchronous code.
    /// </summary>
    public sealed class AsyncEventWrapper : BaseEventWrapper<AsyncDelegate>
    {
        public AsyncEventWrapper(Plugin partOwner, Priority priority, AsyncDelegate @delegate)
: base(partOwner, priority, @delegate)
        { }
        public AsyncEventWrapper(Assembly owner, Priority priority, AsyncDelegate @delegate)
            : base(owner, priority, @delegate)
        { }
    }
}
