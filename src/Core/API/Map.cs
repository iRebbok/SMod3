using System.Collections.Generic;

namespace SMod3.Core.API
{
	public abstract class Map
	{
		public abstract List<Item> GetItems(ItemType type, bool world_only);
		public abstract Vector GetRandomSpawnPoint(RoleType role);
		public abstract List<Vector> GetSpawnPoints(RoleType role);
		public abstract List<Vector> GetBlastDoorPoints();
		public abstract List<Door> GetDoors();
		public abstract List<PocketDimensionExit> GetPocketDimensionExits();
		public abstract Dictionary<Vector, Vector> GetElevatorTeleportPoints();
		public abstract Generator[] GetGenerators();
		public abstract Room[] Get079InteractionRooms(Scp079InteractionType type);
		public abstract void DetonateWarhead();
		public abstract void StartWarhead();
		public abstract void StopWarhead();
		public abstract void Shake();
		public abstract bool WarheadDetonated { get; }
		public abstract bool LCZDecontaminated { get; }
		public abstract void SpawnItem(ItemType type, Vector position, Vector rotation);
		public abstract void SpawnItem(WeaponType type, float Ammo, WeaponSight Sight, WeaponBarrel Barrel, WeaponOther Other, Vector pos, Vector rotation);
		public abstract void SpawnItem(AmmoType type, float Ammo, Vector position, Vector rotation);
		/// <summary>  
		/// Note: When FemurBreaker is enabled, SCP-106 can't be contained. This might break the lure configs and mechanism.
		/// </summary> 
		public abstract void FemurBreaker(bool enable);
		public abstract List<Elevator> GetElevators();
		public abstract string CurrentIntercomContent { get; set; }
		public abstract void SetIntercomContent(IntercomStatus intercomStatus, string content);
		public abstract string GetIntercomContent(IntercomStatus intercomStatus);
		public abstract List<TeslaGate> GetTeslaGates();
		public abstract void AnnounceNtfEntrance(int scpsLeft, int mtfNumber, char mtfLetter);
		public abstract void AnnounceScpKill(string scpNumber, Player killer = null);
		public abstract void AnnounceCustomMessage(string words);
		public abstract void SetIntercomSpeaker(Player player);
		public abstract Player GetIntercomSpeaker();
		public abstract void Broadcast(uint duration, string message, bool isMonoSpaced);
		public abstract void ClearBroadcasts();
		public abstract bool WarheadLeverEnabled { get; set; }
		public abstract bool WarheadKeycardEntered { get; set; }
	}

	public abstract class Door
	{
		public abstract bool Open { get; set; }
		public abstract bool Destroyed { get; set; }
		public abstract bool DontOpenOnWarhead { get; set; }
		public abstract bool BlockAfterWarheadDetonation { get; set; }
		public abstract bool Locked { get; set; }
		public abstract float LockCooldown { get; set; }
		public abstract Vector Position { get; }
		public abstract string Name { get; }
		public abstract string Permission { get; }
		public abstract object GetComponent();
	}

	public abstract class TeslaGate
	{
		public abstract void Activate(bool instant = false);
		public abstract float TriggerDistance { get; set; }
		public abstract Vector Position { get; }
		public abstract object GetComponent();
	}

	public abstract class Elevator
	{
		public abstract ElevatorType ElevatorType { get; }
		public abstract ElevatorStatus ElevatorStatus { get; }
		public abstract void Use();
		public abstract bool Locked { get; set; }
		public abstract bool Lockable { get; set; }
		public abstract float MovingSpeed { get; set; }
		public abstract List<Vector> GetPositions();
		public abstract object GetComponent();
	}

	public abstract class PocketDimensionExit
	{
		public abstract PocketDimensionExitType ExitType { get; set; }
		public abstract Vector Position { get; }
	}

	public abstract class Room
	{
		public abstract ZoneType ZoneType { get; }
		public abstract RoomType RoomType { get; }
		public abstract int GenericID { get; }
		public abstract Vector Position { get; }
		public abstract Vector Forward { get; }
		public abstract Vector SpeakerPosition { get; }

		public abstract void FlickerLights(float duration = 8f);
		public abstract string[] GetObjectName();
		public abstract object GetGameObject();
	}

	public abstract class Generator
	{
		public abstract bool Open { get; set; }
		public abstract bool Locked { get; }
		public abstract bool HasTablet { get; set; }
		public abstract bool Engaged { get; }
		public abstract float StartTime { get; }
		public abstract float TimeLeft { get; set; }
		public abstract Vector Position { get; }
		public abstract Room Room { get; }

		public abstract void Unlock();
		public abstract object GetComponent();
	}
}
