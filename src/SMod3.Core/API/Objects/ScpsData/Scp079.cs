
using System.Collections.Generic;

using UnityEngine;

namespace SMod3.API
{
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
        /// <summary>
        ///     The current camera the player is on.
        /// </summary>
        public abstract Scp079Camera Camera { get; }

        public abstract IEnumerable<Door> GetLockedDoors();
        public abstract void Lock(Door door);
        public abstract void Unlock(Door door);
        public abstract void TriggerTesla(TeslaGate tesla);
        public abstract void Lockdown(Room room);
        public abstract void SetCamera(Vector3 position, bool lookAt = false);
        public abstract void ShowGainExp(ExperienceType expType);
        public abstract void ShowLevelUp(int level);
    }

    public abstract class Scp079Camera : IGenericApiObject
    {
        public abstract string Name { get; }
        public abstract ushort Id { get; }
        public abstract bool IsMain { get; }
        /// <summary>
        ///     The room where the camera is.
        /// </summary>
        public abstract Room Room { get; }

        public abstract GameObject GetGameObject();
    }
}
