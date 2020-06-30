using System.Reflection;

using SMod3.Core.Fundamental;

namespace SMod3.Core
{
    /// <summary>
    ///     Endpoint metadata for the plugin.
    /// </summary>
    public sealed class PluginMetadata : BaseMetadata
    {
        /// <summary>
        ///     The main identifier of the plugin.
        /// </summary>
        public override string Id { get; }
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

        internal PluginMetadata(PluginMetadataAttribute attribute, Assembly assembly) : base(assembly)
        {
            Id = attribute.Id;
            Name = attribute.Name;
            Authors = attribute.Authors;
            Collaborators = attribute.Collaborators;
            Priority = attribute.Priority;
        }
    }
}
