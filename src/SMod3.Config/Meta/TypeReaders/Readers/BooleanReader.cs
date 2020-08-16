using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class BooleanReader : ITypeReader
    {
        private readonly Type _nullableBooleanType = typeof(bool?);
        private readonly Type _booleanType = typeof(bool);

        public bool CanRead(Type type) => type.Equals(_nullableBooleanType) || type.Equals(_booleanType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetBoolValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
