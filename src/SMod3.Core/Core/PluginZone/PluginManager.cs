using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

using SMod3.API;
using SMod3.Core.Fundamental;
using SMod3.Core.Meta;
using SMod3.Core.Misc;
using SMod3.Core.RuntimeSettings.Native;

namespace SMod3.Core
{
    public sealed partial class PluginManager : BaseManager
    {
        #region Static zone

        public const int PRIME_OF_SUFFICIENT_SIZE = 397;
        public const string DLL_SEARCH_PATTERN = "*" + GacBypass.ASSEMBLY_FILE_EXTENSION;

        public static readonly string DEPENDENCY_FOLDER_NAME = "dependencies";
        public static readonly string PLUGIN_FOLDER_NAME = "sm_plugins";
        public static readonly string GLOBAL_FOLDER_NAME = "common";

#nullable disable
        public static PluginManager Manager { get; private set; }
#nullable restore

        #endregion

        #region Variables & Properties

        public override string LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(PluginManager));

        private readonly SortedList<Plugin, Assembly> _plugins = new SortedList<Plugin, Assembly>(new DuplicateKeyOrderByDescendingComparator<Plugin>());
        private readonly HashSet<Assembly> _dependencies = new HashSet<Assembly>();

        /// <summary>
        ///     The path to the bin folder
        ///     where important SMod3 components are located,
        ///     including SMod3.Core.
        /// </summary>
        /// <remarks>
        ///     It's assumed that you will never need
        ///     to use this property for anything else than getting the path,
        ///     and therefore this information isn't updated,
        ///     if you need to get updated information about files and stuff,
        ///     use <see cref="FileSystemInfo.Refresh"/> before.
        /// </remarks>
        public DirectoryInfo BinPath { get; }

        /// <summary>
        ///		The path to the server folder.
        /// </summary>
        /// <remarks><inheritdoc cref="BinPath"/></remarks>
        public DirectoryInfo GamePath { get; }

        private readonly DirectoryInfo _pluginsPath;
        /// <summary>
        ///     Gets the main plugins path.
        /// </summary>
        public DirectoryInfo PluginsPath { get => _pluginsPath.EnsureExists(); }

        private readonly DirectoryInfo _globalPluginsPath;
        /// <summary>
        ///		Global path to the global folder with plugins.
        /// </summary>
        public DirectoryInfo GlobalPluginsPath { get => _globalPluginsPath.EnsureExists(true); }

        private readonly DirectoryInfo _serverPluginsPath;
        /// <summary>
        ///     Gets the dedicated folder for the server with plugins.
        /// </summary>
        public DirectoryInfo ServerPluginsPath { get => _serverPluginsPath.EnsureExists(); }

        private readonly DirectoryInfo _globalDependencyPath;
        /// <summary>
        ///     Gets the global dependency path.
        /// </summary>
        public DirectoryInfo GlobalDependencyPath { get => _globalDependencyPath.EnsureExists(true); }

        private readonly DirectoryInfo _serverDependencyPath;
        /// <summary>
        ///     Gets the dedicated path for the server to the dependencies.
        /// </summary>
        public DirectoryInfo ServerDependencyPath { get => _serverDependencyPath.EnsureExists(); }

        public GacBypass GACBypasser { get; }

        #endregion

        #region API properties

        public Server Server { get; }

        #endregion

        #region Plugin related properties

        public ReadOnlyDictionary<Plugin, Assembly> PluginsAndAssemblies { get; }

        /// <summary>
        ///     Returns plugins that have been loaded.
        /// </summary>
        /// <remarks>
        ///     Loaded plugins mean that they have a status <see cref="PluginStatus.LOADED"/>,
        ///     which is assigned only when the plugin is loaded but not enabled.
        /// </remarks>
        public IEnumerable<Plugin> LoadedPlugins => from p in _plugins.Keys where p.Status == PluginStatus.LOADED select p;

        /// <summary>
        ///     Returns plugins that have been enabled.
        /// </summary>
        public IEnumerable<Plugin> EnabledPlugins => from p in _plugins.Keys where p.Status == PluginStatus.ENABLED select p;

        /// <summary>
        ///     Returns plugins that have been disabled.
        /// </summary>
        public IEnumerable<Plugin> DisabledPlugins => from p in _plugins.Keys where p.Status == PluginStatus.DISABLED select p;

        /// <summary>
        ///     Returns all plugins.
        /// </summary>
        public IEnumerable<Plugin> Plugins => _plugins.Keys;

        #endregion

        #region Plugin related events

        /// <summary>
        ///     Called when the plugin status changes.
        /// </summary>
        /// <remarks>
        ///     Called before the status is applied,
        ///     therefore, the previous status can be obtained.
        /// </remarks>
        public event CustomDelegate<PluginChangedStatusEvent>? PluginChangedStatus;

        /// <summary>
        ///     Called when loading the plugin, allowing to prevent loading.
        /// </summary>
        public event CustomDelegate<PluginLoadingEvent>? PluginLoading;

        /// <summary>
        ///     Called when the plugin starts,
        ///     allowing it to be allowed or denied to continue.
        /// </summary>
        public event CustomDelegate<PluginEnablingEvent>? PluginEnabling;

        /// <summary>
        ///     Called when the plugin is enabled.
        /// </summary>
        public event CustomDelegate<PluginEnabledEvent>? PluginEnabled;

        /// <summary>
        ///     Called when the plugin is disabled,
        ///     allowing it to be allowed or denied to continue.
        /// </summary>
        public event CustomDelegate<PluginDisablingEvent>? PluginDisabling;

        /// <summary>
        ///     Called when the plugin is disabled.
        /// </summary>
        public event CustomDelegate<PluginDisabledEvent>? PluginDisabled;

        /// <summary>
        ///     Called when the plugin is disposed.
        /// </summary>
        public event CustomDelegate<Plugin>? PluginDisposed;

        #endregion

        #region Construstors

        /// <exception cref="InvalidOperationException">
        ///     Recall when <see cref="Manager"/> is already assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Empty/Null arguments.
        /// </exception>
        private PluginManager(string binPath, string gamePath, Server server)
        {
            Server = server;
            GACBypasser = new GacBypass();

            BinPath = new DirectoryInfo(binPath);
            GamePath = new DirectoryInfo(gamePath);

            PluginsAndAssemblies = new ReadOnlyDictionary<Plugin, Assembly>(_plugins);

            _pluginsPath = new DirectoryInfo(Path.Combine(GamePath.FullName, PLUGIN_FOLDER_NAME)).EnsureExists();
            _globalPluginsPath = new DirectoryInfo(Path.Combine(_pluginsPath.FullName, GLOBAL_FOLDER_NAME));
            _serverPluginsPath = GetPluginsPathByPort(server.Port).EnsureExists();
            _globalDependencyPath = new DirectoryInfo(Path.Combine(_globalPluginsPath.FullName, DEPENDENCY_FOLDER_NAME));
            _serverDependencyPath = GetDependencyPathByPort(server.Port).EnsureExists();
        }

        internal static void Intialize(string binPath, string gamePath, Server server)
        {
            if (!(Manager is null))
                throw new InvalidOperationException("Creating a second instance??!");
            else if (string.IsNullOrEmpty(binPath) || string.IsNullOrEmpty(gamePath) || server is null)
                throw new ArgumentException("Wrong arguments");

            Manager = new PluginManager(binPath, gamePath, server);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Checks the id for uniqueness.
        /// </summary>
        /// <returns>
        ///     true if the id is not unique,
        ///     otherwise false.
        /// </returns>
        public bool IsNotUniqueId(string id)
        {
            return !_plugins.Keys.Any(pl => pl.Metadata.Id.Equals(id, StringComparison.Ordinal));
        }

        /// <summary>
        ///     Checks if the plugin has been disposed.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Plugin is null.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     Plugin has been disposed.
        /// </exception>
        internal static void CheckPluginDisposed(Plugin? plugin)
        {
            if (plugin is null)
                throw new ArgumentNullException("Plugin cannot be null", nameof(plugin));
            else if (plugin.Status == PluginStatus.DISPOSED)
                throw new ObjectDisposedException(nameof(Plugin));
        }

        /// <summary>
        ///     Gets the plugins path for the port.
        /// </summary>
        public DirectoryInfo GetPluginsPathByPort(ushort port)
        {
            return new DirectoryInfo(Path.Combine(_pluginsPath.FullName, port.ToString()));
        }

        /// <summary>
        ///     Gets the dependency path for the port.
        /// </summary>
        public DirectoryInfo GetDependencyPathByPort(ushort port)
        {
            return new DirectoryInfo(Path.Combine(_pluginsPath.FullName, port.ToString(), DEPENDENCY_FOLDER_NAME));
        }

        /// <summary>
        ///     Gets all available plugins paths.
        /// </summary>
        public IEnumerable<DirectoryInfo> GetAvailablePluginsPaths()
        {
            foreach (var subfolder in _pluginsPath.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                if (ushort.TryParse(subfolder.Name, out _))
                    yield return subfolder;
            }
        }

        /// <summary>
        ///     Gets all available dependency paths.
        /// </summary>
        public IEnumerable<DirectoryInfo> GetAvailableDependencyPathes()
        {
            foreach (var subfolder in GetAvailablePluginsPaths().Select(d => new DirectoryInfo(Path.Combine(d.FullName, DEPENDENCY_FOLDER_NAME))))
            {
                if (subfolder.Exists)
                    yield return subfolder;
            }
        }

        #endregion

        #region Plugin related methods

        /// <summary>
        ///     Gets the plugin by its id.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Id is null or empty.
        /// </exception>
        public Plugin? GetPlugin(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id must not be empty or null", nameof(id));

            // Rolsynator is trying to suggest using `First` instead of `FirstOrDefault`
            // but the `First` method throws an exception if the value cannot be found,
            // we don't need it
#pragma warning disable RCS1077 // Optimize LINQ method call.
            return _plugins.Keys.FirstOrDefault(p => p.Metadata.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
#pragma warning restore RCS1077 // Optimize LINQ method call.
        }

        /// <summary>
        ///     Gets plugin status by id.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Id is null or empty.
        /// </exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        public PluginStatus? GetPluginStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Id must not be empty or null", nameof(id));

            Plugin? plugin;
            if ((plugin = GetPlugin(id)) is null)
                return null;

            return GetPluginStatus(plugin);
        }

        /// <summary>
        ///     Gets plugin status.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        public PluginStatus GetPluginStatus(Plugin plugin)
        {
            CheckPluginDisposed(plugin);

            return plugin.Status;
        }

        /// <summary>
        ///		Sets the status of the plugin and returns the old status.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        internal PluginStatus ChangePluginStatus(Plugin plugin, PluginStatus status)
        {
            CheckPluginDisposed(plugin);

            PluginStatus oldStatus = GetPluginStatus(plugin);
            plugin.Status = status;
            using var ev = new PluginChangedStatusEvent(plugin, status, oldStatus);
            EventMisc.InvokeSafely(PluginChangedStatus, ev);
            return oldStatus;
        }

        /// <summary>
        ///     Enables add loaded plugins.
        /// </summary>
        /// <remarks><inheritdoc cref="LoadedPlugins"/></remarks>
        /// <returns><inheritdoc cref="EnablePlugins" /></returns>
        public bool EnableLoadedPlugins() => LoadedPlugins.All(Enable);

        /// <summary>
        ///     Enables all disabled plugins.
        /// </summary>
        /// <returns>
        ///     true if all plugins have been successfully enabled,
        ///     otherwise false.
        /// </returns>
        public bool EnablePlugins() => DisabledPlugins.All(Enable);

        /// <summary>
        ///     Enables plugin.
        /// </summary>
        /// <returns>
        ///     true if the plugin hasn't thrown an exception when it's enabling,
        ///     otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="InvalidOperationException">
        ///     Plugin already enabled.
        /// </exception>
        public bool Enable(Plugin plugin)
        {
            if (GetPluginStatus(plugin) == PluginStatus.ENABLED)
                throw new InvalidOperationException("Plugin already enabled");

            if (plugin.Metadata.RuntimeSettings.HasSetting<DisabledRuntimeSetting>())
            {
                Info($"Enabling {plugin} plugin was aborted by the runtime setting");
                ChangePluginStatus(plugin, PluginStatus.DISABLED);
                return true;
            }

            using var ev1 = new PluginEnablingEvent(plugin);
            EventMisc.InvokeSafely(PluginEnabling, ev1);
            if (!ev1.Allow)
            {
                Info($"Enabling {plugin} plugin was aborted externally");
                return false;
            }

            var result = true;
            try
            {
                Info($"Enabling plugin: {plugin}");
                ChangePluginStatus(plugin, PluginStatus.ENABLED);

                try
                { plugin.CallEnable(); }
                catch (Exception e)
                {
                    Error($"Enabling the plugin throw an exception: {e.Message}");
                    Debug(e.ToString());
                    result = false;
                }

                Info($"Enabled plugin: {plugin}");
            }
            catch (Exception e)
            {
                Critical($"Somehow of enabling the plugin throw an exception: {e.Message}");
                Debug(e.ToString());
                result = false;
            }

            using var ev2 = new PluginEnabledEvent(plugin, result);
            EventMisc.InvokeSafely(PluginEnabled, ev2);
            return result;
        }

        /// <summary>
        ///     Enables plugin by its id.
        /// </summary>
        /// <returns>
        ///     true if the plugin hasn't thrown an exception when it's enabling,
        ///     otherwise false.
        ///     if the plugin by id was not found, then false will be returned.
        /// </returns>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetPlugin(string)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Enable(Plugin)"/></exception>
        public bool Enable(string id)
        {
            Plugin? plugin;
            if ((plugin = GetPlugin(id)) is null)
                return false;

            return Enable(plugin);
        }

        /// <summary>
        ///     Disables all enabled plugins.
        /// </summary>
        /// <returns>
        ///     true if all plugins have been successfully disabled,
        ///     otherwise false.
        /// </returns>
        public bool DisablePlugins() => EnabledPlugins.All(Disable);

        /// <summary>
        ///     Disables plugin.
        /// </summary>
        /// <returns>
        ///     true if the plugin hasn't thrown an exception when it's disabling,
        ///     otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="InvalidOperationException">
        ///     Plugin already disabled.
        /// </exception>
        public bool Disable(Plugin plugin)
        {
            if (GetPluginStatus(plugin) == PluginStatus.DISABLED)
                throw new InvalidOperationException("Plugin already disabled");

            using var ev1 = new PluginDisablingEvent(plugin);
            EventMisc.InvokeSafely(PluginDisabling, ev1);
            if (!ev1.Allow)
            {
                Info($"Disabling {plugin} plugin was aborted externally");
                return false;
            }

            var result = true;
            try
            {
                Info($"Disabling plugin: {plugin}");
                ChangePluginStatus(plugin, PluginStatus.DISABLED);

                try
                { plugin.CallDisable(); }
                catch (Exception e)
                {
                    Error($"Disabling the plugin throw an exception: {e.Message}");
                    Debug(e.ToString());
                    result = false;
                }

                Info($"Disabled plugin: {plugin}");
            }
            catch (Exception e)
            {
                Critical($"Somehow of disabling the plugin throw an exception: {e.Message}");
                Debug(e.ToString());
                result = false;
            }

            using var ev2 = new PluginDisabledEvent(plugin, result);
            EventMisc.InvokeSafely(PluginDisabled, ev2);
            return result;
        }

        /// <summary>
        ///     Disables plugin by its id.
        /// </summary>
        /// <returns>
        ///     true if the plugin hasn't thrown an exception when it's disabling,
        ///     otherwise false.
        ///     if the plugin by id was not found, then false will be returned.
        /// </returns>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetPlugin(string)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Disable(Plugin)"/></exception>
        public bool Disable(string id)
        {
            Plugin? plugin;
            if ((plugin = GetPlugin(id)) is null)
                return false;

            return Disable(plugin);
        }

        /// <inheritdoc />
        public override void Dispose(Plugin owner)
        {
            base.Dispose(owner);

            // Disabling the plugin before disposing it
            // Because the plugin must destroy most of its stuffs when disabling
            if (owner.Status != PluginStatus.DISABLED)
                Disable(owner);

            try
            {
                Info($"Disposing plugin: {owner}");
                ChangePluginStatus(owner, PluginStatus.DISPOSED);

                try
                { owner.CallDispose(); }
                catch (Exception e)
                {
                    Error($"Disposing the plugin throw an exception: {e.Message}");
                    Debug(e.ToString());
                }

                GACBypasser.Unload(_plugins[owner]);
                _plugins.Remove(owner);

                Info($"Disposed plugin: {owner}");
            }
            catch (Exception ex)
            {
                Critical($"Somehow of disposing the plugin throw an exception: {ex.Message}");
                Debug(ex.ToString());
            }

            ModuleManager.Manager.Dispose(owner);
            EventMisc.InvokeSafely(PluginDisposed, owner);
            GC.SuppressFinalize(owner);
        }

        /// <summary>
        ///     Disposes the plugin by its id.
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetPlugin(string)"/></exception>
        /// <exception cref="ObjectDisposedException"><inheritdoc cref="CheckPluginDisposed(Plugin?)"/></exception>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Dispose(Plugin)"/></exception>
        public void Dispose(string id)
        {
            Plugin? plugin;
            if ((plugin = GetPlugin(id)) is null)
                return;

            Dispose(plugin);
        }

        #endregion
    }
}
