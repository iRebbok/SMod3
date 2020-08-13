using System;
using System.Collections.Generic;

namespace SMod3.Core.RuntimeSettings
{
    /// <summary>
    ///     A specialized collection of runtime plugin settings.
    /// </summary>
    public sealed class RuntimeSettingCollection : List<IRuntimeSetting>
    {
        internal RuntimeSettingCollection() : base(20) { }

        /// <summary><inheritdoc cref="HasSetting(Type)" /></summary>
        /// <typeparam name="T">
        ///     Runtime setting type.
        /// </typeparam>
        /// <returns><inheritdoc cref="HasSetting(Type)" /></returns>
        public bool HasSetting<T>() where T : IRuntimeSetting => HasSetting(typeof(T));

        /// <summary>
        ///     Indicates whether a runtime setting is in the collection.
        /// </summary>
        /// <param name="settingType">
        ///     Runtime setting type.
        /// </param>
        /// <returns>
        ///     true if it's in the collection; otherwise, false.
        /// </returns>
        public bool HasSetting(Type settingType)
        {
            for (var z = 0; z < Count; z++)
            {
                if (this[z].GetType() == settingType)
                    return true;
            }

            return false;
        }
    }
}
