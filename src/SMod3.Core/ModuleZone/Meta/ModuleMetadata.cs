using System.Reflection;

using SMod3.Core.Fundamental;

namespace SMod3.Core
{
    public sealed class ModuleMetadata : BaseMetadata
    {
        public override string Id { get; }

        internal ModuleMetadata(Assembly assembly) : base(assembly)
        {
            Id = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        }
    }
}
