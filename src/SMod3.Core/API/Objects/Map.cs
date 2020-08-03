using System;
using System.Collections.ObjectModel;

using UnityEngine;

namespace SMod3.API
{
    public abstract class Map
    {
        #region Properties

        /// <summary>
        ///     Gets all existing items in the game.
        /// </summary>
        public abstract ReadOnlyCollection<Item> Items { get; }

        /// <summary>
        ///   Gets all the doors on the map.
        /// </summary>
        public abstract ReadOnlyCollection<Door> Doors { get; }

        /// <summary>
        ///     Gets all existing roles.
        /// </summary>
        public abstract ReadOnlyCollection<RoleData> Roles { get; }

        /// <summary>
        ///     Gets all the generators.
        /// </summary>
        public abstract ReadOnlyCollection<Generator> Generators { get; }

        /// <summary>
        ///     Gets all tesla gates.
        /// </summary>
        public abstract ReadOnlyCollection<TeslaGate> TeslaGates { get; }

        /// <summary>
        ///     Gets all the elevators.
        /// </summary>
        public abstract ReadOnlyCollection<Elevator> Elevators { get; }

        /// <summary>
        ///     Gets all the rooms.
        /// </summary>
        public abstract ReadOnlyCollection<Room> Rooms { get; }

        /// <summary>
        ///     Gets all exits from the pocket dimension.
        /// </summary>
        public abstract ReadOnlyCollection<PocketDimensionExit> PocketDimensionExits { get; }

        /// <summary>
        ///     Gets a warhead manager.
        /// </summary>
        public abstract Warhead Warhead { get; }

        public abstract bool LCZDecontaminated { get; }

        /// <summary>
        ///     Gets or sets the activation status.
        /// </summary>
        /// <remarks>
        ///     When FemurBreaker is enabled, SCP-106 can't be contained. This might break the lure configs and mechanism.
        /// </remarks>
        public abstract bool FemurBreaker { get; set; }

        /// <summary>
        ///     Gets or sets the current intercom content.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Content is null.
        /// </exception>
        public abstract string IntercomContent { get; set; }

        /// <summary>
        ///     Gets or sets the speaker of the intercom.
        /// </summary>
        /// <remarks>
        ///     When set to null, current intercom usage will be interrupted.
        /// </remarks>
        public abstract Player? IntercomSpeaker { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets an item by its type.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Type is <see cref="ItemType.NONE"/> or no such type.
        /// </exception>
        public abstract Item GetItem(ItemType type);

        /// <summary>
        ///     Gets items on the map and in the players inventory.
        /// </summary>
        /// <remarks>
        ///     Not to be confused with <see cref="GetItem(ItemType)"/> and <see cref="Items"/>,
        ///     these are static objects that are in the game, not those that the players have.
        /// </remarks>
        public abstract ItemInfo[] GetItems();

        /// <summary>
        ///     Gets surface items.
        /// </summary>
        /// <remarks><inheritdoc cref="GetItems"/></remarks>
        public abstract ISurfaceItemInfo[] GetSurfaceItems();

        /// <summary>
        ///     Gets all items from players inventory.
        /// </summary>
        public abstract InventoryItemInfo[] GetInventoryItems();

        /// <summary>
        ///     Gets a random spawn point for a specific role.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     RoleType is <see cref="RoleType.NONE"/>.
        /// </exception>
        public abstract Vector3 GetRandomSpawnPoint(RoleType role);

        /// <summary>
        ///     Gets random spawn points for a specific role.
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetRandomSpawnPoint(RoleType)"/></exception>
        public abstract Vector3[] GetSpawnPoints(RoleType role);

        /// <summary>
        ///     Broadcasts to all players a message.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Message is null or empty.
        /// </exception>
        public abstract void Broadcast(ushort duration, string message, BroadcastFlag broadcastFlag);

        /// <summary>
        ///     Clears the entire broadcast queue for players.
        /// </summary>
        public abstract void ClearBroadcasts();

        /// <summary>
        ///     Spawns an item.
        /// </summary>
        /// <param name="type">
        ///     Item type.
        /// </param>
        /// <param name="position">
        ///     Item position.
        /// </param>
        /// <param name="rotation">
        ///     Item rotation, if null then used <see cref="Quaternion.identity"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     ItemType is <see cref="ItemType.NONE"/>.
        /// </exception>
        public abstract ISurfaceItemInfo SpawnItem(ItemType type, Vector3 position, Quaternion? rotation = null);

        /// <summary><inheritdoc cref="SpawnItem(ItemType, Vector3, Quaternion?)"/></summary>
        /// <param name="type">
        ///     Weapon type.
        /// </param>
        /// <param name="ammo">
        ///     Ammunition in the clip.
        /// </param>
        /// <param name="pos"><inheritdoc cref="SpawnItem(ItemType, Vector3, Quaternion?)"/></param>
        /// <param name="rotation"><inheritdoc cref="SpawnItem(ItemType, Vector3, Quaternion?)"/></param>
        public abstract ISurfaceItemInfo SpawnItem(WeaponType type, uint ammo, WeaponSight sight, WeaponBarrel barrel, WeaponOther other, Vector3 pos, Quaternion? rotation = null);

        /// <summary><inheritdoc cref="SpawnItem(ItemType, Vector3, Quaternion?)"/></summary>
        /// <param name="type">
        ///     Ammo type.
        /// </param>
        /// <param name="ammo">
        ///     Ammo amount.
        /// </param>
        /// <param name="position"><inheritdoc cref="SpawnItem(ItemType, Vector3, Quaternion?)"/></param>
        /// <param name="rotation"><inheritdoc cref="SpawnItem(ItemType, Vector3, Quaternion?)"/></param>
        /// <exception cref="ArgumentException">
        ///     AmmoType is <see cref="AmmoType.NONE"/>.
        /// </exception>
        public abstract ISurfaceItemInfo SpawnItem(AmmoType type, uint ammo, Vector3 position, Quaternion? rotation = null);

        /// <summary>
        ///     Sets intercom content with status.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Content is null.
        /// </exception>
        public abstract void SetIntercomContent(IntercomStatus intercomStatus, string content);

        /// <summary>
        ///     Gets intercom content by status.
        /// </summary>
        public abstract string GetIntercomContent(IntercomStatus intercomStatus);

        /// <summary>
        ///     Announces spawn NTF.
        /// </summary>>
        public abstract void AnnounceNtfEntrance(int scpsLeft, int mtfNumber, char mtfLetter);

        /// <summary>
        ///     Announces the killing of SCP.
        /// </summary>
        public abstract void AnnounceScpKill(RoleType scp, Player? killer = null);

        /// <summary>
        ///     Announces custom message.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Message is null or empty.
        /// </exception>
        public abstract void AnnounceCustomMessage(string message);

        /// <summary>
        ///     Custom throws a grenade.
        /// </summary>
        /// <param name="position">
        ///     Grenade starting position.
        /// </param>
        /// <param name="direction">
        ///     Grenade direction, calculated by forward from the player.
        /// </param>
        /// <param name="throwForce">
        ///     Grenade throw force,
        ///     is 0.5f for slow feed and 1f for normal feed.
        /// </param>
        /// <param name="delay">
        ///     Grenade throw delay.
        /// </param>
        public abstract void ThrowGrenade(GrenadeType grenadeType, Vector3 position, Vector3 direction, float throwForce, float delay);

        #endregion
    }

    public abstract class Warhead
    {
        #region Properties

        /// <summary>
        ///     Determines if a warhead has been detonated.
        /// </summary>
        public abstract bool Detonated { get; }

        /// <summary>
        ///     Indicates Nukesite panel activity.
        /// </summary>
        public abstract bool WarheadLeverEnabled { get; set; }

        /// <summary>
        ///     Indicates whether a card is inserted in the Outside panel.
        /// </summary>
        public abstract bool WarheadKeycardEntered { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Activates warhead.
        /// </summary>
        public abstract void Activate();

        /// <summary>
        ///     Deactivates warhead.
        /// </summary>
        public abstract void Deactivate();

        /// <summary>
        ///     Detonates warhead.
        /// </summary>
        public abstract void Detonate();

        /// <summary>
        ///     Shakes the camera to the players.
        /// </summary>
        public abstract void Shake();

        #endregion
    }

    public abstract class TeslaGate : IGenericApiObject
    {
        #region Properties

        /// <summary>
        ///     Tesla gate position.
        /// </summary>
        public abstract Vector3 Position { get; }

        /// <summary>
        ///     Tesla gate activation distance.
        /// </summary>
        /// <remarks>
        ///     <c>2.2</c> by default.
        /// </remarks>
        public abstract float TriggerDistance { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Activates tesla sate.
        /// </summary>
        /// <param name="instant">
        ///     Activates the gate instantly (SCP-079 effect).
        /// </param>
        public abstract void Activate(bool instant = false);

        public abstract GameObject GetGameObject();

        #endregion
    }

    public abstract class Elevator : IGenericApiObject
    {
        #region Properties

        /// <summary>
        ///     Evevator type.
        /// </summary>
        public abstract ElevatorType Type { get; }

        /// <summary>
        ///     Current elevator status.
        /// </summary>
        public abstract ElevatorStatus Status { get; }

        /// <summary>
        ///     Gets or sets the elevator lock to use.
        /// </summary>
        public abstract bool Locked { get; set; }

        /// <summary>
        ///     Gets or sets whether to lock the elevator after decontamination.
        /// </summary>
        /// <remarks>
        ///     The elevator goes up and locks.
        /// </remarks>
        public abstract bool Lockable { get; set; }

        public abstract Vector3 DownPosition { get; }
        public abstract Vector3 UpPosition { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets or sets the elevator speed.
        /// </summary>
        public abstract float MovingSpeed { get; set; }

        public abstract void Use();
        public abstract GameObject GetGameObject();

        #endregion
    }

    public abstract class PocketDimensionExit : IGenericApiObject
    {
        public abstract PocketDimensionExitType ExitType { get; set; }

        public abstract Vector3 Position { get; }

        public abstract GameObject GetGameObject();
    }

    public abstract class Room : IGenericApiObject
    {
        /// <summary>
        ///     Room zone type.
        /// </summary>
        public abstract ZoneType Zone { get; }

        public abstract Transform Transform { get; }

        // <summary>
        //     Scp079 camera if exists.
        // </summary>
        public abstract Scp079Camera? Camera { get; }

        /// <summary>
        ///     Turns off the light for duration.
        /// </summary>
        public abstract void FlickerLights(float duration = 8f);

        public abstract GameObject GetGameObject();
    }

    public abstract class Generator : IGenericApiObject
    {
        public abstract bool Open { get; set; }
        public abstract bool Locked { get; }
        public abstract bool HasTablet { get; set; }
        public abstract bool Engaged { get; }
        public abstract float StartTime { get; }
        public abstract float TimeLeft { get; set; }
        public abstract Vector3 Position { get; }
        public abstract Room Room { get; }

        public abstract void Unlock();

        public abstract GameObject GetGameObject();
    }
}
