using System;

namespace SMod3.Module.Config.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConfigOptionAttribute : Attribute
    {
        public string? Key { get; set; }
        /// <summary>
        ///     Doesn't assign a it's is empty or null.
        /// </summary>
        public bool DoNotAssign { get; set; }
    }
}
