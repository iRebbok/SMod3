using SMod3.Module.Command.Meta;
using System.Collections.Generic;
using UnityEngine;

namespace SMod3.API
{
    public abstract class Player : ICommandSender
    {
        /// <summary>
        ///		Defines the trigger for the player spawn event.
        ///		<para>
        ///			Assigned once for the player's connection to the server.
        ///			Then the value disappears due to creating a new class.
        ///		</para>
        /// </summary>
        public bool CallSetRoleEvent { get; set; } = true;

        public abstract bool BypassMode { get; set; }
        public abstract bool DoNotTrack { get; }
        /// <summary>
        ///		Gets/Sets the ghost mode for the player.
        /// </summary>
        public bool GhostMode { get; set; }
        /// <summary>
        ///		Gets/Sets the god mode for the player.
        /// </summary>
        public bool GodMode { get; set; }
        public abstract bool IsConnected { get; }
        public abstract string IpAddress { get; }
        public abstract bool IntercomMuted { get; set; }
        public abstract bool Muted { get; set; }
        public abstract string Nickname { get; }
        public abstract bool OverwatchMode { get; set; }
        public abstract int PlayerId { get; }
        public abstract int Ping { get; }
        public abstract RadioStatus RadioStatus { get; set; }
        public abstract Vector3 Rotation { get; set; }
        public abstract Scp079Data Scp079Data { get; }
        public abstract Scp268Data Scp268Data { get; }
        public abstract TeamRole TeamRole { get; }
        public abstract string UserId { get; }


        public abstract void AddHealth(float amount);
        public abstract void Ban(int duration);
        public abstract void Ban(int duration, string message);
        public abstract void Ban(int duration, string message, bool isGlobalBan);
        public abstract void ChangeRole(RoleType role, bool full = true, bool spawnTeleport = true, bool spawnProtect = true, bool removeHandcuffs = false);
        public abstract void ConfigureGhostSettings(bool visibleToSpec = true, bool visibleWhenTalking = true);
        public abstract void Damage(float amount, DamageType type = DamageType.NONE);
        public abstract void Disconnect(string message = null);
        /// <summary>
        ///		Gets the amount of ammo.
        /// </summary>
        /// <param name="type">Type of ammo to get the amount of.</param>
        public abstract int GetAmmo(AmmoType type);
        /// <summary>
        ///		Gets the current index of the item in the player's inventory.
        /// </summary>
        /// <returns>
        ///		-1 if there is no item in the hand.
        /// </returns>
        public abstract int GetCurrentItemIndex();
        /// <summary>
        ///		Gets the item index of the item in the player's inventory.
        /// </summary>
        /// <param name="type">Type of item to get the index of.</param>
        /// <returns>
        ///		-1 if the item isn't in the inventory.
        ///	</returns>
        public abstract int GetItemIndex(ItemType type);
        /// <summary>
        ///		Gets the current health of the player.
        /// </summary>
        public abstract float GetHealth();
        /// <summary>
        ///		Gets the authentication token of the player.
        /// </summary>
        public abstract string GetAuthToken();
        /// <summary>
        ///		Gets the name of the player's group. Not to be confused with group badge.
        /// </summary>
        public abstract string GetRankName();
        /// <summary>
        ///		Gets the player's inventory.
        /// </summary>
        public abstract IEnumerable<Item> GetInventory();
        /// <summary>
        ///		Get the current item in the player's hand.
        /// </summary>
        /// <returns>
        ///		Null if the player's hands are empty.
        /// </returns>
        public abstract Item GetCurrentItem();
        /// <summary>
        ///		Gives the item to the player.
        /// </summary>
        /// <returns>
        ///		A new item in the player's inventory.
        ///	</returns>
        public abstract Item GiveItem(ItemType type);
        /// <summary>
        ///		Gets the current position of the player.
        /// </summary>
        public abstract Vector3 GetPosition();
        /// <summary>  
        ///		Gets SCP-106's portal position.
        /// </summary>
        /// <returns>
        ///		Null if Player is not SCP-106 or SCP-106 hasn't created one.
        /// </returns>
        public abstract Vector3 Get106Portal();
        /// <summary>
        ///		Gets a player's UserGroup from the player's rank.
        /// </summary>
        /// <returns>
        ///		Null If the player doesn't have a rank.
        /// </returns>
        public abstract UserGroup GetUserGroup();
        /// <summary>
        ///		Gets the player's GameObject.
        /// </summary>
        /// <remarks>
        ///		It is recommended to use the `(Player as PlayerImpl).gameObject` to avoid packing.
        /// </remarks>
        public abstract object GetGameObject();
        public abstract bool HasItem(ItemType type);
        public abstract void HideTag(bool enable);
        public abstract void HandcuffPlayer(Player playerToHandcuff);
        public abstract void Infect();
        public abstract bool IsHandcuffed();
        public abstract void Kill(DamageType type = DamageType.NONE);
        public abstract void PersonalBroadcast(uint duration, string message, bool isMonoSpaced);
        public abstract void PersonalClearBroadcasts();
        public abstract void RemoveHandcuffs();
        public abstract void SetAmmo(AmmoType type, int amount);
        public abstract void SetRank(ColorType? color = null, string text = null, string group = null);
        public abstract void SetCurrentItem(ItemType type);
        public abstract void SetCurrentItemIndex(int index);
        public abstract void SetGodmode(bool godmode);
        public abstract void SendConsoleMessage(string message, ColorType color = ColorType.GREEN);
        public abstract void SetHealth(float amount, DamageType type = DamageType.NUKE);
        public abstract void SetRadioBattery(int battery);
        public abstract void Teleport(Vector3 pos, bool unstuck = false);
        public abstract void Teleport(Vector3 pos, float rot, bool unstuk = false);
        public abstract void ThrowGrenade(GrenadeType grenadeType, float? throwForce = null, bool isSlowThrow = false, Vector3 customDirection = default(Vector3), Vector3 customPosition = default(Vector3));
    }

    public abstract class Scp079Data
    {
        public abstract float Exp { get; set; }
        public abstract int ExpToLevelUp { get; set; }
        public abstract int Level { get; set; }
        public abstract float AP { get; set; }
        public abstract float APPerSecond { get; set; }
        public abstract float MaxAP { get; set; }
        public abstract float SpeakerAPPerSecond { get; set; }
        public abstract float LockedDoorAPPerSecond { get; set; }
        public abstract float Yaw { get; }
        public abstract float Pitch { get; }
        public abstract Room Speaker { get; set; }
        public abstract Vector3 Camera { get; } //todo: implement api object

        public abstract Door[] GetLockedDoors();
        public abstract void Lock(Door door);
        public abstract void Unlock(Door door);
        public abstract void TriggerTesla(TeslaGate tesla);
        public abstract void Lockdown(Room room);
        public abstract void SetCamera(Vector3 position, bool lookAt = false);
        public abstract void ShowGainExp(ExperienceType expType);
        public abstract void ShowLevelUp(int level);
        public abstract object GetComponent();
    }

    public abstract class Scp268Data
    {
        public abstract bool Enabled { get; set; }
        public abstract float CurrentTime { get; set; }
        public abstract uint? UsingTime { get; set; } // If you set it to null, the usage time will be infinite
    }
}
