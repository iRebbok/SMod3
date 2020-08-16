using System;

namespace SMod3.Module.Config.Meta.TypeReaders
{
    internal interface ITypeReader
    {
        bool CanRead(Type type);

        void Read(ConfigAttributeWrapper wrapper);
    }
}
