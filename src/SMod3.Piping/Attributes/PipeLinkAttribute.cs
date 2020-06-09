using System;

using SMod3.Module.Attributes.Meta;

namespace SMod3.Module.Piping.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PipeLinkAttribute : BaseAttribute
    {
        public string Plugin { get; }
        public string Pipe { get; }

        public PipeLinkAttribute(string pluginId, string pipeName)
        {
            Plugin = !string.IsNullOrWhiteSpace(pluginId) ? pluginId : throw new ArgumentNullException(nameof(pluginId));
            Pipe = !string.IsNullOrWhiteSpace(pipeName) ? pipeName : throw new ArgumentNullException(nameof(pipeName));
        }
    }
}
