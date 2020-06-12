using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using SMod3.Core.Fundamental;
using SMod3.Core.Misc;

namespace SMod3.Core
{
    public enum PluginStatus
    {
        DISABLED,
        ENABLED,
        /// <summary>
        ///     Plugin has been destroyed and awaiting GC collection.
        /// </summary>
        DESTROYED
    }

    public sealed class PluginManager : BaseManager
    {
        #region Static zone

        public const BindingFlags ALL_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static readonly string DEPENDENCY_FOLDER_NAME = "dependencies";
        public static readonly string PLUGIN_FOLDER_NAME = "sm_plugins";

        public static PluginManager Manager { get; } = new PluginManager();

        #endregion

        #region Properties

        public override string LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(PluginManager));

        private readonly List<Plugin> plugins = new List<Plugin>();
        private readonly Dictionary<string, Assembly> pluginAssemblies = new Dictionary<string, Assembly>();
        private readonly List<Assembly> dependencies = new List<Assembly>();

        private string? executingPath;
        /// <summary>
        ///		Location of the SMod3.Core.dll.
        ///		<para>
        ///			It is initially located in <c>Game/atlasLoader/bin</c>
        ///		</para>
        /// </summary>
        public string ExecutingPath
        {
            get
            {
                if (string.IsNullOrEmpty(executingPath))
                    executingPath = Assembly.GetExecutingAssembly().Location;
                return executingPath!;
            }
        }

        private string? gamePath;
        /// <summary>
        ///		Path with SCPSL.exe or SCPSL.x86_64 (if uses linux).
        /// </summary>
        public string GamePath
        {
            get
            {
                if (string.IsNullOrEmpty(gamePath))
                    gamePath = Path.Combine(ExecutingPath, "..", "..");
                return gamePath!;
            }
        }

        private string? pluginPath;
        /// <summary>
        ///		Path to the plugin folder.
        /// </summary>
        public string PluginPath
        {
            get
            {
                if (string.IsNullOrEmpty(pluginPath))
                    pluginPath = Path.Combine(GamePath, PLUGIN_FOLDER_NAME);
                return pluginPath!;
            }
        }

        private string? dependencyPath;
        /// <summary>
        ///		Path to the plugin dependencies.
        /// </summary>
        public string DependencyPath
        {
            get
            {
                if (string.IsNullOrEmpty(dependencyPath))
                    dependencyPath = Path.Combine(PluginPath, DEPENDENCY_FOLDER_NAME);
                return dependencyPath!;
            }
        }

        #endregion

        #region Plugin related properties

        /// <summary>
        ///     Returns plugins that have been enabled.
        /// </summary>
        public IEnumerable<Plugin> EnabledPlugins { get => plugins.Where(p => p.Status == PluginStatus.ENABLED).OrderByDescending(p => p.Metadata!.Priority); }

        /// <summary>
        ///     Returns plugins that have been disabled.
        /// </summary>
        public IEnumerable<Plugin> DisabledPlugins { get => plugins.Where(p => p.Status == PluginStatus.DISABLED).OrderByDescending(p => p.Metadata!.Priority); }

        /// <summary>
        ///     Returns all plugins.
        /// </summary>
        public IEnumerable<Plugin> Plugins { get => plugins.OrderByDescending(p => p.Metadata!.Priority).ThenBy(p => p.Status); }

        #endregion

        #region Plugin related events

        /// <summary>
        ///     Called when the plugin status changes.
        /// </summary>
        /// <remarks>
        ///     Called before the status is applied,
        ///     therefore, the previous status can be obtained.
        /// </remarks>
        public event Action<Plugin, PluginStatus>? ChangingStatus;

        /// <summary>
        ///     Called when the plugin is enabled.
        /// </summary>
        public event Action<Plugin, bool>? Enabled;

        /// <summary>
        ///     Called when the plugin is disabled.
        /// </summary>
        public event Action<Plugin, bool>? Disabled;

        /// <summary>
        ///     Called when the plugin is disposed.
        /// </summary>
        public event Action<Plugin>? Disposed;

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
            return !plugins.Any(pl => pl.Metadata!.Id.Equals(id));
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

            return plugins.FirstOrDefault(p => p.Metadata.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Gets plugin status by id.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Id is null or empty.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        ///     Plugin is null.
        /// </exception>
        public PluginStatus GetPluginStatus(Plugin plugin)
        {
            if (plugin is null)
                throw new ArgumentNullException("The argument must not be null", nameof(plugin));

            return plugin.Status;
        }

        /// <summary>
        ///		Sets the status of the plugin and returns the old status.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Plugin is null.
        /// </exception>
        internal PluginStatus ChangePluginStatus(Plugin plugin, PluginStatus status)
        {
            if (plugin is null)
                throw new ArgumentNullException("Argument must not be null", nameof(plugin));

            EventMisc.InvokeSafely(ChangingStatus, plugin, status);
            PluginStatus oldStatus = plugin.Status;
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
        public bool EnablePlugins()
        {
            return DisabledPlugins.All(plugin => Enable(plugin));
        }

        /// <summary>
        ///     Enable plugin.
        /// </summary>
        /// <returns>
        ///     true if the plugin hasn't thrown an exception when it's enabling,
        ///     otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Plugin is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Plugin already enabled.
        /// </exception>
        public bool Enable(Plugin plugin)
        {
            if (plugin is null)
                throw new ArgumentNullException("The argument must not be null", nameof(plugin));

            if (GetPluginStatus(plugin) == PluginStatus.ENABLED)
                throw new InvalidOperationException("Plugin already enabled");

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

            EventMisc.InvokeSafely(Enabled, plugin, result);
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
        public bool DisablePlugins()
        {
            return EnabledPlugins.All(plugin => Disable(plugin));
        }

        /// <summary>
        ///     Disable plugin.
        /// </summary>
        /// <returns>
        ///     true if the plugin hasn't thrown an exception when it's disabling,
        ///     otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Plugin is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Plugin already disabled.
        /// </exception>
        public bool Disable(Plugin plugin)
        {
            if (plugin is null)
                throw new ArgumentNullException("The plugin cannot be null", nameof(plugin));

            if (GetPluginStatus(plugin) == PluginStatus.DISABLED)
                throw new InvalidOperationException("Plugin already disabled");

            var result = true;
            try
            {
                Info($"Disabling plugin: {plugin}");
                ChangePluginStatus(plugin, PluginStatus.DISABLED);

                try { plugin.CallDisable(); }
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

            EventMisc.InvokeSafely(Disabled, plugin, result);
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
        /// <exception cref="InvalidOperationException"><inheritdoc cref="Disable(Plugin)"/></exception>
        public bool Disable(string id)
        {
            Plugin? plugin;
            if ((plugin = GetPlugin(id)) is null)
                return false;

            return Disable(plugin);
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">
        ///     Plugin already disposed.
        /// </exception>
        public override void Dispose(Plugin owner)
        {
            base.Dispose(owner);

            EventMisc.InvokeSafely(Disposed, owner);
        }

        /// <summary>
        ///     Disposes the plugin by its id.
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetPlugin(string)"/></exception>
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
            CheckBootstrapFolders(out _, out _);
            LoadDependencies();
            LoadPlugins();
        }

        /// <summary>
        ///     Checks the necessary folders for bootstrap.
        /// </summary>
        private void CheckBootstrapFolders(out bool pluginsFolderIsExists, out bool dependenciesFolderIsExists)
        {
            Verbose("Checking the desired folders");
            pluginsFolderIsExists = Directory.Exists(PluginPath);
            Verbose($"Plugins folder exists: {pluginsFolderIsExists}");
            dependenciesFolderIsExists = Directory.Exists(DependencyPath);
            Verbose($"Dependencies folder exists: {dependenciesFolderIsExists}");

            if (!pluginsFolderIsExists)
            {
                Verbose("Creating a plugins folder...");
                Directory.CreateDirectory(PluginPath);
            }

            if (!dependenciesFolderIsExists)
            {
                Verbose("Creating a dependencies folder...");
                Directory.CreateDirectory(DependencyPath);
            }
        }

        /// <summary>
        ///     Initializes dependency libraries.
        /// </summary>
        public void LoadDependencies()
        {
            try
            {
                CheckBootstrapFolders(out _, out bool dependenciesFolderIsExists);
                if (!dependenciesFolderIsExists)
                {
                    Verbose("No dependencies, folder just created");
                    return;
                }

                DirectoryInfo dir = new DirectoryInfo(DependencyPath);
                foreach (FileInfo file in dir.GetFiles("*.dll"))
                {
                    Info($"Loading dependency: {file.FullName}");
                    try
                    {
                        Assembly assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
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
        public void LoadPlugins()
        {
            try
            {
                CheckBootstrapFolders(out bool pluginsFolderIsExists, out _);
                if (!pluginsFolderIsExists)
                {
                    Verbose("No plugins, folder just created");
                    return;
                }

                // We load all plugin assemblies before initializing them
                var assemblies = new List<Assembly>();

                DirectoryInfo dirInfo = new DirectoryInfo(PluginPath);
                foreach (FileInfo file in dirInfo.GetFiles("*.dll"))
                {
                    Info($"Loading plugin assembly: {file.FullName}");
                    try
                    {
                        var assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
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
                Warn($"Attempt to identify a null assembly");
                throw new ArgumentNullException("The assembly cannot be null", nameof(assembly));
            }

            // todo: gac bypass
            if (dependencies.Contains(assembly))
            {
                Warn("Attempt to identify an already identified assembly");
                return;
            }

            dependencies.Add(assembly);
        }

        /// <summary>
        ///     Loads a plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     The assembly is null.
        /// </exception>
        public void LoadPlugin(Assembly assembly)
        {
            if (assembly is null)
            {
                Warn("Attempt to identify null assembly for a plugin");
                throw new ArgumentNullException("The assembly cannot be null", nameof(assembly));
            }

            if (pluginAssemblies.ContainsValue(assembly))
            {
                Warn("Attempt to identify an already identified assembly in the plugin");
                return;
            }

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

                    Verbose($"Calling Awake");
                    try { plugin.CallAwake(); }
                    catch (Exception e)
                    {
                        Warn($"An exception occurred while calling Awake in the plugin: {e.Message}");
                        Debug(e.ToString());
                    }

                    pluginAssemblies.Add(plugin.Metadata.Id, assembly);
                    plugins.Add(plugin);
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
        ///     Reloads plugins and their <c>.dll</c>s.
        /// </summary>
        public void ReloadPlugins()
        {
            foreach (var p in plugins)
                Dispose(p);

            LoadPlugins();
        }

        /// <summary>
        ///     Reloads the dependency <c>.dll</c>s.
        /// </summary>
        public void ReloadDependencies()
        {
            dependencies.Clear();
            LoadDependencies();
        }

        #endregion
    }
}
