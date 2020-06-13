using System;
using System.Collections.Generic;
using System.Reflection;

using SMod3.Core.Fundamental;
using SMod3.Core.Misc;

namespace SMod3.Core
{
    public sealed class ModuleManager : BaseManager
    {
        #region Properties

        public static ModuleManager Manager { get; } = new ModuleManager();

        public override string? LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(ModuleManager));

        #endregion

        private IList<Module> _modules = new List<Module>();

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

                        Verbose($"The module has an entry point");
                        var type = moduleAttrbite.Entry;
                        if (type.BaseType != typeof(Module))
                        {
                            Warn($"The base type of the module entry point isn't a module");
                            continue;
                        }

                        var cctor = type.GetConstructor(Type.EmptyTypes);
                        if (cctor is null)
                        {
                            Warn($"The module doesn't have the proper constructor at its entry point");
                            continue;
                        }

                        var module = cctor.Invoke(null) as Module;
                        if (module is null)
                        {
                            Warn($"The constructor called turned out to be null");
                            continue;
                        }
                        module.Metadata = new ModuleMetadata(assembly);

                        _modules.Add(module);

                        Verbose($"Calling Awake");
                        try
                        { module.CallAwake(); }
                        catch (Exception ex)
                        {
                            Error($"An exception occurred while calling Awake in a module");
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
    }
}
