using System;

namespace SMod3.Core.Meta
{
    public abstract class BaseManager : BaseLogging
    {
        /// <summary>
        ///		Used to clear everything associated with the plugin.
        /// </summary>
        public virtual void Dispose(Plugin owner)
        {
            throw new NotImplementedException();
        }
    }
}
