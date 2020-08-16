using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class Int64Reader : ITypeReader
    {
        private readonly Type _nullableInt64Type = typeof(long?);
        private readonly Type _int64Type = typeof(long);

        public bool CanRead(Type type) => type.Equals(_nullableInt64Type) || type.Equals(_int64Type);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetInt64Value(key, null);
            wrapper.AssignValue(value);
        }
    }
}
