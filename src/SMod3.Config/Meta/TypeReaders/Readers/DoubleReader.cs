using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class DoubleReader : ITypeReader
    {
        private readonly Type _nullableDoubleType = typeof(double?);
        private readonly Type _doubleType = typeof(double);

        public bool CanRead(Type type) => type.Equals(_nullableDoubleType) || type.Equals(_doubleType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetDoubleValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
