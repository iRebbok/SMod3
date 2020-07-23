using System.Reflection;

using SMod3.Core;

namespace SMod3.Module.EventSystem.Meta.Wrappers.Native
{
    public delegate bool FutureDefiningDelegate();

    /// <summary>
    ///     A wrapper that has the ability to stop an event by blocking other subscribers from accessing it.
    /// </summary>
    public sealed class FutureDefiningEventWrapper : BaseEventWrapper<FutureDefiningDelegate>
    {
        public FutureDefiningEventWrapper(Plugin partOwner, Priority priority, FutureDefiningDelegate @delegate)
            : base(partOwner, priority, @delegate)
        { }
        public FutureDefiningEventWrapper(Assembly owner, Priority priority, FutureDefiningDelegate @delegate)
            : base(owner, priority, @delegate)
        { }
    }
}
