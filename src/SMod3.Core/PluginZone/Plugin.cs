using SMod3.Core.Logging;

namespace SMod3.Core
{
    /// <summary>
    ///     An abstract class for inheritance for the implementation of plugins.
    /// </summary>
    public abstract class Plugin : BaseLogger
    {
        #region Properties

        /// <summary>
        ///     Immutable plugin metadata.
        /// </summary>
        /// <remarks>
        ///     It will never be null,
        ///     it's determined and installed when the plugin is initialized
        ///     before calling Awake, but after calling the constructor.
        /// </remarks>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public PluginMetadata Metadata { get; internal set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <summary>
        ///     Current plugin status.
        /// </summary>
        public PluginStatus Status { get; internal set; }

        #endregion

        #region Plugin Events

        /// <summary>
        ///		Called first when loading the plugin.
        ///		Called once for the entire existence of the plugin object.
        ///		<para>
        ///         Here, create the objects that you will need throughout the entire life cycle.
        ///     </para>
        /// </summary>
        protected virtual void Awake() { }

        /// <summary>
        ///		Called when starting the plugin.
        ///		<para>
        ///         Here you have to register all your teams, events, etc.
        ///     </para>
        /// </summary>
        protected virtual void OnEnable() { }

        /// <summary>
        ///		Called when disabling the plugin.
        ///		<para>
        ///			Here you must cancel all registrations in other libraries
        ///			that have a callback to yours,
        ///			everything else is automatically untied.
        ///		</para>
        /// </summary>
        protected virtual void OnDisable() { }

        /// <summary>
        ///		Called when destroying the plugin.
        ///		But before that, the plugin turns off.
        ///		<para>
        ///			Here you must complete the plugin life cycle,
        ///			you must remove all callbacks from other libraries.
        ///		</para>
        /// </summary>
        protected virtual void OnDestroy() { }

        #endregion

        #region Native

        public override string LoggingTag => Metadata!.Id;

        internal void CallAwake() => Awake();
        internal void CallEnable() => OnEnable();
        internal void CallDisable() => OnDisable();
        internal void CallDestroy() => OnDestroy();

        #endregion

        #region Basic object overrides

        public override string ToString()
        {
            return $"{(!string.IsNullOrEmpty(Metadata.Name) ? Metadata.Name : "Anonymous")} ({Metadata.Id}) [{Metadata.Version}]";
        }

        #endregion
    }
}