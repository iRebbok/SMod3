using SMod3.Module.Config.Attributes;
using SMod3.Module.Lang.Attributes;
using System;

namespace SMod3.Core
{
    /// <summary>
    ///		The main attribute for identifying the plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PluginDefineAttribute : Attribute
    {
        /// <summary>
        ///		The main identifier of the plugin.
        ///		The plugin will not be initialized without it.
        /// </summary>
        public string Id { get; }
        /// <summary>
        ///		Name of the plugin.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///		Author of the plugin.
        /// </summary>
        public string[] Authors { get; set; }
        /// <summary>
        ///		Co-authors of the plugin.
        /// </summary>
        public string[] Collaborators { get; set; }
        /// <summary>
        ///		Plugin description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///		The prefix for config.
        ///		<para>
        ///			Required value when using the attribute <see cref="ConfigOptionAttribute"/>.
        ///		</para>
        /// </summary>
        public string ConfigPrefix { get; set; }
        /// <summary>
        ///		Name of the lang file.
        ///		<para>
        ///			Required value when using the attribute <see cref="LangOptionAttribute"/>.
        ///		</para>
        /// </summary>
        public string LangFile { get; set; }
        /// <summary>
        ///		Plugin version.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///		Priority for launching the plugin.
        ///		Works on <see cref="Plugin.OnDisable"/>,
        ///		<see cref="Plugin.OnEnable"/> and <see cref="Plugin.OnDestroy"/> events,
        ///		but not on <see cref="Plugin.Awake"/>.
        /// </summary>
        public byte LoadPriority { get; set; } = 0;
        /// <summary>
        ///		Fully compatible Major version of SMod.
        /// </summary>
        public int SmodMajor { get; set; }
        /// <summary>
        ///		Fully compatible Minor version of SMod.
        /// </summary>
        public int SmodMinor { get; set; }
        /// <summary>
        ///		Fully compatible Revision version of SMod.
        /// </summary>
        public int SmodRevision { get; set; }
        /// <param name="id">
        ///		The main identifier of the plugin.
        ///		It is converted to lower case and is unique.
        /// </param>
        public PluginDefineAttribute(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(id);
            }

            Id = id.ToLowerInvariant();
        }
    }
}
