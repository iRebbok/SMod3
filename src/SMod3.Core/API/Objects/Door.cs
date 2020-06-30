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
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract bool Open { get; set; }
        /// <summary>
        ///     Returns the status of a destroyed door or not; sets status.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     <inheritdoc cref="Door"/>
        ///     An attempt to return the status to not destroyed.
        /// </exception>
        public abstract bool Destroyed { get; set; }
        /// <summary>
        ///     Get or set status to open or not to open when the warhead is active.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract bool DontOpenOnWarhead { get; set; }
        /// <summary>
        ///     Get or set status to open or not open after a warhead explosion.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract bool BlockAfterWarheadDetonation { get; set; }
        /// <summary>
        ///     Get or set the locked status.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract bool Locked { get; set; }
        // todo: rethink/rewrite the cooldown API for the door
        /// <summary>
        ///     Current cooldown updates the position of the door???
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract float LockCooldown { get; set; }
        /// <summary>
        ///     Door position.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract Vector3 Position { get; }
        /// <summary>
        ///     Door name.
        /// </summary>
        /// <remarks>
        ///     Only those doors that appear in the Remote Admin panel have a name.
        /// </remarks>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract string Name { get; }
        /// <summary><inheritdoc cref="AccessRequirements"/></summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract AccessRequirements Access { get; set; }
        /// <summary><inheritdoc cref="DoorType"/></summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract DoorType Type { get; }
        /// <summary><inheritdoc cref="ButtonType"/></summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract ButtonType Button { get; }
        /// <summary><inheritdoc cref="DoorStatus"/></summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract DoorStatus Status { get; }
        /// <summary>
        ///     Indicates whether the door requires all permissions.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract bool RequireAllPermissions { get; set; }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Door"/></exception>
        public abstract GameObject GetGameObject();
    }
}
