using System;

namespace SMod3.Core
{
    /// <summary>
    ///     Attribute defining the plugin in assembly itself.
    /// </summary>
    /// <remarks>
    ///     This attribute directly points to the plugin type instead of foreach all types.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class PluginDefineAttribute : Attribute
    {
        public Type[] Types { get; }

        /// <param name="types">
        ///     Types of plugin classes that are desirable for initialization.
        /// </param>
        public PluginDefineAttribute(Type[] types)
        {
            Types = types;
        }
    }
}
