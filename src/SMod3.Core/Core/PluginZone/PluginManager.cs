using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

using SMod3.API;
using SMod3.Core.Fundamental;
using SMod3.Core.Misc;

namespace SMod3.Core
{
    public sealed class PluginManager : BaseManager
    {
        #region Static zone

        public const string DLL_SEARCH_PATTERN = "*" + GacBypass.ASSEMBLY_FILE_EXTENSION;

        public static readonly string DEPENDENCY_FOLDER_NAME = "dependencies";
        public static readonly string PLUGIN_FOLDER_NAME = "sm_plugins";
        public static readonly string GLOBAL_FOLDER_NAME = "common";

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public static PluginManager Manager { get; private set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        #endregion

        #region Variables & Properties

        public override string LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(PluginManager));

        private readonly Dictionary<Plugin, Assembly> _plugins = new Dictionary<Plugin, Assembly>();
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

        public IReadOnlyDictionary<Plugin, Assembly> PluginsAndAssemblies { get; }

        /// <summary>
        ///     Returns plugins that have been enabled.
        /// </summary>
        public IEnumerable<Plugin> EnabledPlugins => from p in _plugins.Keys where p.Status == PluginStatus.ENABLED orderby p.Metadata.Priority select p;

        /// <summary>
        ///     Returns plugins that have been disabled.
        /// </summary>
        public IEnumerable<Plugin> DisabledPlugins => from p in _plugins.Keys where p.Status == PluginStatus.DISABLED orderby p.Metadata.Priority select p;

        /// <summary>
        ///     Returns all plugins.
        /// </summary>
        public IEnumerable<Plugin> Plugins => from p in _plugins.Keys orderby p.Metadata.Priority descending, p.Status select p;

        #endregion

        #region Plugin related events

        public delegate void OnPluginChangingStatus(Plugin target, PluginStatus nextStatus);
        /// <summary>
        ///     Called when the plugin status changes.
        /// </summary>
        /// <remarks>
        ///     Called before the status is applied,
        ///     therefore, the previous status can be obtained.
        /// </remarks>
        public event OnPluginChangingStatus? PluginChangingStatus;

        public delegate void OnPluginEnabling(Plugin target, ref bool allow);
        /// <summary>
        ///     Called when the plugin starts,
        ///     allowing it to be allowed or denied to continue.
        /// </summary>
        public event OnPluginEnabling? PluginEnabling;

        public delegate void OnPluginEnabled(Plugin target, bool success);
        /// <summary>
        ///     Called when the plugin is enabled.
        /// </summary>
        public event OnPluginEnabled? PluginEnabled;

        public delegate void OnPluginLoading(PluginMetadata metadata, ref bool allow);
        /// <summary>
        ///     Called when loading the plugin, allowing to prevent loading.
        /// </summary>
        public event OnPluginLoading? PluginLoading;

        public delegate void OnPluginDisabling(Plugin target, ref bool allow);
        /// <summary>
        ///     Called when the plugin is disabled,
        ///     allowing it to be allowed or denied to continue.
        /// </summary>
        public event OnPluginDisabling? PluginDisabling;

        public delegate void OnPluginDisabled(Plugin target, bool success);
        /// <summary>
        ///     Called when the plugin is disabled.
        /// </summary>
        public event OnPluginDisabled? PluginDisabled;

        /// <summary>
        ///     Called when the plugin is disposed.
        /// </summary>
        public event Action<Plugin>? PluginDisposed;

        #endregion

        /// <exception cref="InvalidOperationException">
        ///     Recall when <see cref="Manager"/> is already assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Empty/Null arguments.
        /// </exception>
        internal PluginManager(string binPath, string gamePath, Server server)
        {
            if (Manager is null)
                throw new InvalidOperationException("Creating a second instance ??!");
            else if (string.IsNullOrEmpty(binPath) || string.IsNullOrEmpty(gamePath) || server is null)
                throw new ArgumentException("Wrong arguments");

            Manager = this;

            Server = server;
            GACBypasser = new GacBypass();

            BinPath = new DirectoryInfo(binPath);
            GamePath = new DirectoryInfo(gamePath);

            PluginsAndAssemblies = new ReadOnlyDictionary<Plugin, Assembly>(_plugins);

            _pluginsPath = new DirectoryInfo(Path.Combine(GamePath.FullName, PLUGIN_FOLDER_NAME)).EnsureExists();
            _globalPluginsPath = new DirectoryInfo(Path.Combine(_pluginsPath.FullName, GLOBAL_FOLDER_NAME));
            _serverPluginsPath = new DirectoryInfo(Path.Combine(_pluginsPath.FullName, server.Port.ToString())).EnsureExists();
            _globalDependencyPath = new DirectoryInfo(Path.Combine(_globalPluginsPath.FullName, DEPENDENCY_FOLDER_NAME));
            _serverDependencyPath = new DirectoryInfo(Path.Combine(_serverPluginsPath.FullName, DEPENDENCY_FOLDER_NAME)).EnsureExists();
        }

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
            return !_plugins.Keys.Any(pl => pl.Metadata!.Id.Equals(id));
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

            EventMisc.InvokeSafely(PluginChangingStatus, plugin, status);
            PluginStatus oldStatus = GetPluginStatus(plugin);
            plugin.Status = status;
            return oldStatus;
        }

        /// <summary>
        ///     Enable all disabled plugins.
        /// </summary>
        /// <returns>
        ///     true if all plugins have been successfully enabled,
        ///     otherwise false.
        /// </returns>
        public bool EnablePlugins() => DisabledPlugins.All(Enable);

        /// <summary>
        ///     Enable plugin.
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

            // This is the ref variable
#pragma warning disable RCS1118 // Mark local variable as const.
            var allow = true;
#pragma warning restore RCS1118 // Mark local variable as const.
            EventMisc.InvokeSafely(PluginEnabling, plugin, allow);
            if (!allow)
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

            EventMisc.InvokeSafely(PluginEnabled, plugin, result);
            return result;
        }

        /// <summary>
        ///     Enable plugin by its id.
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
        ///     Disable all enabled plugins.
        /// </summary>
        /// <returns>
        ///     true if all plugins have been successfully disabled,
        ///     otherwise false.
        /// </returns>
        public bool DisablePlugins() => EnabledPlugins.All(Disable);

        /// <summary>
        ///     Disable plugin.
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

            // This is the ref variable
#pragma warning disable RCS1118 // Mark local variable as const.
            var allow = true;
#pragma warning restore RCS1118 // Mark local variable as const.
            EventMisc.InvokeSafely(PluginDisabled, plugin, allow);
            if (!allow)
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

            EventMisc.InvokeSafely(PluginDisabled, plugin, result);
            return result;
        }

        /// <summary>
        ///     Disable plugin by its id.
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

        #region Bootstrapper zone 

        /// <summary>
        ///     Entry point for bootstrap.
        /// </summary>
        internal void Load()
        {
            LoadDependencies();
            LoadPlugins();
        }

        internal void LoadDependencies()
        {
            if (GlobalDependencyPath.Exists)
                LoadDependencies(GlobalDependencyPath.GetFiles(DLL_SEARCH_PATTERN));

            LoadDependencies(ServerDependencyPath.GetFiles(DLL_SEARCH_PATTERN));
        }

        internal void LoadPlugins()
        {
            if (GlobalPluginsPath.Exists)
                LoadPlugins(GlobalPluginsPath.GetFiles(DLL_SEARCH_PATTERN));

            LoadPlugins(ServerPluginsPath.GetFiles(DLL_SEARCH_PATTERN));
        }

        /// <summary>
        ///     Initializes dependency libraries.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Files is null.
        /// </exception>
        public void LoadDependencies(FileInfo[] files)
        {
            if (files is null)
                throw new ArgumentNullException("Files cannot be null", nameof(files));

            try
            {
                foreach (FileInfo file in files)
                {
                    Info($"Loading dependency: {file.FullName}");
                    try
                    {
                        var assembly = GACBypasser.Load(file.FullName);
                        Verbose($"Loaded dependency: {assembly.FullName}");
                        LoadDependency(assembly);
                    }
                    catch (IOException e)
                    {
                        Error($"An exception of type IO occurred (is the file used by another process?) when loading a dependency file: {e.Message}");
                        Debug(e.ToString());
                    }
                    catch (BadImageFormatException e)
                    {
                        Error($"An exception of type BadImageFormat occurred (is the file not a .NET class library?) when loading a dependency file: {e.Message}");
                        Debug("More detailed information:");
                        Debug($"- FusionLog: {e.FusionLog}");
                        Debug(e.ToString());
                    }
                    catch (Exception e)
                    {
                        Error($"An exception occurred while loading the dependency file: {e.Message}");
                        Debug(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Critical($"An exception occurred while loading dependencies: {e.Message}");
                Debug(e.ToString());
            }
        }

        /// <summary>
        ///     Initializes plugin libraries.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Files is null.
        /// </exception>
        public void LoadPlugins(FileInfo[] files)
        {
            if (files is null)
                throw new ArgumentNullException("Files cannot be null", nameof(files));

            try
            {
                // We load all plugin assemblies before initializing them
                // This avoids issues such as TypeInitializationException/FileNotFoundException,
                // when a plugin accesses another plugin, although this is bad practice and there is Piping for this
                var assemblies = new List<Assembly>(files.Length);

                foreach (var file in files)
                {
                    Info($"Loading plugin assembly: {file.FullName}");
                    try
                    {
                        var assembly = GACBypasser.Load(file.FullName);
                        assemblies.Add(assembly);
                    }
                    catch (IOException e)
                    {
                        Error($"An exception of type IO occurred (is the file used by another process?) when loading a plugin file: {e.Message}");
                        Debug(e.ToString());
                    }
                    catch (BadImageFormatException e)
                    {
                        Error($"An exception of type BadImageFormat occurred (is the file not a .NET class library?) when loading a plugin file: {e.Message}");
                        Debug("More detailed information:");
                        Debug($"- FusionLog: {e.FusionLog}");
                        Debug(e.ToString());
                    }
                    catch (Exception e)
                    {
                        Error($"An exception occurred while loading the plugin file: {e.Message}");
                        Debug(e.ToString());
                    }
                }

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        LoadPlugin(assembly);
                    }
                    catch (Exception e)
                    {
                        Error($"An exception occurred while loading the plugin: {e.Message}");
                        Debug(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Critical($"An exception occurred while loading plugins: {e.Message}");
                Debug(e.ToString());
            }
        }

        /// <summary>
        ///     Loads the assembly.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     The assembly is null.
        /// </exception>
        public void LoadDependency(Assembly assembly)
        {
            Verbose($"Calling {nameof(LoadDependency)}");
            if (assembly is null)
            {
                Warn("Attempt to identify a null assembly");
                throw new ArgumentNullException("The assembly cannot be null", nameof(assembly));
            }

            if (!_dependencies.Add(assembly))
            {
                Warn("Attempt to identify an already identified assembly");
            }
        }

        /// <summary>
        ///     Loads a plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Assembly is null.
        /// </exception>
        public void LoadPlugin(Assembly assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            if (_plugins.ContainsValue(assembly))
                Warn($"Plugin assembly that has already been loadled is loading: {assembly.FullName}");

            // Class types for searching among them plugin
            Type[] typesForSearch;

            Verbose($"Initializing the assembly as a plugin: {assembly.FullName}");
            var pluginDefineAttribute = assembly.GetCustomAttribute<PluginDefineAttribute>();
            Verbose($"The defining attribute is null: {pluginDefineAttribute is null}");
            if (!(pluginDefineAttribute is null))
            {
                Verbose("Defining attribute processing");
                typesForSearch = pluginDefineAttribute.Types;
            }
            else
            {
                Verbose("Predefined types were not found, process everything...");
                typesForSearch = assembly.GetTypes();
            }

            foreach (var type in typesForSearch)
            {
                try
                {
                    Verbose($"Processing type: {type?.FullName ?? "null"}");
                    if (type is null)
                    {
                        Verbose("Somehow the type is null, skipping...");
                        continue;
                    }

                    var metadataAttribute = type.GetCustomAttribute<PluginMetadataAttribute>();
                    Verbose($"Methodata aatribute is null: {metadataAttribute is null}");
                    if (metadataAttribute is null)
                    {
                        Verbose("Metadata attribute is null, skipping...");
                        continue;
                    }

                    if (IsNotUniqueId(metadataAttribute.Id))
                    {
                        Error("The type metadata id is not unique; the type will not go through further processing");
                        continue;
                    }

                    // Here we check for the base type instead of 'Type::IsSubclassOf(Type)'
                    // because this is how we allow
                    // TypeLoadException, TypeInitializationException and FileNotFoundException
                    // when the dependency cannot be found in runtime.
                    // Somehow 'Type::IsSubclassOf(Type)' just returns false when such an exception is possible.
                    var typeBaseTypeIsPlugin = type.BaseType == typeof(Plugin);
                    Verbose($"Base type equals plugin type: {typeBaseTypeIsPlugin}");

                    if (!typeBaseTypeIsPlugin)
                    {
                        Verbose("Base type not equals plugin type, skipping...");
                        continue;
                    }

                    var pluginCCtor = type.GetConstructor(Type.EmptyTypes);
                    if (pluginCCtor is null)
                    {
                        Verbose("Constructor with empty arguments was not found, skipping...");
                        continue;
                    }

                    Plugin? plugin;
                    try
                    {
                        plugin = pluginCCtor.Invoke(null) as Plugin;
                        Verbose("The plugin instance was created successfully");
                    }
                    catch (Exception e)
                    {
                        Error($"An exception occurred while creating the plugin instance: {e.Message}");
                        Debug(e.ToString());
                        continue;
                    }

                    if (plugin is null)
                    {
                        Error("Somehow the plugin instance is null, skipping...");
                        continue;
                    }

                    plugin.Metadata = new PluginMetadata(metadataAttribute, assembly);
                    Verbose($"The plugin is remembered as: {plugin}");

                    Verbose("Calling Awake");
                    try { plugin.CallAwake(); }
                    catch (Exception e)
                    {
                        Warn($"An exception occurred while calling Awake in the plugin: {e.Message}");
                        Debug(e.ToString());
                    }

                    _plugins.Add(plugin, assembly);
                }
                catch (Exception e)
                {
                    Error($"An exception occurred while initializing the plugin: {e.Message}");
                    Debug(e.ToString());
                }
            }
        }

        /// <summary>
        ///     Disables and enables plugins.
        ///     Doesn't reload them <c>.dll</c>.
        /// </summary>
        public void RestartPlugins()
        {
            DisablePlugins();
            EnablePlugins();
        }

        /// <summary>
        ///     Reloads dependencies and plugins.
        /// </summary>
        public void Reload()
        {
            ReloadDependencies();
            ReloadPlugins();
        }

        /// <summary>
        ///     Reloads plugins and their <c>.dll</c>s.
        /// </summary>
        public void ReloadPlugins()
        {
            // We create an array from the existing list of plugins
            // because `Dispose (Plugin)` edits `_plugins`,
            // so we avoid `InvalidOperationException`
            foreach (var plugin in _plugins.Keys.ToArray())
                Dispose(plugin);

            LoadPlugins();
        }

        /// <summary>
        ///     Reloads the dependency <c>.dll</c>s.
        /// </summary>
        public void ReloadDependencies()
        {
            _dependencies.Clear();
            LoadDependencies();
        }

        #endregion
    }
}
