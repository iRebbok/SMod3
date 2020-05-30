namespace SMod3.Core.API
{
    /// <summary>
    ///		The API that mirroring <see cref="UnityEngine.Time"/>.
    /// </summary>
    public abstract class Time
    {
        /// <summary>
        ///		The time at the beginning of this frame.
        ///		This is the time in seconds since the start of the game.
        ///		As <see cref="UnityEngine.Time.time"/>.
        /// </summary>
        public abstract float CurrentTime { get; }
        /// <summary>
        ///		The time the latest FixedUpdate has started.
        ///		This is the time in seconds since the start of the game.
        ///		As <see cref="UnityEngine.Time.fixedTime"/>.
        /// </summary>
        public abstract float CurrentFixedTime { get; }
        /// <summary>
        ///		The completion time in seconds since the last frame.
        ///		As <see cref="UnityEngine.Time.deltaTime"/>.
        /// </summary>
        public abstract float DeltaTime { get; }
        /// <summary>
        ///		The interval in seconds at which physics and
        ///		other fixed frame rate updates (like MonoBehaviour's FixedUpdate)
        ///		are performed.
        ///		As <see cref="UnityEngine.Time.fixedDeltaTime"/>.
        /// </summary>
        public abstract float DeltaFixedTime { get; }
        /// <summary>
        ///		The total number of frames that have passed.
        ///		As <see cref="UnityEngine.Time.frameCount"/>.
        /// </summary>
        public abstract int FrameCount { get; }
    }
}
