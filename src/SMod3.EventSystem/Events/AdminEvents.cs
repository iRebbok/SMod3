using SMod3.API;
using SMod3.Module.EventSystem.EventHandlers;
using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events.Meta;

namespace SMod3.Module.EventSystem.Events
{
    public abstract class AdminEvent : Event
    {
        public ICommandSender CommandSender { get; }

        public AdminEvent(ICommandSender sender)
        {
            CommandSender = sender;
        }
    }

    public class AdminQueryEvent : AdminEvent
    {
        public string Query { get; }
        public string Reply { get; set; }
        public bool Successful { get; }

        public AdminQueryEvent(ICommandSender sender, string query, string reply, bool successful) : base(sender)
        {
            Query = query;
            Reply = reply;
            Successful = successful;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerAdminQuery)handler).OnAdminQuery(this);
        }
    }

    public class AdminQueryCheckEvent : AdminEvent
    {
        public string Query { get; }
        public bool? Allow { get; set; } // The initial value is null, if set to true, then skip the permission check, if false, then do not allow the command to run
        public string Reply { get; set; } // The initial value is null, if set to Allow, this value will be returned to the console

        public AdminQueryCheckEvent(ICommandSender sender, string query) : base(sender)
        {
            Allow = null;
            Reply = null;
            Query = query;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerAdminQueryCheck)handler).OnAdminQueryCheck(this);
        }
    }

    public class AdminPermissionCheckEvent : AdminEvent
    {
        public AdminPermissionCheckEvent(ICommandSender admin) : base(admin)
        {

        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerAdminPermissionCheck)handler).OnAdminPermissionCheck(this);
        }
    }

    public class AuthCheckEvent : Event
    {
        public Player Requester { get; set; }
        public AuthType AuthType { get; set; }
        public bool Allow { get; set; }
        public string DeniedMessage { get; set; }
        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerAuthCheck)handler).OnAuthCheck(this);
        }
    }

    public class BanEvent : Event
    {
        public Player Player { get; set; }
        public Player Admin { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }
        public bool AllowBan { get; set; }
        public bool IsGlobalBan { get; set; }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerBan)handler).OnBan(this);
        }
    }
}
