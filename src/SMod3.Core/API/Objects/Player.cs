using System;
using System.Collections.ObjectModel;

using UnityEngine;

namespace SMod3.API
{
    public abstract class BaseScpController { }

    public abstract class BasePlayerEffect
    {
        /// <remarks>
        ///     Disabling simply sets the intensity to zero.
        /// </remarks>
        public abstract bool Enabled { get; set; }

        public abstract byte Intensity { get; set; }
    }

    /// <exception cref="InvalidOperationException">
    ///     Player object was destroyed.
    /// </exception>
    public abstract class Player : ICommandSender, IGenericApiObject, IComparable<Player>, IEquatable<Player>
    {
        #region Abstract properties

        public abstract bool DoNotTrack { get; }
        /// <summary>
        ///     Gets the player’s connection status,
        ///     whether it's on the server or its object is already destroyed.
        /// </summary>
        public abstract bool IsConnected { get; }
        public abstract Connection Connection { get; }
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
        public abstract RoleData RoleData { get; }
        /// <summary>
        ///		Gets a player's UserGroup from the player's rank.
        /// </summary>
        /// <returns>
        ///		Null If the player doesn't have a rank.
        /// </returns>
        public abstract IUserGroup? UserGroup { get; }
        public abstract Inventory Inventory { get; }
        public abstract ReadOnlyCollection<BaseScpController> ScpControllers { get; }
        public abstract ReadOnlyCollection<BasePlayerEffect> PlayerEffects { get; }

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

        #region Abstract methods

        public abstract void AddHealth(float amount);
        public abstract bool Ban(uint duration, string? message = null, bool isGlobalBan = false);
        /// <summary>
        ///     Assigns the class of the player.
        /// </summary>
        /// <param name="full">
        ///     Determines whether this is a full role change.
        ///     Restores stamina.
        ///     Gives starting class items, requires false on <paramref name="lite"/>.
        ///     Keeps current items, requires true on <paramref name="escape"/> and on `KeepItemsAfterEscaping`.
        /// /// </param>
        /// <param name="lite">
        ///     Determines whether this is a lite role change.
        ///     Affects the player's movement to the role's respawn position.
        /// </param>
        /// <param name="escape">
        ///     Determines if this is a role change for the escaped player.
        ///     Affects the dropping of ammo when `KeepItemsAfterEscaping` is true, requires false on <paramref name="lite"/>.
        /// </param>
        /// <remarks>
        ///     `KeepItemsAfterEscaping` - is the server configuration parameter.
        /// </remarks>
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

        /// <inheritdoc />
        public abstract GameObject GetGameObject();

        #endregion

        #region Methods

        public T? GetPlayerEffect<T>() where T : BasePlayerEffect
        {
            TryGetPlayerEffect<T>(out var ef);
            return ef;
        }

        public bool TryGetPlayerEffect<T>(out T? effect) where T : BasePlayerEffect
        {
            for (var z = 0; z < PlayerEffects.Count; z++)
            {
                if (PlayerEffects[z] is T ef)
                {
                    effect = ef;
                    return true;
                }
            }

            effect = null;
            return false;
        }

        public T? GetPlayerScpController<T>() where T : BaseScpController
        {
            TryGetPlayerScpController<T>(out var sc);
            return sc;
        }

        public bool TryGetPlayerScpController<T>(out T? controller) where T : BaseScpController
        {
            for (var z = 0; z < ScpControllers.Count; z++)
            {
                if (ScpControllers[z] is T sc)
                {
                    controller = sc;
                    return true;
                }
            }

            controller = null;
            return false;
        }

        /// <inheritdoc />
        public int CompareTo(Player other) => PlayerId.CompareTo(other.PlayerId);

        /// <inheritdoc />
        public bool Equals(Player other) => Equals(other, true);

        /// <summary>
        ///     Soft equals without checking the PlayerId.
        /// </summary>
        public bool SoftEquals(Player other) => Equals(other, false);

        private bool Equals(Player other, bool idCheck) => !(other is null) && (PlayerId == other.PlayerId || !idCheck) && UserId == other.UserId && Connection.IpAddress == other.Connection.IpAddress;

        #endregion

        #region Standard

        public static bool operator ==(Player ply1, Player ply2)
        {
            return ply1.Equals(ply2);
        }

        public static bool operator !=(Player ply1, Player ply2)
        {
            return !(ply1 == ply2);
        }

        public override bool Equals(object obj)
        {
            return obj is Player ply && this == ply;
        }

        public override int GetHashCode()
        {
            const int PRIME_OF_SUFFICIENT_SIZE = 397;

            unchecked
            {
                var hash = PlayerId.GetHashCode();
                hash = (hash * PRIME_OF_SUFFICIENT_SIZE) ^ UserId.GetHashCode();
                hash = (hash * PRIME_OF_SUFFICIENT_SIZE) ^ Connection.IpAddress.GetHashCode();
                hash = (hash * PRIME_OF_SUFFICIENT_SIZE) ^ Nickname.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            const string SEPARACTOR = "::";
            return string.Concat(Nickname.Replace(":", string.Empty), SEPARACTOR, Connection.IpAddress, SEPARACTOR, UserId, SEPARACTOR, PlayerId);
        }

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
        public abstract uint Count { get; }

        /// <summary>
        ///     Gets the current item type in the player’s hand.
        /// </summary>
        public abstract ItemType CurrentItemType { get; set; }

        /// <summary>
        ///		Gets the current item in the player's hand.
        /// </summary>
        /// <returns>
        ///		Null if the player's hands are empty.
        /// </returns>
        public abstract InventoryItemInfo? CurrentItem { get; }

        /// <summary>
        ///		Gets the player's inventory.
        /// </summary>
        public abstract ReadOnlyCollection<InventoryItemInfo> GetInventory();

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

    public abstract class RoleData
    {
        public abstract TeamType Team { get; }
        public abstract RoleType Role { get; }

        public abstract string Name { get; }

        public abstract float WalkSpeed { get; }
        public abstract float RunSpeed { get; }
        public abstract float JumpSpeed { get; }

        public abstract int MaxHP { get; }
        public abstract ReadOnlyDictionary<AmmoType, uint> DefaultAmmo { get; }
        public abstract ReadOnlyDictionary<AmmoType, uint> MaxAmmo { get; }
    }

    /// <summary>
    ///     Player invisibility settings.
    /// </summary>
    public readonly struct GhostSettings
    {
        /// <summary>
        ///     Default settings.
        /// </summary>
        public static readonly GhostSettings Default = new GhostSettings(false, false, false);

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
