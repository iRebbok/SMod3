using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

using Mono.Cecil;

using NorthwoodLib.Pools;

using Random = UnityEngine.Random;

namespace SMod3.Core.Misc
{
    public sealed class BypassData
    {
        public string? Path { get; internal set; }

        public string SourceName { get; }

        public string Name { get; }

        public BypassData(string? path, string sourceName, string name)
        {
            Path = path;
            SourceName = sourceName;
            Name = name;
        }
    }

    /// <summary>
    ///     Bypasses the limitations of Global Assembly Cache when reloading libraries.
    /// </summary>
    public sealed class GacBypass
    {
        public const string ASSEMBLY_FILE_EXTENSION = ".dll";

        #region Properties

        /// <summary>
        ///     Prefix to all edited names.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        ///     GAC bypass counter.
        /// </summary>
        public uint Counter { get; private set; }

        /// <summary>
        ///     Dictionary of assemblies with their location.
        /// </summary>
        public IReadOnlyDictionary<Assembly, BypassData> Assemblies { get; }

        #endregion

        private readonly IDictionary<Assembly, BypassData> _assemblies;
        private readonly StringCollection _pathes;

        public GacBypass()
        {
            Prefix = GeneratePrefix();

            _assemblies = new Dictionary<Assembly, BypassData>();
            Assemblies = new ReadOnlyDictionary<Assembly, BypassData>(_assemblies);
            _pathes = new StringCollection();
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
        }

        ~GacBypass()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnResolveAssembly;
        }

        /// <summary>
        ///     Loads the assembly and bypasses the GAC if required.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The path empty or null
        ///     or the file extension is not as expected.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     No file.
        /// </exception>
        public Assembly Load(string path, bool forceBypass = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException(nameof(path));

            var fileInfo = new FileInfo(path);
            return Load(fileInfo, forceBypass);
        }

        /// <summary>
        ///     Loads the assembly and bypasses the GAC if required.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fileInfo"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     The file extension is not as expected.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     No file.
        /// </exception>
        public Assembly Load(FileInfo fileInfo, bool forceBypass = false)
        {
            if (fileInfo is null)
                throw new ArgumentNullException(nameof(fileInfo));

            if (!fileInfo.Exists)
                throw new FileNotFoundException();
            else if (fileInfo.Extension != ASSEMBLY_FILE_EXTENSION)
                throw new ArgumentException($"The file extension is not {ASSEMBLY_FILE_EXTENSION}", nameof(fileInfo));

            using var fileStream = fileInfo.OpenRead();
            var assembly = Load(fileStream, forceBypass || _pathes.Contains(fileInfo.FullName));
            _pathes.Add(fileInfo.FullName);
            _assemblies[assembly].Path = fileInfo.FullName;
            return assembly;
        }

        /// <summary>
        ///     Loads assembly from stream and bypasses the GAC if required.
        ///     This doesn't support the definition for the need to bypass the GAC,
        ///     you must specify yourself.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Stream is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     It's impossible to read from stream. -or another reason.
        /// </exception>
        public Assembly Load(Stream stream, bool bypass = false)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new IOException("Stream impossible to read");

            using var memoryStream = new MemoryStream();
            string sourceName = string.Empty;
            string name = string.Empty;
            if (bypass)
            {
                using var module = ModuleDefinition.ReadModule(stream);
                sourceName = module.Name;
                module.Name += $"{Prefix}{Counter++}";
                name = module.Name;
                module.Write(memoryStream);
            }
            else
            {
                stream.CopyTo(memoryStream);
            }

            var assembly = Assembly.Load(memoryStream.ToArray());
            if (!bypass)
            {
                sourceName = assembly.GetName().Name;
                name = sourceName;
            }

            _assemblies.Add(assembly, new BypassData(null, sourceName, name));
            return assembly;
        }

        /// <summary>
        ///     Removes assembly from <see cref="Assemblies"/> cache.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Assembly is null.
        /// </exception>
        /// <returns><inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/></returns>
        public bool Unload(Assembly assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            return _assemblies.Remove(assembly);
        }

        public Assembly? OnResolveAssembly(object s, ResolveEventArgs args)
        {
            foreach (var pair in _assemblies)
            {
                if (string.Equals(args.Name, pair.Value.Name, StringComparison.Ordinal) ||
                    string.Equals(args.Name, pair.Value.SourceName, StringComparison.Ordinal))
                {
                    return pair.Key;
                }
            }

            return null;
        }

        /// <summary>
        ///     Generates a prefix.
        /// </summary>
        public static string GeneratePrefix(uint minLength = 3)
        {
            minLength = minLength < 3 ? 3 : minLength;
            var maxLength = minLength * 3;

            var stringBuilder = StringBuilderPool.Shared.Rent((int)maxLength);
            // remember, `UnityEngine.Random(int, int)` is exclusive
            while ((stringBuilder.Length < maxLength && Random.Range(0, 1001) <= 500) || stringBuilder.Length < minLength)
            {
                var symbol = (char)Random.Range(33, 126);
                if (char.IsLetterOrDigit(symbol))
                    stringBuilder.Append(symbol);
            }

            var result = string.Concat("---", stringBuilder.ToString());
            StringBuilderPool.Shared.Return(stringBuilder);
            return result;
        }
    }
}
