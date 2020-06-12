using SMod3.Core;
using SMod3.Core.Meta;

namespace SMod3.Imbedded.Attribute
{
    public abstract class BaseAttributeManager : BaseManager
    {
        /// <summary>
        ///		Registers the attributes of the plugin.
        /// </summary>
        /// <typeparam name="TInstance">
        ///		Instance of the class whose attributes will be registered.
        /// </typeparam>
        public abstract void RegisterAttributes<TInstance>(Plugin plugin, TInstance instance) where TInstance : class;

        /// <summary>
        ///		Refresh the attributes of the plugin.
        ///		Note that some implementations of the attribute manager don't require refreshing attributes.
        /// </summary>
        public abstract void RefreshAttributes(Plugin plugin);
    }
}
