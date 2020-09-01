using System;

using SMod3.API;
using SMod3.Core.Logging;

namespace SMod3.Core
{
    public enum PluginStatus : byte
    {
        DISABLED = 0,
        ENABLED = 1,
        /// <summary>
        ///     Plugin has been disposed and awaiting GC collection.
        /// </summary>
        DISPOSED = 2
    }

    /// <summary>
    ///     An abstract class for inheritance for the implementation of plugins.
    /// </summary>
    public abstract class Plugin : BaseLogger, IComparable<Plugin>, IEquatable<Plugin>
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
#nullable disable
        public PluginMetadata Metadata { get; internal set; }
#nullable restore

        /// <summary>
        ///     Current plugin status.
        /// </summary>
        public PluginStatus Status { get; internal set; }

        public Server Server => PluginManager.Manager.Server;
        public Map Map => Server.Map;
        public Round Round => Server.Round;

        #endregion

        #region Plugin Events

        /// <summary>
        ///     Fires only once on creation.
        /// </summary>
        protected virtual void Awake() { }

        /// <summary>
        ///		Fires while enabling.
        /// </summary>
        protected virtual void OnEnable() { }

        /// <summary>
        ///		Fires while disabling.
        /// </summary>
        protected virtual void OnDisable() { }

        /// <summary>
        ///     Fires while disposing.
        ///     <para>
        ///         The plugin should dispose all managed and unmanaged resources here.
        ///     </para>
        /// </summary>
        protected virtual void OnDispose() { }

        #endregion

        #region Native

        public override string LoggingTag => Metadata.Id;

        internal void CallAwake() => Awake();
        internal void CallEnable() => OnEnable();
        internal void CallDisable() => OnDisable();
        internal void CallDispose() => OnDispose();

        #endregion

        #region Basic object overrides

        public override string ToString()
        {
            return $"{(!string.IsNullOrEmpty(Metadata.Name) ? Metadata.Name : "Anonymous")} ({Metadata.Id}) [{Metadata.Version}]";
        }

        public int CompareTo(Plugin other) => Metadata.Priority.CompareTo(other.Metadata.Priority);

        public bool Equals(Plugin other) =>
            Metadata.Priority == other.Metadata.Priority
            && Metadata.Id == other.Metadata.Id
            && Metadata.Name == other.Metadata.Name
            && Metadata.Configuration == other.Metadata.Configuration;
        // todo: version comparison

        public override bool Equals(object obj) => obj is Plugin plugin && Equals(plugin);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Metadata.Id.GetHashCode();
                hash = (hash * PluginManager.PRIME_OF_SUFFICIENT_SIZE) ^ Metadata.Priority.GetHashCode();

                if (!(Metadata.Name is null))
                    hash = (hash * PluginManager.PRIME_OF_SUFFICIENT_SIZE) ^ Metadata.Name.GetHashCode();
                if (!(Metadata.Authors is null))
                    hash = (hash * PluginManager.PRIME_OF_SUFFICIENT_SIZE) ^ Metadata.Authors.GetHashCode();
                if (!(Metadata.Collaborators is null))
                    hash = (hash * PluginManager.PRIME_OF_SUFFICIENT_SIZE) ^ Metadata.Collaborators.GetHashCode();

                return hash;
            }
        }

        public static bool operator ==(Plugin pl1, Plugin pl2) => pl1.Equals(pl2);

        public static bool operator !=(Plugin pl1, Plugin pl2) => !(pl1 == pl2);

        #endregion
    }
}
