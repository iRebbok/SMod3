using System;

namespace SMod3.Module.EventSystem.Meta.Wrappers
{
    /// <summary>
    ///     Wrapper for instances.
    /// </summary>
    internal sealed class InstanceWrapper
    {
        public Type InstanceType { get; }
        public object Instance { get; }

        public InstanceWrapper(object instance)
        {
            Instance = instance;
            InstanceType = instance.GetType();
        }
    }
}
