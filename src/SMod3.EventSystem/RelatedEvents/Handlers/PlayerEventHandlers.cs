using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.Handlers
{
    #region Player behavior

    /// <summary>
    ///     Called when the player's components are ready
    ///     (the player has connected to the server,
    ///     but hasn't yet sent his nickname and authentication token).
    /// </summary>
    public interface IEventHandlerPlayerJoin : IEventHandler
    {
        void OnPlayerJoin(PlayerJoinEvent ev);
    }

    public interface IEventHandlerPlayerLeave : IEventHandler
    {
        void OnPlayerLeave(PlayerLeaveEvent ev);
    }

    /// <summary>
    ///     Called when the player is ready (authenticated).
    /// </summary>
    public interface IEventHandlerPlayerReady : IEventHandler
    {
        void OnPlayerReady(PlayerReadyEvent ev);
    }

    public interface IEventHandlerPlayerNicknameSet : IEventHandler
    {
        void OnPlayerNicknameSet(PlayerNicknameSetEvent ev);
    }

    #endregion

    #region Health damage events

    /// <summary>
    ///     Called before the player is going to take damage.
    /// </summary>
    public interface IEventHandlerPlayerHurt : IEventHandler
    {
        void OnPlayerHurt(PlayerHurtEvent ev);
    }

    public interface IEventHandlerPlayerDie : IEventHandler
    {
        void OnPlayerDie(PlayerDeathEvent ev);
    }

    /// <summary>
    ///     Called when SCP-049 infects a player.
    /// </summary>
    public interface IEventHandlerPlayerInfected : IEventHandler
    {
        void OnPlayerInfected(PlayerInfectedEvent ev);
    }

    /// <summary>
    ///     Called when a player enters FemurBreaker.
    /// </summary>
    public interface IEventHandlerPlayerLure : IEventHandler
    {
        void OnPlayerLure(PlayerLureEvent ev);
    }

    #endregion

    #region Item events

    public interface IEventHandlerPlayerPickupItem : IEventHandler
    {
        void OnPlayerPickupItem(PlayerPickupItemEvent ev);
    }

    public interface IEventHandlerPlayerPickupItemLate : IEventHandler
    {
        void OnPlayerPickupItemLate(PlayerPickupItemLateEvent ev);
    }

    public interface IEventHandlerPlayerDropItem : IEventHandler
    {
        void OnPlayerDropItem(PlayerDropItemEvent ev);
    }

    public interface IEventHandlerPlayerDropItemLate : IEventHandler
    {
        void OnPlayerDropItemLate(PlayerDropItemLateEvent ev);
    }

    /// <summary>
    ///     Called while dropping all items.
    /// </summary>
    public interface IEventHandlerPlayerDropItems : IEventHandler
    {
        void OnPlayerDropAllItems(PlayerDropItemsEvent ev);
    }

    public interface IEventHandlerPlayerCurrentItemUpdate : IEventHandler
    {
        void OnPlayerCurrentItem(PlayerCurrentItemUpdateEvent ev);
    }

    #endregion

    #region Roles events

    /// <summary>
    ///     Called at the start of a round when assigning roles.
    /// </summary>
    public interface IEventHandlerInitialAssignTeam : IEventHandler
    {
        void OnAssignTeam(PlayerInitialAssignTeamEvent ev);
    }

    public interface IEventHandlerPlayerSetRole : IEventHandler
    {
        void OnPlayerSetRole(PlayerSetRoleEvent ev);
    }

    public interface IEventHandlerPlayerCheckEscape : IEventHandler
    {
        void OnPlayerCheckEscape(PlayerCheckEscapeEvent ev);
    }

    public interface IEventHandlerPlayerSpawn : IEventHandler
    {
        void OnPlayerSpawn(PlayerSpawnEvent ev);
    }

    #endregion

    #region Interaction events

    public interface IEventHandlerPlayerDoorAccess : IEventHandler
    {
        void OnPlayerDoorAccess(PlayerDoorAccessEvent ev);
    }

    public interface IEventHandlerPlayerIntercomInteract : IEventHandler
    {
        void OnPlayerIntercomInteract(PlayerIntercomInteractEvent ev);
    }

    public interface IEventHandlerPlayerIntercomCooldownCheck : IEventHandler
    {
        void OnPlayerIntercomCooldownCheck(PlayerIntercomCooldownCheckEvent ev);
    }

    public interface IEventHandlerPlayerThrowGrenade : IEventHandler
    {
        void OnPlayerThrowGrenade(PlayerThrowGrenadeEvent ev);
    }

    public interface IEventHandlerPlayerMedicalUse : IEventHandler
    {
        void OnPlayerMedkitUse(PlayerMedicalUseEvent ev);
    }

    public interface IEventHandlerPlayerShoot : IEventHandler
    {
        void OnPlayerShoot(PlayerShootEvent ev);
    }

    public interface IEventHandlerPlayerElevatorUse : IEventHandler
    {
        void OnPlayerElevatorUse(PlayerElevatorUseEvent ev);
    }

    public interface IEventHandlerPlayerHandcuffed : IEventHandler
    {
        void OnPlayerHandcuffed(PlayerHandcuffedEvent ev);
    }

    public interface IEventHandlerPlayerTriggerTesla : IEventHandler
    {
        void OnPlayerTriggerTesla(PlayerTriggerTeslaEvent ev);
    }

    public interface IEventHandlerPlayerScp914ChangeKnob : IEventHandler
    {
        void OnPlayerScp914ChangeKnob(PlayerSCP914ChangeKnobEvent ev);
    }

    public interface IEventHandlerPlayerRadioSwitch : IEventHandler
    {
        void OnPlayerRadioSwitch(PlayerRadioSwitchEvent ev);
    }

    public interface IEventHandlerPlayerCallConsoleCommand : IEventHandler
    {
        void OnCallCommand(PlayerCallConsoleCommandEvent ev);
    }

    public interface IEventHandlerPlayerWeaponReload : IEventHandler
    {
        void OnWeaponReload(PlayerWeaponReloadEvent ev);
    }

    /// <summary>
    ///     Called when a player presses the button to contain SCP-106
    /// </summary>
    public interface IEventHandlerPlayerContain106 : IEventHandler
    {
        void OnContain106(PlayerContain106Event ev);
    }

    #endregion

    #region Pocket dimension

    public interface IEventHandlerPlayerPocketDimensionEnter : IEventHandler
    {
        void OnPocketDimensionEnter(PlayerPocketDimensionEnterEvent ev);
    }

    public interface IEventHandlerPlayerPocketDimensionExit : IEventHandler
    {
        void OnPocketDimensionExit(PlayerPocketDimensionExitEvent ev);
    }

    public interface IEventHandlerPlayerPocketDimensionDie : IEventHandler
    {
        void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev);
    }

    #endregion

    #region Environment

    public interface IEventHandlerPlayerSpawnRagdoll : IEventHandler
    {
        void OnSpawnRagdoll(PlayerSpawnRagdollEvent ev);
    }

    public interface IEventHandlerGrenadeExplosion : IEventHandler
    {
        void OnGrenadeExplosion(PlayerGrenadeExplosion ev);
    }

    public interface IEventHandlerGrenadeHitPlayer : IEventHandler
    {
        void OnGrenadeHitPlayer(PlayerGrenadeHitPlayer ev);
    }

    public interface IEventHandlerPlayerMakeNoise : IEventHandler
    {
        void OnMakeNoise(PlayerMakeNoiseEvent ev);
    }

    #endregion

    #region Scp 106

    public interface IEventHandlerScp106CreatePortal : IEventHandler
    {
        void OnScp106CreatePortal(Scp106CreatePortalEvent ev);
    }

    public interface IEventHandlerScp106Teleport : IEventHandler
    {
        void OnScp106Teleport(Scp106TeleportEvent ev);
    }

    #endregion

    #region Scp 049

    public interface IEventHandlerScp049RecallZombie : IEventHandler
    {
        void OnRecallZombie(Scp049RecallZombieEvent ev);
    }

    #endregion

    #region Generator

    public interface IEventHandlerGeneratorUnlock : IEventHandler
    {
        void OnGeneratorUnlock(PlayerGeneratorUnlockEvent ev);
    }

    public interface IEventHandlerGeneratorAccess : IEventHandler
    {
        void OnGeneratorAccess(PlayerGeneratorAccessEvent ev);
    }

    public interface IEventHandlerGeneratorInsertTablet : IEventHandler
    {
        void OnGeneratorInsertTablet(PlayerGeneratorInsertTabletEvent ev);
    }

    public interface IEventHandlerGeneratorEjectTablet : IEventHandler
    {
        void OnGeneratorEjectTablet(PlayerGeneratorEjectTabletEvent ev);
    }

    #endregion

    #region Scp 079

    public interface IEventHandlerScp079Door : IEventHandler
    {
        void OnScp079Door(Scp079DoorInteractEvent ev);
    }

    public interface IEventHandlerScp079Lock : IEventHandler
    {
        void OnScp079Lock(Scp079DoorLockEvent ev);
    }

    public interface IEventHandlerScp079Elevator : IEventHandler
    {
        void OnScp079Elevator(Scp079ElevatorInteractEvent ev);
    }

    public interface IEventHandlerScp079TeslaGate : IEventHandler
    {
        void OnScp079TeslaGate(Scp079TeslaGateInteractEvent ev);
    }

    public interface IEventHandlerScp079AddExp : IEventHandler
    {
        void OnScp079AddExp(Scp079AddExpEvent ev);
    }

    public interface IEventHandlerScp079LevelUp : IEventHandler
    {
        void OnScp079LevelUp(Scp079LevelUpEvent ev);
    }

    public interface IEventHandler079UnlockDoors : IEventHandler
    {
        void OnScp079UnlockDoors(Scp079DoorUnlockEvent ev);
    }

    public interface IEventHandlerScp079CameraTeleport : IEventHandler
    {
        void OnScp079CameraTeleport(Scp079CameraTeleportEvent ev);
    }

    public interface IEventHandlerScp079StartSpeaker : IEventHandler
    {
        void OnScp079StartSpeaker(Scp079StartSpeakerEvent ev);
    }

    public interface IEventHandlerScp079StopSpeaker : IEventHandler
    {
        void OnScp079StopSpeaker(Scp079StopSpeakerEvent ev);
    }

    public interface IEventHandlerScp079Lockdown : IEventHandler
    {
        void OnScp079Lockdown(Scp079LockdownEvent ev);
    }

    public interface IEventHandlerScp079ElevatorTeleport : IEventHandler
    {
        void OnScp079ElevatorTeleport(Scp079ElevatorTeleportEvent ev);
    }

    #endregion

    #region Scp 096

    public interface IEventHandlerScp096Panic : IEventHandler
    {
        void OnScp096Panic(Scp096PanicEvent ev);
    }

    public interface IEventHandlerScp096Enrage : IEventHandler
    {
        void OnScp096Enrage(Scp096EnrageEvent ev);
    }

    public interface IEventHandlerScp096CooldownStart : IEventHandler
    {
        void OnScp096CooldownStart(Scp096CooldownStartEvent ev);
    }

    public interface IEventHandlerScp096CooldownEnd : IEventHandler
    {
        void OnScp096CooldownEnd(Scp096CooldownEndEvent ev);
    }

    #endregion
}
