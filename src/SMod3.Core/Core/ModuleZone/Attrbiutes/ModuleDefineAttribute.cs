using System;

namespace SMod3.Core
{
    /// <remarks>
    /// <para>
    ///     The attribute is the starting point of the module,
    ///     without which the module is not defined as a module and does not work properly.
    /// </para>
    /// <para>
    ///     Standalone modules have no entry point.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ModuleDefineAttribute : Attribute
    {
        /// <summary>
        ///     Module entry point.
        /// </summary>
        public Type? Entry { get; }

        /// <param name="entry"><inheritdoc cref="Entry"/></param>
        /// <exception cref="ArgumentNullException">
        ///     Entry is null.
        /// </exception>
        public ModuleDefineAttribute(Type? entry)
        {
            Entry = entry;
        }
    }
}
