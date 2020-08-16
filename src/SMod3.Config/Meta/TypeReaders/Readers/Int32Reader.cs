using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class Int32Reader : ITypeReader
    {
        private readonly Type _nullableInt32Type = typeof(int?);
        private readonly Type _int32Type = typeof(int);

        public bool CanRead(Type type) => type.Equals(_nullableInt32Type) || type.Equals(_int32Type);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetInt32Value(key, null);
            wrapper.AssignValue(value);
        }
    }
}
