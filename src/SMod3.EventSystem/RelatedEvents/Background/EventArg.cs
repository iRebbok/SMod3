using System;

namespace SMod3.Module.EventSystem.Background
{
    /// <summary>
    ///     Abstraction for event arguments.
    /// </summary>
    public abstract class EventArg : EventArgs
    {
        /// <summary>
        ///     Resets for later use.
        /// </summary>
        internal abstract void Reset();

        /// <summary>
        ///     Copy all values to other.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Wrong other type.
        /// </exception>
        internal abstract void CopyTo(EventArg other);
    }
}
