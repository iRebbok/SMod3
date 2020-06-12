using System;

using SMod3.Core.Logging;

namespace SMod3.Core.Fundamental
{
    public abstract class BaseManager : BaseLogger
    {
        /// <summary>
        ///		Used to clear everything associated with the plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Plugin is null.
        /// </exception>
        public virtual void Dispose(Plugin owner)
        {
            if (owner is null)
                throw new ArgumentNullException("Plugin cannot be null", nameof(owner));
        }
    }
}
