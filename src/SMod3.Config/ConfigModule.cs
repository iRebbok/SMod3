using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SMod3.Core;
using SMod3.Core.Misc;
using SMod3.Module.Config.Meta.TypeReaders;

namespace SMod3.Module.Config
{
    public sealed class ConfigModule : Core.Module
    {
        public override string? LoggingTag { get; } = StringMisc.ToFullyUpperSnakeCase(nameof(ConfigModule));

        private static ConfigModule? _instance;
#nullable disable
        public static ConfigModule Module { get => _instance ??= ModuleManager.Manager.FindModule<ConfigModule>(); }
#nullable restore

#nullable disable
        public IConfigProvider GameplayConfig { get; internal set; }
#nullable restore

        private readonly List<ITypeReader> _typeReaders;

        private ConfigModule()
        {
            _typeReaders = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !(t.GetInterface(nameof(ITypeReader)) is null))
                .Select(t => Activator.CreateInstance(t, true))
                .Cast<ITypeReader>()
                .ToList();
        }

        public void AddTypeReader(ITypeReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            _typeReaders.Add(reader);
        }

        public bool IsReadableType(Type type) => TryGetTypeReader(type, out _);

        public bool TryGetTypeReader(Type type, out ITypeReader? reader)
        {
            for (var z = 0; z < _typeReaders.Count; z++)
            {
                reader = _typeReaders[z];
                if (reader.CanRead(type))
                    return true;
            }

            reader = null;
            return false;
        }
    }
}
