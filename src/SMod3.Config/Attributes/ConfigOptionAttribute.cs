using System;

namespace SMod3.Module.Config.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ConfigOptionAttribute : Attribute
    {
        public string Key { get; }
        public string Description { get; }
        public bool Randomized { get; }

        public ConfigOptionAttribute(bool isRandomized = false)
        {
            Randomized = isRandomized;
        }

        public ConfigOptionAttribute(string key, bool isRandomized = false) : this(isRandomized)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Config key cannot be null, whitespace, or empty.", nameof(key));
            }

            Key = key;
        }

        public ConfigOptionAttribute(string key, string description, bool isRandomized = false) : this(key, isRandomized)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Config description cannot be null, whitespace, or empty.", nameof(description));
            }

            Description = description;
        }
    }
}
