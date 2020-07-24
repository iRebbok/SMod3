using System.Reflection;

using SMod3.Core;

namespace SMod3.Module.EventSystem.Meta.Wrappers
{
    internal readonly ref struct TempIdWrapper
    {
        public Plugin? PartOwner { get; }
        public Assembly Owner { get; }

        public TempIdWrapper(Plugin? partOwner, Assembly? owner)
        {
            PartOwner = partOwner;
            Owner = owner ?? partOwner!.GetType().Assembly;
        }
    }
}
