using System;
using System.Reflection;

namespace SMod3.Module.Piping.Meta
{
    public class MethodPipeParameter
    {
        public Type Type { get; }

        public bool HasDefault { get; }
        public object Default { get; }

        internal MethodPipeParameter(ParameterInfo info)
        {
            Type = info.ParameterType;

            HasDefault = info.HasDefaultValue;
            Default = info.DefaultValue;
        }
    }
}
