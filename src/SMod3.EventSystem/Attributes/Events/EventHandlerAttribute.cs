using System;

using SMod3.Core;

namespace SMod3.Module.EventSystem.Attributes
{
    /// <summary>
    ///     The attribute that the object that handles events uses.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EventHandlerAttribute : Attribute
    {
        /// <summary>
        ///     Preferences that are used when registering methods.
        /// </summary>
        public RegistrationPreferences? Preferences { get; }
        /// <summary>
        ///     Event execution priority.
        ///     By default it's <see cref="Priority.NORMAL"/>.
        /// </summary>
        public Priority? HandlePriority { get; }

        public EventHandlerAttribute(Priority? priority = null)
        {
            HandlePriority = priority;
        }

        public EventHandlerAttribute(RegistrationPreferences preferences, Priority? priority = null) : this(priority)
        {
            Preferences = preferences;
        }
    }
}
