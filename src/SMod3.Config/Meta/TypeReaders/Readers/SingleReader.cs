using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class SingleReader : ITypeReader
    {
        private readonly Type _nullableSignleType = typeof(float?);
        private readonly Type _singleType = typeof(float);

        public bool CanRead(Type type) => type.Equals(_nullableSignleType) || type.Equals(_singleType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetSingleValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
