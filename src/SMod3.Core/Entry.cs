namespace SMod3.Core
{
    public static class Entry
    {
        /// <summary>
        ///     Versioning constant for SMod3.Core.
        /// </summary>
        public const string VERSION = "1.0.0";

        public static readonly int SMOD_VERSION_MAJOR = int.Parse(VERSION.Split('.')[0]);
        public static readonly int SMOD_VERSION_MINOR = int.Parse(VERSION.Split('.')[1]);
        public static readonly int SMOD_VERSION_REVISION = int.Parse(VERSION.Split('.')[2]);
        public static readonly int SMOD_VERSION_BUILD = int.Parse(VERSION.Split('.')[3]);
        public static readonly string SMOD_VERSION = $"{SMOD_VERSION_MAJOR}.{SMOD_VERSION_MINOR}.{SMOD_VERSION_REVISION}";

        /// <summary>
        ///     Entry point for all SMod3.
        /// </summary>
        public static void Call()
        {
            ModuleManager.Manager.LoadModules();
            PluginManager.Manager.Load();
        }
    }
}
