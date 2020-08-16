using System;
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

        private readonly ITypeReader[] _typeReaders;

        private ConfigModule()
        {
            _typeReaders = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !(t.GetInterface(nameof(ITypeReader)) is null))
                .Select(t => Activator.CreateInstance(t, true))
                .Cast<ITypeReader>()
                .ToArray();
        }

        private bool IsReadableType(Type type) => TryGetTypeReader(type, out _);

        private bool TryGetTypeReader(Type type, out ITypeReader? reader)
        {
            for (var z = 0; z < _typeReaders.Length; z++)
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
