using System.Reflection;

using SMod3.Core.Imbedded.Version;

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
        public SemanticVersion Version { get; }

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
            var verAttrb = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (verAttrb is null)
                Version = new SemanticVersion(0, 0, 0);
            else
                Version = SemanticMatcher.Default.Parse(verAttrb.InformationalVersion);
        }
    }
}
