namespace SMod3.Core.RuntimeSettings.Native
{
    /// <summary>
    ///     Marks the plugin disabled at runtime.
    ///     The plugin will not be enabled and <see cref="Plugin.OnEnable"/> will not be called.
    /// </summary>
    public readonly struct DisabledRuntimeSetting : IRuntimeSetting { }
}
