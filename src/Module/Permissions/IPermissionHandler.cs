using SMod3.Core.API;

namespace SMod3.Module.Permissions
{
    public interface IPermissionHandler
    {
        /// <summary>
        ///		Queries a plugin for player permissions.
        /// </summary>
        /// <param name="player">
        ///		The player to check the permission of.
        /// </param>
        /// <param name="permissionName">
        ///		The name of the permission to check.
        /// </param>
        /// <returns>
        ///		false for negative permission. (Stops other handlers from allowing it)
        ///		null for no permission.
        ///		true for positive permission.
        /// </returns>
        bool? CheckPermission(Player player, string permissionName);
    }
}
