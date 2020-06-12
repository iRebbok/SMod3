namespace SMod3.Core
{
    public static class Entry
    {
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
