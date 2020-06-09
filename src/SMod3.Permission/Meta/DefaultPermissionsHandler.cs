using System.Collections.Generic;

using SMod3.API;

namespace SMod3.Module.Permissions
{
    /// <remarks>
    ///		Plugins can register permissions which are given to everyone by default.
    ///		Permission plugins can still counter these by returing a negative permission.
    /// </remarks>
    public class DefaultPermissionsHandler : IPermissionHandler
    {
        public static DefaultPermissionsHandler Handler { get; } = new DefaultPermissionsHandler();

        private readonly HashSet<string> defaultPerms = new HashSet<string>();

        public bool? CheckPermission(Player player, string permissionName)
        {
            return defaultPerms.Contains(permissionName) ? (bool?)true : null;
        }

        public bool AddPermission(string permissionName)
        {
            if (string.IsNullOrEmpty(permissionName))
            {
                PermissionManager.Manager.Warn("Attempted to add default permission but it was empty or null");
                return false;
            }

            PermissionManager.Manager.Debug($"Added default permission: {permissionName}");
            return defaultPerms.Add(permissionName);
        }

        public bool RemovePermission(string permissionName)
        {
            if (string.IsNullOrEmpty(permissionName))
            {
                PermissionManager.Manager.Warn("Attempted to remove default permission but string was empty or null");
                return false;
            }

            PermissionManager.Manager.Debug($"Removed default permission: {permissionName}");
            return defaultPerms.Remove(permissionName);
        }
    }
}
