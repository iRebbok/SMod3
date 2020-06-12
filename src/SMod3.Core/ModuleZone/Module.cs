
using SMod3.Core.Meta;

namespace SMod3.Core
{
    public abstract class Module : BaseManager
    {
        /// <summary>
        ///     Module starting point.
        /// </summary>
        protected virtual void Awake() { }

        internal void CallAwake() => Awake();
    }
}
