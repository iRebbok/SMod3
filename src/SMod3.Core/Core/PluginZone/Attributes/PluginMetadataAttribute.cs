using System;

namespace SMod3.Core
{
    /// <summary>
    ///		Plugin information attribute that is used on the plugin class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PluginMetadataAttribute : Attribute
    {
        /// <summary>
        ///		The main identifier of the plugin.
        ///		The plugin will not be initialized without it.
        /// </summary>
        public string Id { get; }
        /// <summary>
        ///		Name of the plugin.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        ///		Authors of the plugin.
        /// </summary>
        public string[]? Authors { get; set; }
        /// <summary>
        ///		Co-authors of the plugin.
        /// </summary>
        public string[]? Collaborators { get; set; }
        /// <summary>
        ///     Priority on plugin initialization.
        /// </summary>
        public byte Priority { get; set; } = 0;

        /// <param name="id">
        ///		The main identifier of the plugin.
        ///		It is converted to lower case and is unique.
        /// </param>
        public PluginMetadataAttribute(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id cannot be null, empty or whitespace", nameof(id));

            Id = id.ToLowerInvariant();
        }
    }
}
