using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;

using Mono.Cecil;

using Random = UnityEngine.Random;

namespace SMod3.Core.Misc
{
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
        public IReadOnlyDictionary<Assembly, string> Assemblies { get; }

        #endregion

        private readonly IDictionary<Assembly, string> _assemblies;
        private readonly StringCollection _pathes;

        public GacBypass()
        {
            Prefix = $"---{GeneratePrefix()}";

            _assemblies = new Dictionary<Assembly, string>();
            Assemblies = new ReadOnlyDictionary<Assembly, string>(_assemblies);
            _pathes = new StringCollection();
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
            _assemblies[assembly] = fileInfo.FullName;
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
            if (bypass)
            {
                using var module = ModuleDefinition.ReadModule(stream);
                module.Name += $"{Prefix}{Counter++}";
                module.Write(memoryStream);
            }
            else
            {
                stream.CopyTo(memoryStream);
            }

            var assembly = Assembly.Load(memoryStream.ToArray());
            _assemblies.Add(assembly, string.Empty);
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

        /// <summary>
        ///     Generates a prefix.
        /// </summary>
        public static string GeneratePrefix(uint minLength = 3)
        {
            minLength = minLength < 3 ? 3 : minLength;
            var maxLength = minLength * 3;

            var stringBuilder = new StringBuilder((int)minLength, (int)maxLength);
            // remember, `UnityEngine.Random(int, int)` is exclusive
            while ((stringBuilder.Length < maxLength && Random.Range(0, 1001) <= 500) || stringBuilder.Length < minLength)
            {
                var symbol = (char)Random.Range(33, 126);
                if (char.IsLetterOrDigit(symbol))
                    stringBuilder.Append(symbol);
            }

            return stringBuilder.ToString();
        }
    }
}
