using System;

namespace SMod3.Core.Fundamental
{
    /// <summary>
    ///     Chunked data is used to get rid of direct dependencies between modules,
    ///     providing some of them as possible functionality,
    ///     a good example would be to use with a module like versioning.
    ///     This class cannot be extended.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ChunkDataAttribute : Attribute
    {
        public string Key { get; }

        public object? Value { get; }

        public ChunkDataAttribute(string key, object? value)
        {
            Key = !string.IsNullOrEmpty(key) ? key : throw new ArgumentException("The key cannot be empty or null");
            Value = value;
        }
    }
}
