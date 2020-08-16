using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class DecimalReader : ITypeReader
    {
        private readonly Type _nullableDecimalType = typeof(decimal?);
        private readonly Type _decimalType = typeof(decimal);

        public bool CanRead(Type type) => type.Equals(_nullableDecimalType) || type.Equals(_decimalType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetDecimalValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
