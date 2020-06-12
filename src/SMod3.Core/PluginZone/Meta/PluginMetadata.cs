using System.Reflection;

namespace SMod3.Core
{
    /// <summary>
    ///     Endpoint metadata for the plugin.
    /// </summary>
    public sealed class PluginMetadata
    {
        /// <summary>
        ///     The main identifier of the plugin.
        /// </summary>
        public string Id { get; }
        /// <summary>
        ///     Name of the plugin.
        /// </summary>
        public string? Name { get; }
        /// <summary>
        ///     Authors of the plugin.
        /// </summary>
        public string[]? Authors { get; }
        /// <summary>
        ///     Co-authors of the plugin.
        /// </summary>
        public string[]? Collaborators { get; }
        /// <summary>
        ///     Priority for the plugin.
        /// </summary>
        public byte Priority { get; }
        /// <summary>
        ///     Plugin assembly version.
        /// </summary>
        /// <remarks>
        ///     <see cref="AssemblyVersionAttribute"/> is used to determine,
        ///     if null, then the version is set to '0.0.0'.
        /// </remarks>
        public string Version { get; }

        internal PluginMetadata(PluginMetadataAttribute attribute, Assembly assembly)
        {
            Id = attribute.Id;
            Name = attribute.Name;
            Authors = attribute.Authors;
            Collaborators = attribute.Collaborators;
            Priority = attribute.Priority;

            Version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "0.0.0";
        }
    }
}
