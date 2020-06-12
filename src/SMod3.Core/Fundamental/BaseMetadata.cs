using System.Reflection;

namespace SMod3.Core.Fundamental
{
    public abstract class BaseMetadata
    {
        /// <summary>
        ///     Assembly version.
        /// </summary>
        /// <remarks>
        ///     <see cref="AssemblyVersionAttribute"/> is used to determine,
        ///     if null, then the version is set to '0.0.0'.
        /// </remarks>
        public string Version { get; }

        /// <summary>
        ///     The configuration under which the assembly was builded.
        ///     If the <see cref="AssemblyConfigurationAttribute"/> is null, then the result will be null.
        /// </summary>
        public string? Configuration { get; }

        /// <summary>
        ///     Unique metadata id.
        /// </summary>
        public abstract string Id { get; }

        protected BaseMetadata(Assembly assembly)
        {
            Configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
            Version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "0.0.0";
        }
    }
}
