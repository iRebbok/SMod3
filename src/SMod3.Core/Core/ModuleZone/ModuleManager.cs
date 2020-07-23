using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using SMod3.Core.Fundamental;
using SMod3.Core.Misc;

namespace SMod3.Core
{
    public sealed class ModuleManager : BaseManager
    {
        #region Properties

        public static ModuleManager Manager { get; } = new ModuleManager();

        public IReadOnlyDictionary<Assembly, Module> Modules { get; }

        public override string? LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(ModuleManager));

        #endregion

        private readonly IDictionary<Assembly, Module> _modules;

        // Close the constructor from unnecessary eyes
        private ModuleManager()
        {
            _modules = new Dictionary<Assembly, Module>();
            Modules = new ReadOnlyDictionary<Assembly, Module>(_modules);
        }

        /// <remarks>
        ///     Although this method does nothing special,
        ///     it just instantiates the modules and calls <see cref="Module.Awake"/> them.
        /// </remarks>
        public void LoadModules()
        {
            try
            {
                Verbose($"Calling {nameof(LoadModules)}");
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Verbose($"Got assemblies: there are {assemblies.Length} of them");
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        if (_modules.TryGetValue(assembly, out var preModule))
                        {
                            Warn($"Attempt to re-process the module {preModule}");
                            continue;
                        }

                        Verbose($"Processing: '{assembly.FullName}' from {(string.IsNullOrEmpty(assembly.Location) ? "undefined" : assembly.Location)}");
                        var moduleAttrbite = assembly.GetCustomAttribute<ModuleDefineAttribute>();
                        if (moduleAttrbite is null)
                        {
                            Verbose("Assembly isn't a module");
                            continue;
                        }

                        if (moduleAttrbite.Entry is null)
                        {
                            var metadata = new ModuleMetadata(assembly);
                            Info($"Standalone module '{metadata.Id}' ({metadata.Version})  successfully recognized");
                            continue;
                        }

                        Verbose("The module has an entry point");
                        var type = moduleAttrbite.Entry;
                        if (type.BaseType != typeof(Module))
                        {
                            Warn("The base type of the module entry point isn't a module");
                            continue;
                        }

                        var cctor = type.GetConstructor(Type.EmptyTypes);
                        if (cctor is null)
                        {
                            Warn("The module doesn't have the proper constructor at its entry point");
                            continue;
                        }

                        if (!(cctor.Invoke(null) is Module module))
                        {
                            Warn("The constructor called turned out to be null");
                            continue;
                        }
                        module.Metadata = new ModuleMetadata(assembly);

                        _modules.Add(assembly, module);

                        Verbose("Calling Awake");
                        try
                        { module.CallAwake(); }
                        catch (Exception ex)
                        {
                            Error($"An exception occurred while calling Awake in a module: {ex.Message}");
                            Debug(ex.ToString());
                        }

                        Info($"Module '{module.Metadata.Id}' ({module.Metadata.Version}) loaded successfully");
                    }
                    catch (Exception ex)
                    {
                        Error($"An exception occurred in enumerating assemblies: {ex}");
                        Debug(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Critical($"An exception occurred while loading modules: {ex.Message}");
                Debug(ex.ToString());
            }
        }

        /// <summary>
        ///     Finds and returns a specific module type.
        /// </summary>
        public T? FindModule<T>() where T : Module
        {
            foreach (var pair in _modules)
            {
                if (pair.Value is T mod)
                {
                    return mod;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public override void Dispose(Plugin owner)
        {
            base.Dispose(owner);

            Debug($"Finalizing {owner} plugin for modules");
            try
            {
                foreach (var modulePair in _modules)
                {
                    var module = modulePair.Value;
                    Debug($"Processing the plugin finalizer in the {module} module");

                    try
                    { module.Dispose(owner); }
                    catch (Exception ex)
                    {
                        Warn($"Error during call of plugin finalizer at module {module.Metadata.Id}: {ex.Message}");
                        Debug(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Critical($"Somehow an exception was thrown during the finalization of the plugin for modules: {ex.Message}");
                Debug(ex.ToString());
            }
        }
    }
}
