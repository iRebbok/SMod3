using System;

namespace SMod3.Core.Fundamental.Native
{
    /// <summary>
    ///     Attribute used by the module to serialize chunks into an object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ChuckSerializableAttribute : Attribute
    {
        public string? Key { get; }

        // Native serialization by field/property name
        public ChuckSerializableAttribute()
        {
            Key = null;
        }

        public ChuckSerializableAttribute(string key)
        {
            Key = !string.IsNullOrEmpty(key) ? key : throw new ArgumentException("Cannot be empty or null", nameof(key));
        }
    }
}
