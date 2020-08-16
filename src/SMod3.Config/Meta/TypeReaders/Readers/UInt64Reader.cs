using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class UInt64Reader : ITypeReader
    {
        private readonly Type _nullableUInt64Type = typeof(ulong?);
        private readonly Type _uint64Type = typeof(ulong);

        public bool CanRead(Type type) => type.Equals(_nullableUInt64Type) || type.Equals(_uint64Type);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetUInt64Value(key, null);
            wrapper.AssignValue(value);
        }
    }
}
