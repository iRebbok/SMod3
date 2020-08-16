using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class Int16Reader : ITypeReader
    {
        private readonly Type _nullableInt16Type = typeof(short?);
        private readonly Type _int16Type = typeof(short);

        public bool CanRead(Type type) => type.Equals(_nullableInt16Type) || type.Equals(_int16Type);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetInt16Value(key, null);
            wrapper.AssignValue(value);
        }
    }
}
