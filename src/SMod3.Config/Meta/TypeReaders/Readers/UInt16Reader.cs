using System;

namespace SMod3.Module.Config.Meta.TypeReaders.Readers
{
    internal sealed class UInt16Reader : ITypeReader
    {
        private readonly Type _nullabledUInt16Type = typeof(ushort?);
        private readonly Type _uint16Type = typeof(ushort);

        public bool CanRead(Type type) => type.Equals(_nullabledUInt16Type) || type.Equals(_uint16Type);

        public void Read(ConfigAttributeWrapper wrapper)
        {
            this.AssertCorrentType(wrapper);
            var key = wrapper.GetConfigKey();
            var value = ConfigModule.Module.GameplayConfig.GetUInt16Value(key, null);
            wrapper.AssignValue(value);
        }
    }
}
