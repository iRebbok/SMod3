using System;

using UnityEngine;

namespace SMod3.API
{
    /// <exception cref="InvalidOperationException">
    ///     Attempt to interact with a null door (game object of the door was destroyed).
    /// </exception>
    public abstract class Door : IGenericApiObject
    {
        /// <summary>
        ///     Returns true or false if the door is open or closed; sets door status.
        /// </summary>
        public abstract bool Open { get; set; }
        /// <summary>
        ///     Returns the status of a destroyed door or not; sets status.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     An attempt to return the status to not destroyed.
        /// </exception>
        public abstract bool Destroyed { get; set; }
        /// <summary>
        ///     Get or set status to open or not to open when the warhead is active.
        /// </summary>
        public abstract bool DontOpenOnWarhead { get; set; }
        /// <summary>
        ///     Get or set status to open or not open after a warhead explosion.
        /// </summary>
        public abstract bool BlockAfterWarheadDetonation { get; set; }
        /// <summary>
        ///     Get or set the locked status.
        /// </summary>
        public abstract bool Locked { get; set; }
        // todo: rethink/rewrite the cooldown API for the door
        /// <summary>
        ///     Current cooldown updates the position of the door???
        /// </summary>
        public abstract float LockCooldown { get; set; }
        /// <summary>
        ///     Door position.
        /// </summary>
        public abstract Vector3 Position { get; }
        /// <summary>
        ///     Door name.
        /// </summary>
        /// <remarks>
        ///     Only those doors that appear in the Remote Admin panel have a name.
        /// </remarks>
        public abstract string Name { get; }
        /// <summary><inheritdoc cref="DoorAccessRequirements"/></summary>
        public abstract DoorAccessRequirements Access { get; set; }
        /// <summary><inheritdoc cref="DoorType"/></summary>
        public abstract DoorType Type { get; }
        /// <summary><inheritdoc cref="DoorButtonType"/></summary>
        public abstract DoorButtonType Button { get; }
        /// <summary><inheritdoc cref="DoorStatus"/></summary>
        public abstract DoorStatus Status { get; }
        /// <summary>
        ///     Indicates whether the door requires all permissions.
        /// </summary>
        public abstract bool RequireAllPermissions { get; set; }

        /// <inheritdoc/>
        public abstract GameObject GetGameObject();
    }
}
