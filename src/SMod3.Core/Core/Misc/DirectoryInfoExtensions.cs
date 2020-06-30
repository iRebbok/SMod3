using System;
using System.IO;

namespace SMod3.Core.Misc
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        ///     Make sure the folder exists, and creating if it does not exist.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Directory is null.
        /// </exception>
        /// <returns>
        ///     The same.
        /// </returns>
        public static DirectoryInfo EnsureExists(this DirectoryInfo directory, bool onlyRefresh = false)
        {
            if (directory is null)
                throw new ArgumentNullException(nameof(directory));

            directory.Refresh();
            if (!onlyRefresh && !directory.Exists)
                directory.Create();

            return directory;
        }
    }
}
