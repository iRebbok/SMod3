using System;

namespace SMod3.Core
{
    /// <summary>
    ///		The main attribute for identifying the plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PluginInfoAttribute : Attribute
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
        ///		Author of the plugin.
        /// </summary>
        public string[]? Authors { get; set; }
        /// <summary>
        ///		Co-authors of the plugin.
        /// </summary>
        public string[]? Collaborators { get; set; }
        /// <summary>
        ///		Priority for launching the plugin.
        ///		Works on <see cref="Plugin.OnDisable"/>,
        ///		<see cref="Plugin.OnEnable"/> and <see cref="Plugin.OnDestroy"/> events,
        ///		but not on <see cref="Plugin.Awake"/>.
        /// </summary>
        public byte LoadPriority { get; set; } = 0;
        /// <param name="id">
        ///		The main identifier of the plugin.
        ///		It is converted to lower case and is unique.
        /// </param>
        public PluginInfoAttribute(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id cannot be null, empty or whitespace", nameof(id));

            Id = id.ToLowerInvariant();
        }
    }
}
