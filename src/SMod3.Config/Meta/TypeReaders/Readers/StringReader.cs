using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class StringReader : ITypeReader
    {
        private readonly Type _stringType = typeof(string);

        public bool CanRead(Type type) => type.Equals(_stringType);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetRawValue(key, null);
            wrapper.AssignValue(value);
        }
    }
}
