using SMod3.Core;
using SMod3.Core.API;
using SMod3.Core.Meta;
using System.Collections.Generic;

namespace SMod3.Module.Permissions
{
    public sealed class PermissionManager : BaseManager
    {
        public static PermissionManager Manager { get; } = new PermissionManager();

        public override string LoggingTag => "PERMISSION_MANAGER";

        private PermissionManager() { /* Immediately registers the default permissions handler */ RegisterHandler(DefaultPermissionsHandler.Handler); }

        private readonly HashSet<IPermissionHandler> permissionHandlers = new HashSet<IPermissionHandler>();

        // Used by permission plugins to register themselves as permission handlers
        public bool RegisterHandler(IPermissionHandler handler)
        {
            if (handler == null)
            {
                PluginManager.Manager.Logger.Error("PERMISSIONS_MANAGER", "Failed to add permissions handler as it was null.");
                return false;
            }

            if (permissionHandlers.Add(handler))
            {
                return true;
            }

            PluginManager.Manager.Logger.Warn("PERMISSIONS_MANAGER", "Attempted to add duplicate permissions handler.");
            return false;
        }

        public void UnregisterHandler(IPermissionHandler handler)
        {
            permissionHandlers.Remove(handler);
        }

        public bool? CheckPermission(Player player, string permissionName)
        {
            bool? allowed = false;
            foreach (IPermissionHandler handler in permissionHandlers)
            {
                // Checks each permission handler, aborts if this permission node is negative in any handler
                allowed = handler.CheckPermission(player, permissionName);
                if (allowed == false) break;
            }
            // True if any permission handler returns positively and none return negatively
            return allowed;
        }
    }

}
