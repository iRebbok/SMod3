using System;
using System.Collections.Generic;
using System.Reflection;

using SMod3.Core.Fundamental;

namespace SMod3.Core.PluginZone.Meta
{
    public sealed class PluginChangeStatusEvent : EventArgs
    {
        public Plugin Source { get; }

        public PluginStatus Status { get; }

        internal PluginChangeStatusEvent(Plugin source, PluginStatus status)
        {
            Source = source;
            Status = status;
        }
    }

    public sealed class PluginLoadingEvent : EventArgs
    {
        public Type Source { get; }
        public PluginMetadataAttribute DefineAttribute { get; }
        public IEnumerable<ChunkDataAttribute> ChunkAttributes { get; }
        public IList<IExtraData> ExtraDatas { get; }
        public bool Allow { get; set; }

        internal PluginLoadingEvent(Type source, PluginMetadataAttribute defineAttribute)
        {
            Source = source;
            DefineAttribute = defineAttribute;
            ChunkAttributes = source.GetCustomAttributes<ChunkDataAttribute>();
            ExtraDatas = new List<IExtraData>();
            Allow = true;
        }
    }

    public sealed class PluginEnablingEvent : EventArgs
    {
        public Plugin Source { get; }
        public bool Allow { get; set; }

        internal PluginEnablingEvent(Plugin source)
        {
            Source = source;
            Allow = true;
        }
    }

    public sealed class PluginEnabledEvent : EventArgs
    {
        public Plugin Source { get; }
        public bool Success { get; set; }

        internal PluginEnabledEvent(Plugin source, bool success)
        {
            Source = source;
            Success = success;
        }
    }

    public sealed class PluginDisablingEvent : EventArgs
    {
        public Plugin Source { get; }
        public bool Allow { get; set; }

        internal PluginDisablingEvent(Plugin source)
        {
            Source = source;
            Allow = true;
        }
    }

    public sealed class PluginDisabledEvent : EventArgs
    {
        public Plugin Source { get; }
        public bool Success { get; set; }

        internal PluginDisabledEvent(Plugin source, bool success)
        {
            Source = source;
            Success = success;
        }
    }
}
