using System;

using SMod3.Core;
using SMod3.Module.EventSystem.Meta;

namespace SMod3.Module.EventSystem.Attributes
{
    /// <summary>
    ///     The attribute the method uses to identify itself as an event handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HandlerMethodAttribute : Attribute
    {
        /// <summary>
        ///     The type of handler that the method operates on.
        /// </summary>
        public Type Handler { get; }
        /// <summary>
        ///     Method registration preferences.
        ///     Override the preferences that the object uses.
        /// </summary>
        public RegistrationPreferences? Preferences { get; }
        /// <summary>
        ///     Event execution priority.
        ///     By default it's <see cref="Priority.NORMAL"/>.
        ///     Override the preferences that the object uses.
        /// </summary>
        public Priority? HandlePriority { get; }

        public HandlerMethodAttribute(Type handler, Priority? priority = null)
        {
            TypeExtension.CheckTypeIsEventHandler(handler);
            Handler = handler;
            HandlePriority = priority;
        }

        public HandlerMethodAttribute(Type handler, RegistrationPreferences preferences, Priority? priority = null) : this(handler, priority)
        {
            Preferences = preferences;
        }
    }
}
