using System;
using System.Collections.Generic;
using System.Reflection;

using SMod3.Core.Fundamental;

namespace SMod3.Core.Meta
{
    public abstract class CustomEventArgs : EventArgs, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public sealed class PluginChangedStatusEvent : CustomEventArgs
    {
        public Plugin Source { get; }
        public PluginStatus Status { get; }
        public PluginStatus PrevStatus { get; }

        internal PluginChangedStatusEvent(Plugin source, PluginStatus status, PluginStatus prevStatus)
        {
            Source = source;
            Status = status;
            PrevStatus = prevStatus;
        }
    }

    public sealed class PluginLoadingEvent : CustomEventArgs
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

    public sealed class PluginEnablingEvent : CustomEventArgs
    {
        public Plugin Source { get; }
        public bool Allow { get; set; }

        internal PluginEnablingEvent(Plugin source)
        {
            Source = source;
            Allow = true;
        }
    }

    public sealed class PluginEnabledEvent : CustomEventArgs
    {
        public Plugin Source { get; }
        public bool Success { get; }

        internal PluginEnabledEvent(Plugin source, bool success)
        {
            Source = source;
            Success = success;
        }
    }

    public sealed class PluginDisablingEvent : CustomEventArgs
    {
        public Plugin Source { get; }
        public bool Allow { get; set; }

        internal PluginDisablingEvent(Plugin source)
        {
            Source = source;
            Allow = true;
        }
    }

    public sealed class PluginDisabledEvent : CustomEventArgs
    {
        public Plugin Source { get; }
        public bool Success { get; }

        internal PluginDisabledEvent(Plugin source, bool success)
        {
            Source = source;
            Success = success;
        }
    }
}
