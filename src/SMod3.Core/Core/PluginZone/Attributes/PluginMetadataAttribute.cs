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
        public string Id { get; set; }
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
        public Priority Priority { get; set; }

        public PluginMetadataAttribute()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ArgumentException("Id cannot be null, empty or whitespace", nameof(Id));

            Id = Id.ToLowerInvariant();
        }
    }
}
