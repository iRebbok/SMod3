using System;
using System.Reflection;

using SMod3.API;
using SMod3.Core.Imbedded.Version;

namespace SMod3.Core
{
    public static class Entry
    {
        public static readonly SemanticVersion VERSION = SemanticMatcher.Default.Parse(typeof(Entry).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);

        /// <summary>
        ///     Entry point for all SMod3.
        /// </summary>
        /// <param name="binPath">
        ///     The path to the bin folder where all the main libraries are located.
        /// </param>
        /// <param name="gamePath">
        ///     The path to the game folder.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     One of the paths is null or empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Server is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Repeated call.
        /// </exception>
        public static void Call(string binPath, string gamePath, Server server)
        {
            if (string.IsNullOrEmpty(binPath) || string.IsNullOrEmpty(gamePath))
                throw new ArgumentException("Paths cannot be empty or null");
            else if (server is null)
                throw new ArgumentNullException("Server cannot be null", nameof(server));
            else if (!(PluginManager.Manager is null))
                throw new InvalidOperationException("Attempt to call again");

            ModuleManager.Manager.LoadModules();
            new PluginManager(binPath, gamePath, server).Load();
        }
    }
}
