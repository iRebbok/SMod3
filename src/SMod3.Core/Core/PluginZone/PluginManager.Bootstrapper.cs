using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using SMod3.Core.Fundamental;
using SMod3.Core.Misc;
using SMod3.Core.PluginZone.Meta;

namespace SMod3.Core
{
    public sealed partial class PluginManager
    {
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

                    var pluginCCtor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public, null, Type.EmptyTypes, null);
                    if (pluginCCtor is null)
                    {
                        Verbose("Constructor with empty arguments was not found, skipping...");
                        continue;
                    }

                    var ev = new PluginLoadingEvent(type, metadataAttribute);
                    EventMisc.InvokeSafely(PluginLoading, ev);
                    if (!ev.Allow)
                    {
                        Info("Plugin loading was aborted externally");
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

                    plugin.Metadata = new PluginMetadata(metadataAttribute, assembly, ev.ExtraDatas);
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
