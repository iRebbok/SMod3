using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class UInt32Reader : ITypeReader
    {
        private readonly Type _nullableUInt32Type = typeof(int?);
        private readonly Type _uint32Type = typeof(int);

        public bool CanRead(Type type) => type.Equals(_uint32Type) || type.Equals(_uint32Type);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetUInt32Value(key, null);
            wrapper.AssignValue(value);
        }
    }
}
