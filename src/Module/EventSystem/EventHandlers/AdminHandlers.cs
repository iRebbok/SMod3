using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.EventHandlers
{
    public interface IEventHandlerAdminQuery : IEventHandler
    {
        void OnAdminQuery(AdminQueryEvent ev);
    }

    public interface IEventHandlerAdminQueryCheck : IEventHandler
    {
        void OnAdminQueryCheck(AdminQueryCheckEvent ev);
    }

    public interface IEventHandlerAdminPermissionCheck : IEventHandler
    {
        void OnAdminPermissionCheck(AdminPermissionCheckEvent ev);
    }

    public interface IEventHandlerAuthCheck : IEventHandler
    {
        void OnAuthCheck(AuthCheckEvent ev);
    }

    public interface IEventHandlerBan : IEventHandler
    {
        void OnBan(BanEvent ev);
    }
}
