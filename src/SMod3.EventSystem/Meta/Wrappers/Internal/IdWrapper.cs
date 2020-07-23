using System.Reflection;

using SMod3.Core;
using SMod3.Core.Fundamental;

namespace SMod3.Module.EventSystem.Meta.Wrappers
{
    /// <summary>
    ///     Wrapper for plugin or assembly identification.
    /// </summary>
    internal sealed class IdWrapper : BaseWrapper
    {
        public IdWrapper(Assembly owner) : base(owner) { }
        public IdWrapper(Plugin partOwner) : base(partOwner) { }
    }
}
