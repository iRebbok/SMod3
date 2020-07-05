using System;

using SMod3.Core.Logging;

namespace SMod3.Core.Fundamental
{
    public abstract class BaseManager : BaseLogger
    {
        /// <summary>
        ///		Used to clear everything associated with the plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="PluginManager.CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="PluginManager.CheckPluginDisposed(Plugin?)"/></exception>
        public virtual void Dispose(Plugin owner)
        {
            PluginManager.CheckPluginDisposed(owner);
        }
    }
}
