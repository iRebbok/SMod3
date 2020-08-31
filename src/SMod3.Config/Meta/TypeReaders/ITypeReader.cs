using System;

namespace SMod3.Module.Config.Meta.TypeReaders
{
    public interface ITypeReader
    {
        bool CanRead(Type type);

        void Read(ConfigAttributeWrapper wrapper);
    }
}
