using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class ByteReader : ITypeReader
    {
        private readonly Type _nullableByteType = typeof(byte?);
        private readonly Type _byteType = typeof(byte);

        public bool CanRead(Type type) => type.Equals(_nullableByteType) || type.Equals(_byteType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetByteValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
