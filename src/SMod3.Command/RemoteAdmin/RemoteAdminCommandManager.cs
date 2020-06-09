using SMod3.Module.Command.Meta;

namespace SMod3.Module.Command.RemoteAdmin
{
    /// <summary>
    ///		The main manager of the remote admin commands.
    /// </summary>
    public sealed class RemoteAdminCommandManager : BaseCommandManager<BaseRemoteAdminCommandHandler>
    {
        public static RemoteAdminCommandManager Manager { get; } = new RemoteAdminCommandManager();

        private RemoteAdminCommandManager() { }

        public override string LoggingTag => "REMOTEADMIN_COMMAND_MANAGER";
    }
}
