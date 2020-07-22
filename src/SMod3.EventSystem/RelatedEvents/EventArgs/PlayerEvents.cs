using System.Collections.Generic;

using SMod3.API;
using SMod3.Module.EventSystem.Background;

using UnityEngine;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Events
{
    #region Player behavior

    public sealed class PlayerJoinEvent : PlayerEvent
    {
        internal override void Reset()
        {
            Player = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerJoinEvent>(other);
            target.Player = Player;
        }
    }

    public sealed class PlayerLeaveEvent : PlayerEvent
    {
        internal override void Reset()
        {
            Player = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerLeaveEvent>(other);
            target.Player = Player;
        }
    }

    public sealed class PlayerReadyEvent : PlayerEvent
    {
        internal override void Reset()
        {
            Player = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerReadyEvent>(other);
            target.Player = Player;
        }
    }

    public sealed class PlayerNicknameSetEvent : PlayerEvent
    {
        /// <remarks>
        ///     Cannot be assigned due to restriction caused by DisplayNickname.
        /// </remarks>
        public string Nickname { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Nickname = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerNicknameSetEvent>(other);
            target.Nickname = Nickname;
            target.Player = Player;
        }
    }

    #endregion

    #region Health damage events

    public sealed class PlayerHurtEvent : PlayerEvent, IAllowable
    {
        public Player Attacker { get; internal set; }
        public float Damage { get; set; }
        public DamageType DamageType { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Attacker = null;
            Damage = default;
            DamageType = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerHurtEvent>(other);
            target.Player = Player;
            target.Attacker = Attacker;
            target.Damage = Damage;
            target.DamageType = DamageType;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerDeathEvent : PlayerEvent, IAllowable
    {
        public Player? Killer { get; internal set; }
        /// <summary>
        ///     Determines whether or not to spawn a ragdoll.
        /// </summary>
        public bool Allow { get; set; }
        public DamageType DamageType { get; set; }

        internal override void Reset()
        {
            Player = null;
            Killer = null;
            Allow = true;
            DamageType = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerDeathEvent>(other);
            target.Player = Player;
            target.Killer = Killer;
            target.Allow = Allow;
            target.DamageType = DamageType;
        }
    }

    public sealed class PlayerInfectedEvent : PlayerEvent, IAllowable
    {
        public float Damage { get; set; }
        public Player Attacker { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Damage = default;
            Attacker = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerInfectedEvent>(other);
            target.Player = Player;
            target.Damage = Damage;
            target.Attacker = Attacker;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerLureEvent : PlayerEvent, IAllowable
    {
        /// <summary>
        ///     Determines if player death is allowed.
        /// </summary>
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerLureEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    #endregion

    #region Item events

    public sealed class PlayerPickupItemEvent : PlayerItemEvent
    {
        public new ISurfaceItemInfo Item { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Item = null;
            ChangeTo = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerPickupItemEvent>(other);
            target.Item = Item;
            target.Player = Player;
            target.ChangeTo = ChangeTo;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerPickupItemLateEvent : PlayerEvent
    {
        public InventoryItemInfo Item { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Item = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerPickupItemLateEvent>(other);
            target.Player = Player;
            target.Item = Item;
        }
    }

    public sealed class PlayerDropItemEvent : PlayerItemEvent
    {
        public new InventoryItemInfo Item { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Item = null;
            ChangeTo = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerDropItemEvent>(other);
            target.Item = Item;
            target.Player = Player;
            target.ChangeTo = ChangeTo;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerDropItemLateEvent : PlayerEvent
    {
        public ISurfaceItemInfo Item { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Item = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerDropItemLateEvent>(other);
            target.Item = Item;
            target.Player = Player;
        }
    }

    public sealed class PlayerDropItemsEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }
        public bool DropAmmo { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            DropAmmo = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerDropItemsEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.DropAmmo = DropAmmo;
        }
    }

    public sealed class PlayerCurrentItemUpdateEvent : PlayerEvent
    {
        public ItemType Type { get; internal set; }
        public ItemType PrevType { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Type = default;
            PrevType = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerCurrentItemUpdateEvent>(other);
            target.Player = Player;
            target.Type = Type;
            target.PrevType = PrevType;
        }
    }

    #endregion

    #region Roles events

    public sealed class PlayerAssignTeamEvent : PlayerEvent
    {
        public TeamType Team { get; set; }

        internal override void Reset()
        {
            Player = null;
            Team = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerAssignTeamEvent>(other);
            target.Player = Player;
            target.Team = Team;
        }
    }

    public sealed class PlayerCheckEscapeEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }
        public RoleType ChangeTo { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            ChangeTo = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerCheckEscapeEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.ChangeTo = ChangeTo;
        }
    }

    public sealed class PlayerSetRoleEvent : PlayerEvent
    {
        /// <summary>
        ///     Contains an initial list of items by default.
        /// </summary>
        public List<ItemType> Items { get; }
        public RoleType Role { get; set; }

        public PlayerSetRoleEvent()
        {
            Items = new List<ItemType>();
        }

        internal override void Reset()
        {
            Player = null;
            Role = default;
            Items.Clear();
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerSetRoleEvent>(other);
            target.Player = Player;
            target.Items.AddRange(Items);
            target.Role = Role;
        }
    }

    public sealed class PlayerSpawnEvent : PlayerEvent
    {
        public Vector3 Position { get; set; }

        internal override void Reset()
        {
            Player = null;
            Position = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerSpawnEvent>(other);
            target.Player = Player;
            target.Position = Position;
        }
    }

    #endregion

    #region Interaction events

    public sealed class PlayerThrowGrenadeEvent : PlayerEvent, IAllowable
    {
        public GrenadeType GrenadeType { get; set; }
        public Vector3 Direction { get; set; }
        public bool SlowThrow { get; set; }
        public bool RemoveItem { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            GrenadeType = default;
            Direction = default;
            SlowThrow = default;
            RemoveItem = true;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerThrowGrenadeEvent>(other);
            target.Player = Player;
            target.GrenadeType = GrenadeType;
            target.Direction = Direction;
            target.SlowThrow = SlowThrow;
            target.RemoveItem = RemoveItem;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerDoorAccessEvent : PlayerEvent
    {
        public Door Door { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Door = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerDoorAccessEvent>(other);
            target.Player = Player;
            target.Door = Door;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerIntercomInteractEvent : PlayerEvent
    {
        public float SpeechTime { get; set; }
        public float CooldownTime { get; set; }

        internal override void Reset()
        {
            Player = null;
            SpeechTime = default;
            CooldownTime = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerIntercomInteractEvent>(other);
            target.Player = Player;
            target.SpeechTime = SpeechTime;
            target.CooldownTime = CooldownTime;
        }
    }

    public sealed class PlayerIntercomCooldownCheckEvent : PlayerEvent
    {
        public float CurrentCooldown { get; set; }

        internal override void Reset()
        {
            Player = null;
            CurrentCooldown = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerIntercomCooldownCheckEvent>(other);
            target.Player = Player;
            target.CurrentCooldown = CurrentCooldown;
        }
    }

    public sealed class PlayerMedicalUseEvent : PlayerEvent
    {
        public int AmountHealth { get; set; }
        public int AmountArtificial { get; set; }
        public int AmountRegen { get; internal set; }
        public ItemType MedicalItem { get; internal set; } //todo: change to MedicalType

        internal override void Reset()
        {
            Player = null;
            AmountHealth = default;
            AmountArtificial = default;
            AmountRegen = default;
            MedicalItem = ItemType.NONE;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerMedicalUseEvent>(other);
            target.Player = Player;
            target.AmountHealth = AmountHealth;
            target.AmountArtificial = AmountArtificial;
            target.AmountRegen = AmountRegen;
            target.MedicalItem = MedicalItem;
        }
    }

    public sealed class PlayerShootEvent : PlayerEvent
    {
        public Player Target { get; internal set; }
        //public Weapon Weapon { get; internal set; } //todo
        public bool ShouldSpawnHitmarker { get; set; }
        public bool ShouldSpawnBloodDecal { get; set; }
        public Vector3 SourcePosition { get; internal set; }
        public Vector3 TargetPosition { get; internal set; }
        public string TargetHitbox { get; internal set; }
        public Vector3 Direction { get; set; }
        public WeaponSound? WeaponSound { get; set; }

        internal override void Reset()
        {
            Player = null;
            Target = null;
            //Weapon = default;
            ShouldSpawnHitmarker = default;
            ShouldSpawnBloodDecal = default;
            SourcePosition = default;
            TargetPosition = default;
            TargetHitbox = default;
            Direction = default;
            WeaponSound = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerShootEvent>(other);
            target.Player = Player;
            target.Target = Target;
            target.ShouldSpawnHitmarker = ShouldSpawnHitmarker;
            target.ShouldSpawnBloodDecal = ShouldSpawnBloodDecal;
            target.SourcePosition = SourcePosition;
            target.TargetPosition = TargetPosition;
            target.TargetHitbox = TargetHitbox;
            target.Direction = Direction;
            target.WeaponSound = WeaponSound;
        }
    }

    public sealed class PlayerElevatorUseEvent : PlayerEvent
    {
        public Elevator Elevator { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Elevator = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerElevatorUseEvent>(other);
            target.Player = Player;
            target.Elevator = Elevator;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerHandcuffedEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }
        public Player Target { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            Target = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerHandcuffedEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.Target = Target;
        }
    }

    public sealed class PlayerTriggerTeslaGateEvent : PlayerEvent, IAllowable
    {
        public TeslaGate TeslaGate { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            TeslaGate = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerTriggerTeslaGateEvent>(other);
            target.Player = Player;
            target.TeslaGate = TeslaGate;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerSCP914ChangeKnobEvent : PlayerEvent, IAllowable
    {
        public KnobSetting KnobStatus { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            KnobStatus = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerSCP914ChangeKnobEvent>(other);
            target.Player = Player;
            target.KnobStatus = KnobStatus;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerRadioSwitchEvent : PlayerEvent
    {
        public RadioStatus ChangeTo { get; set; }

        internal override void Reset()
        {
            Player = null;
            ChangeTo = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerRadioSwitchEvent>(other);
            target.Player = Player;
            target.ChangeTo = ChangeTo;
        }
    }

    public sealed class PlayerCallConsoleCommandEvent : PlayerEvent
    {
        public string Query { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            Query = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerCallConsoleCommandEvent>(other);
            target.Player = Player;
            target.Query = Query;
        }
    }

    public sealed class PlayerWeaponReloadEvent : PlayerEvent
    {
        //public Weapon Weapon { get; internal set; } //todo
        public int AmmoRemoved { get; set; }
        public int ClipAmmoCountAfterReload { get; set; }
        public int NormalMaxClipSize { get; internal set; }
        public int CurrentClipAmmoCount { get; internal set; }
        public int CurrentAmmoTotal { get; internal set; }

        internal override void Reset()
        {
            Player = null;
            //Weapon = null;
            AmmoRemoved = default;
            ClipAmmoCountAfterReload = default;
            NormalMaxClipSize = default;
            CurrentClipAmmoCount = default;
            CurrentAmmoTotal = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerWeaponReloadEvent>(other);
            target.Player = Player;
            //target.Weapon = Weapon;
            target.AmmoRemoved = AmmoRemoved;
            target.ClipAmmoCountAfterReload = ClipAmmoCountAfterReload;
            target.NormalMaxClipSize = NormalMaxClipSize;
            target.CurrentClipAmmoCount = CurrentClipAmmoCount;
            target.CurrentAmmoTotal = CurrentAmmoTotal;
        }
    }

    public sealed class PlayerContain106Event : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerContain106Event>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    #endregion

    #region Pocket events

    public sealed class PlayerPocketDimensionExitEvent : PlayerEvent, IAllowable
    {
        public Vector3 ExitPosition { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            ExitPosition = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerPocketDimensionExitEvent>(other);
            target.Player = Player;
            target.ExitPosition = ExitPosition;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerPocketDimensionEnterEvent : PlayerEvent, IAllowable
    {
        public float Damage { get; set; }
        public Vector3 LastPosition { get; internal set; }
        public Vector3 TargetPosition { get; set; }
        public Player Attacker { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Damage = default;
            LastPosition = default;
            TargetPosition = default;
            Attacker = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerPocketDimensionEnterEvent>(other);
            target.Player = Player;
            target.Damage = Damage;
            target.LastPosition = LastPosition;
            target.TargetPosition = TargetPosition;
            target.Attacker = Attacker;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerPocketDimensionDieEvent : PlayerEvent, IAllowable
    {
        /// <summary>
        ///     Determines whether death is allowed.
        /// </summary>
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerPocketDimensionDieEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    #endregion

    #region Environment

    public sealed class PlayerSpawnRagdollEvent : PlayerEvent, IAllowable
    {
        public RoleType Role { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Player Attacker { get; internal set; }
        public DamageType DamageType { get; set; }
        public bool AllowRecall { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Role = default;
            Position = default;
            Rotation = default;
            Attacker = null;
            DamageType = default;
            AllowRecall = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerSpawnRagdollEvent>(other);
            target.Player = Player;
            target.Position = Position;
            target.Rotation = Rotation;
            target.Attacker = Attacker;
            target.DamageType = DamageType;
            target.AllowRecall = AllowRecall;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerMakeNoiseEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }
        /// <summary>
        ///     Compared to the distance to SCP939,
        ///     and if the distance is less than the radius,
        ///     then SCP939 can see the player.
        /// </summary>
        public float NoiseRadius { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            NoiseRadius = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerMakeNoiseEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.NoiseRadius = NoiseRadius;
        }
    }

    public sealed class PlayerGrenadeExplosion : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }
        public GrenadeType Type { get; internal set; }
        public Vector3 Position { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            Type = default;
            Position = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerGrenadeExplosion>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.Type = Type;
            target.Position = Position;
        }
    }

    public sealed class PlayerGrenadeHitPlayer : PlayerEvent, IAllowable
    {
        public Player Victim { get; internal set; }
        public GrenadeType Type { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Victim = null;
            Type = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerGrenadeHitPlayer>(other);
            target.Player = Player;
            target.Victim = Victim;
            target.Type = Type;
            target.Allow = Allow;
        }
    }

    #endregion

    #region Generator events

    public sealed class PlayerGeneratorUnlockEvent : PlayerEvent, IAllowable
    {
        public Generator Generator { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Generator = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerGeneratorUnlockEvent>(other);
            target.Player = Player;
            target.Generator = Generator;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerGeneratorAccessEvent : PlayerEvent, IAllowable
    {
        public Generator Generator { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Generator = null;
            Allow = false; // Depends on permissions, false by default
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerGeneratorAccessEvent>(other);
            target.Player = Player;
            target.Generator = Generator;
            target.Allow = Allow;
        }
    }

    public sealed class PlayerGeneratorInsertTabletEvent : PlayerEvent, IAllowable
    {
        public Generator Generator { get; internal set; }
        public bool Allow { get; set; }
        public bool RemoveTablet { get; set; }

        internal override void Reset()
        {
            Player = null;
            Generator = null;
            Allow = true;
            RemoveTablet = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerGeneratorInsertTabletEvent>(other);
            target.Player = Player;
            target.Generator = Generator;
            target.Allow = Allow;
            target.RemoveTablet = RemoveTablet;
        }
    }

    public sealed class PlayerGeneratorEjectTabletEvent : PlayerEvent, IAllowable
    {
        public Generator Generator { get; internal set; }
        public bool Allow { get; set; }
        public bool SpawnTablet { get; set; }

        internal override void Reset()
        {
            Player = null;
            Generator = null;
            Allow = true;
            SpawnTablet = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<PlayerGeneratorEjectTabletEvent>(other);
            target.Player = Player;
            target.Generator = Generator;
            target.Allow = Allow;
            target.SpawnTablet = SpawnTablet;
        }
    }

    #endregion

    #region Scp 049 events

    public sealed class Scp049RecallZombieEvent : PlayerEvent, IAllowable
    {
        public Player Target { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Target = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp049RecallZombieEvent>(other);
            target.Player = Player;
            target.Target = Target;
            target.Allow = Allow;
        }
    }

    #endregion

    #region Scp106 events

    public sealed class Scp106CreatePortalEvent : PlayerEvent, IAllowable
    {
        /// <summary>
        ///     Means the portal position.
        /// </summary>
        public Vector3 Position { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Position = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp106CreatePortalEvent>(other);
            target.Player = Player;
            target.Position = Position;
            target.Allow = Allow;
        }
    }

    public sealed class Scp106TeleportEvent : PlayerEvent
    {
        /// <summary>
        ///     Means the target position of the teleport;
        ///     by default it's the portal position.
        /// </summary>
        public Vector3 Position { get; set; }

        internal override void Reset()
        {
            Player = null;
            Position = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp106TeleportEvent>(other);
            target.Player = Player;
            target.Position = Position;
        }
    }

    #endregion

    #region Scp 079 events

    public sealed class Scp079DoorAccessEvent : PlayerEvent, IAllowable
    {
        public Door Door { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            Door = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079DoorAccessEvent>(other);
            target.Player = Player;
            target.Door = Door;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079DoorLockEvent : PlayerEvent, IAllowable
    {
        public Door Door { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            Door = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079DoorLockEvent>(other);
            target.Player = Player;
            target.Door = Door;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079ElevatorUseEvent : PlayerEvent, IAllowable
    {
        public Elevator Elevator { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            Elevator = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079ElevatorUseEvent>(other);
            target.Player = Player;
            target.Elevator = Elevator;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079TriggerTeslaGateEvent : PlayerEvent, IAllowable
    {
        public TeslaGate TeslaGate { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            TeslaGate = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079TriggerTeslaGateEvent>(other);
            target.Player = Player;
            target.TeslaGate = TeslaGate;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079AddExpEvent : PlayerEvent, IAllowable
    {
        public ExperienceType Type { get; internal set; }
        public float Experience { get; set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Type = default;
            Experience = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079AddExpEvent>(other);
            target.Player = Player;
            target.Type = Type;
            target.Experience = Experience;
            target.Allow = Allow;
        }
    }

    public sealed class Scp079LevelUpEvent : PlayerEvent
    {
        internal override void Reset()
        {
            Player = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079LevelUpEvent>(other);
            target.Player = Player;
        }
    }

    public sealed class Scp079DoorUnlockEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079DoorUnlockEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    public sealed class Scp079CameraTeleportEvent : PlayerEvent, IAllowable
    {
        //public Vector3 Camera { get; set; } //todo: 079 Camera API
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            //Camera = default;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079CameraTeleportEvent>(other);
            target.Player = Player;
            //target.Camera = Camera;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079StartSpeakerEvent : PlayerEvent, IAllowable
    {
        public Room Room { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            Room = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079StartSpeakerEvent>(other);
            target.Player = Player;
            target.Room = Room;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079StopSpeakerEvent : PlayerEvent, IAllowable
    {
        public Room Room { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Room = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079StopSpeakerEvent>(other);
            target.Player = Player;
            target.Room = Room;
            target.Allow = Allow;
        }
    }

    public sealed class Scp079LockdownEvent : PlayerEvent, IAllowable
    {
        public Room Room { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            Room = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079LockdownEvent>(other);
            target.Player = Player;
            target.Room = Room;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    public sealed class Scp079ElevatorTeleportEvent : PlayerEvent, IAllowable
    {
        //public Vector3 Camera { get; }
        public Elevator Elevator { get; internal set; }
        public bool Allow { get; set; }
        public float APDrain { get; set; }

        internal override void Reset()
        {
            Player = null;
            Elevator = null;
            Allow = true;
            APDrain = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp079ElevatorTeleportEvent>(other);
            target.Player = Player;
            target.Elevator = Elevator;
            target.Allow = Allow;
            target.APDrain = APDrain;
        }
    }

    #endregion

    #region Scp 096 events

    public sealed class Scp096PanicEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }
        public float PanicTime { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            PanicTime = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp096PanicEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.PanicTime = PanicTime;
        }
    }

    public sealed class Scp096EnrageEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp096EnrageEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    public class Scp096CooldownStartEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp096CooldownStartEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    public class Scp096CooldownEndEvent : PlayerEvent, IAllowable
    {
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<Scp096CooldownEndEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    #endregion
}
