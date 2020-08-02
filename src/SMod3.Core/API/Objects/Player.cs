using System;
using System.Collections.Generic;

using UnityEngine;

namespace SMod3.API
{
    /// <exception cref="InvalidOperationException">
    ///     Player object was destroyed.
    /// </exception>
    public abstract class Player : ICommandSender, IGenericApiObject
    {
        #region Properties

        public abstract bool DoNotTrack { get; }
        /// <summary>
        ///     Gets the player’s connection status,
        ///     whether it's on the server or its object is already destroyed.
        /// </summary>
        public abstract bool IsConnected { get; }
        public abstract Connection Conn { get; }
        /// <summary>
        ///     Gets the player’s nickname.
        /// </summary>
        public abstract string Nickname { get; }
        /// <summary>
        ///     Gets a unique player id.
        /// </summary>
        public abstract int PlayerId { get; }
        /// <summary>
        ///     Gets the player's user id.
        /// </summary>
        public abstract string UserId { get; }
        public abstract string AuthToken { get; }
        /// <summary>
        ///     Player rotation.
        /// </summary>
        /// <remarks>
        ///     The game synchronizes the rotation of the player in only two axes, X and Y.
        ///     The game doesn't allow us to set the player’s rotation.
        /// </remarks>
        public abstract Vector2 Rotation { get; }
        public abstract Scp079Data Scp079Data { get; }
        public abstract Scp268Data Scp268Data { get; }
        public abstract TeamRole TeamRole { get; }
        /// <summary>
        ///		Gets a player's UserGroup from the player's rank.
        /// </summary>
        /// <returns>
        ///		Null If the player doesn't have a rank.
        /// </returns>
        public abstract IUserGroup? UserGroup { get; }
        public abstract Inventory Inventory { get; }

        /// <summary>
        ///     Gets or sets health.
        /// </summary>
        public abstract float Health { get; set; }
        public abstract bool BypassMode { get; set; }
        /// <summary>
        ///		Gets and sets the ghost mode for the player.
        /// </summary>
        public GhostSettings GhostMode { get; set; }
        /// <summary>
        ///		Gets or sets the god mode.
        /// </summary>
        public bool GodMode { get; set; }
        public abstract bool IntercomMuted { get; set; }
        public abstract bool Muted { get; set; }
        public abstract bool OverwatchMode { get; set; }
        public abstract RadioStatus RadioStatus { get; set; }
        /// <summary>
        ///     If the value is more than 101, then the battery is infinite.
        /// </summary>
        /// <remarks>
        ///     If the player isn't holding the radio,
        ///     then it applies to all radio items or returs <see cref="int.MinValue"/>.
        /// </remarks>
        public abstract int RadioBattery { get; set; }
        /// <summary>
        ///     Gets or sets the player’s position
        ///     - teleports with default values via the <see cref="Teleport(Vector3, float, bool)"/> method.
        /// </summary>
        public abstract Vector3 Position { get; set; }
        /// <summary>
        ///     Displayed player nickname in player list and remote admin.
        /// </summary>
        public abstract string? DisplayNickname { get; set; }

        #endregion

        #region Methods

        public abstract void AddHealth(float amount);
        public abstract bool Ban(uint duration, string? message = null, bool isGlobalBan = false);
        /// <summary>
        ///     Assigns the class of the player.
        /// </summary>
        /// <param name="full">
        ///     Determines whether this is a full role change.
        /// </param>
        /// <param name="lite">
        ///     Determines whether this is a lite role change.
        /// </param>
        /// <param name="escape">
        ///     Determines if this is a role change for the escaped player.
        /// </param>
        public abstract void ChangeRole(RoleType role, bool full = true, bool lite = false, bool escape = false);
        public abstract void Damage(float amount, DamageType type = DamageType.NONE);
        /// <summary>
        ///     Disconnects a player from the server.
        /// </summary>
        /// <remarks>
        ///     This isn't a kick wrapper, this is a direct disconnect of the player.
        /// </remarks>
        public abstract void Disconnect(string? message = null);
        /// <summary>
        ///		Gets SCP-106's portal position.
        /// </summary>
        /// <returns>
        ///		Null if player isn't SCP-106 or SCP-106 hasn't created one.
        /// </returns>
        public abstract Vector3? Get106Portal();
        public abstract void HideTag(bool enable);
        public abstract void Kill(DamageType type = DamageType.NONE);
        public abstract void PersonalBroadcast(ushort duration, string message, BroadcastFlag broadcastFlag);
        public abstract void PersonalClearBroadcasts();
        public abstract bool IsHandcuffed();
        public abstract void HandcuffPlayer(Player target);
        public abstract void RemoveHandcuffs();
        /// <summary>
        ///		Gets the amount of ammo.
        /// </summary>
        /// <param name="type">
        ///     Type of ammo to get the amount of.
        /// </param>
        public abstract int GetAmmo(AmmoType type);
        public abstract void SetAmmo(AmmoType type, uint amount);
        public abstract void SetRank(ColorType? color = null, string? text = null, string? group = null);
        /// <summary>
        ///     Sends a message with a specific color to the game console.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Message is null.
        /// </exception>
        /// <remarks>
        ///     Not all colors support, look at <see cref="ColorTypeExtension.ColorsConsole"/>.
        /// </remarks>
        public abstract void SendGameConsoleMessage(string message, ColorType color = ColorType.GREEN);
        /// <summary>
        ///     Sends a message to the remote admin console.
        /// </summary>
        /// <param name="isSuccess">
        ///     Defines the color of the message,
        ///     true - successful - green; false - not successful - red.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Message is null.
        /// </exception>
        /// <remarks>
        ///     The message is sent as is,
        ///     without a tag, the tag is separated by <c>#</c> (number sign),
        ///     example <c>MY_TAG#MY_CONTENT</c>.
        /// </remarks>
        public abstract void SendRemoteAdminConsoleMessage(string message, bool isSuccess = true);
        /// <summary>
        ///     Teleports a player.
        /// </summary>
        /// <param name="rot">
        ///     Y axis of rotation.
        /// </param>
        /// <param name="forceGround">
        ///     From a distance of 100f teleports the player to the ground.
        /// </param>
        public abstract void Teleport(Vector3 pos, float rot = 0f, bool forceGround = false);
        /// <summary>
        ///     Throws a grenade from a player.
        /// </summary>
        /// <param name="isSlowThrow">
        ///     Determines to throw slowly or quickly.
        ///     Comparable to far and near throw.
        /// </param>
        /// <remarks>
        ///     There is a RateLimit, if used too often,
        ///     it'll not always work.
        /// </remarks>
        public abstract void ThrowGrenade(GrenadeType grenadeType, bool isSlowThrow = false);

        public abstract GameObject GetGameObject();

        #endregion
    }

    /// <summary>
    ///     Player inventory.
    /// </summary>
    public abstract class Inventory
    {
        /// <summary>
        ///     The number of items in the inventory.
        /// </summary>
        public uint Count { get; }

        /// <summary>
        ///     Gets the current item type in the player’s hand.
        /// </summary>
        public ItemType CurrentItemType { get; }

        /// <summary>
        ///		Gets the current item in the player's hand.
        /// </summary>
        /// <returns>
        ///		Null if the player's hands are empty.
        /// </returns>
        public InventoryItemInfo? CurrentItem { get; }

        /// <summary>
        ///		Gets the player's inventory.
        /// </summary>
        public abstract IList<InventoryItemInfo> GetInventory();

        /// <summary>
        ///		Gives the item to the player.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Wrong item type.
        /// </exception>
        /// <returns>
        ///		<see cref="InventoryItemInfo"/> if the item was put in inventory,
        ///		otherwise <see cref="ISurfaceItemInfo"/>
        ///		because the item spawned in the player’s position when the inventory was full.
        ///	</returns>
        public abstract ItemInfo GiveItem(ItemType type);

        /// <summary>
        ///     Gets a boolean if the item is in the inventory.
        /// </summary>
        public abstract bool HasItem(ItemType type);
    }

    /// <summary>
    ///     Player invisibility settings.
    /// </summary>
    public readonly struct GhostSettings
    {
        /// <remarks>
        ///     Without it, nothing will work.
        /// </remarks>
        public bool Enabled { get; }
        public bool VisibleToSpec { get; }
        public bool VisibleWhenTalking { get; }

        public GhostSettings(bool enabled = false, bool visibleToSpec = true, bool visibleWhenTalking = true)
        {
            Enabled = enabled;
            VisibleToSpec = visibleToSpec;
            VisibleWhenTalking = visibleWhenTalking;
        }
    }
}
