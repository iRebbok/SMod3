using System;

namespace SMod3.Module.EventSystem.Attributes
{
    /// <summary>
    ///     The attribute used by the plugin to set preferences for the event system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class EventSystemSettingAttribute : Attribute
    {
        /// <summary>
        ///     Automatic registration of event handlers.
        /// </summary>
        public bool AutoRegistration { get; set; }
    }
}
