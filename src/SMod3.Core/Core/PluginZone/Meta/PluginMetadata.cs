using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using SMod3.Core.Fundamental;
using SMod3.Core.RuntimeSettings;

namespace SMod3.Core
{
    /// <summary>
    ///     Endpoint metadata for the plugin.
    /// </summary>
    public sealed class PluginMetadata : BaseMetadata
    {
        /// <remarks>
        ///     Allows dynamic loading of data during configuration during awakening.
        /// </remarks>
        internal readonly IList<IExtraData> _extraDatas;

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
        public Priority Priority { get; }
        /// <summary>
        ///     Extra data provided by plugins for modules.
        /// </summary>
        public ReadOnlyCollection<IExtraData> ExtraDatas { get; }
        /// <summary>
        ///     Runtime settings collection.
        /// </summary>
        public RuntimeSettingCollection RuntimeSettings { get; }

        internal PluginMetadata(PluginMetadataAttribute attribute, Assembly assembly, IList<IExtraData> extraDatas) : base(assembly)
        {
            _extraDatas = extraDatas;
            Id = attribute.Id;
            Name = attribute.Name;
            Authors = attribute.Authors;
            Collaborators = attribute.Collaborators;
            Priority = attribute.Priority;
            ExtraDatas = new ReadOnlyCollection<IExtraData>(extraDatas);
            RuntimeSettings = new RuntimeSettingCollection();
        }
    }
}
