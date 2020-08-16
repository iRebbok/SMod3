using System.Reflection;

using SMod3.Core;
using SMod3.Core.Fundamental;
using SMod3.Module.Config.Attributes;

namespace SMod3.Module.Config.Meta
{
    internal sealed class ConfigAttributeWrapper : BaseAttributeWrapper
    {
        public ConfigOptionAttribute Source { get; }

        internal ConfigAttributeWrapper(ConfigOptionAttribute source, Plugin partOwner, object? instance, MemberInfo target) : base(partOwner, instance, target)
        {
            Source = source;
        }

        internal ConfigAttributeWrapper(ConfigOptionAttribute source, Assembly owner, object? instance, MemberInfo target) : base(owner, instance, target)
        {
            Source = source;
        }
    }
}
