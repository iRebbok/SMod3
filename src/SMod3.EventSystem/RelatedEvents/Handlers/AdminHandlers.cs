using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.Handlers
{
    /// <summary>
    ///     Called when handling Remote Admin commands.
    /// </summary>
    public interface IEventHandlerAdminQuery : IEventHandler
    {
        void OnAdminQuery(AdminQueryEvent ev);
    }

    /// <summary>
    ///     Called during a player's ban.
    /// </summary>
    public interface IEventHandlerBan : IEventHandler
    {
        void OnBan(BanEvent ev);
    }

    /// <summary>
    ///     Called during a player's offline ban.
    /// </summary>
    public interface IEventHandlerOfflineBan : IEventHandler
    {
        void OnOffileBane(OfflineBanEvent ev);
    }
}
