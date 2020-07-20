using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.Handlers
{
    /// <summary>
    ///     Called when setting a configuration value.
    /// </summary>
    /// <remarks>
    ///     Called exclusively for game configuration.
    /// </remarks>
    public interface IEventHandlerSetConfig : IEventHandler
    {
        void OnSetConfig(SetConfigEvent ev);
    }
}

