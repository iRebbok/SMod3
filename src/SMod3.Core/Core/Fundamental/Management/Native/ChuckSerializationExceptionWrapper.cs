using System;
using System.Reflection;

namespace SMod3.Core.Fundamental.Native
{
    /// <summary>
    ///     Serialization exception wrapper.
    /// </summary>
    public readonly struct ChuckSerializationExceptionWrapper
    {
        public Exception Source { get; }

        public MemberInfo MemberInfo { get; }

        public ChuckSerializationExceptionWrapper(Exception source, MemberInfo memberInfo)
        {
            Source = source;
            MemberInfo = memberInfo;
        }
    }
}
