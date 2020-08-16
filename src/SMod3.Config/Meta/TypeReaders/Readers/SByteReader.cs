using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class SByteReader : ITypeReader
    {
        private readonly Type _nullableSByteType = typeof(sbyte?);
        private readonly Type _sbyteType = typeof(sbyte);

        public bool CanRead(Type type) => type.Equals(_nullableSByteType) || _sbyteType.Equals(_sbyteType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetSByteValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
