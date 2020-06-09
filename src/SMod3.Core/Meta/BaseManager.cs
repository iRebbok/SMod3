using SMod3.Core.Logging;

namespace SMod3.Core.Meta
{
    public abstract class BaseManager : BaseLogger
    {
        /// <summary>
        ///		Used to clear everything associated with the plugin.
        /// </summary>
        public virtual void Dispose(Plugin owner) { }
    }
}
