using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SMod3.Core.Misc;

namespace SMod3.Core.Fundamental.Native
{
    /// <summary>
    ///     Settings for serializing chunks.
    /// </summary>
    public struct ChunkSerializationSetting
    {
        /// <summary>
        ///     Defines the use of boolean for null objects.
        ///     Example - specifying one key is considered true, otherwise false.
        /// </summary>
        public bool NullToBoolean { get; set; }

        /// <summary>
        ///     Serialize exclusively fields/properties that are indicated by <see cref="ChuckSerializableAttribute"/>.
        /// </summary>
        public bool OnlyMemberSerialization { get; set; }

        /// <summary>
        ///     Hides exceptions and continues to serialize,
        ///     calling the exception handling method.
        /// </summary>
        public bool SilentExceptions { get; set; }

        /// <summary>
        ///     Called when an exception occurs when <see cref="SilentExceptions"/> is active.
        /// </summary>
        public Action<ChuckSerializationExceptionWrapper> OnException { get; set; }

        public static ChunkSerializationSetting Merge(ChunkSerializationSetting primary, ChunkSerializationSetting second)
        {
            primary.NullToBoolean = primary.NullToBoolean ? primary.NullToBoolean : second.NullToBoolean;
            primary.OnlyMemberSerialization = primary.OnlyMemberSerialization ? primary.OnlyMemberSerialization : second.NullToBoolean;
            primary.SilentExceptions = primary.SilentExceptions ? primary.OnlyMemberSerialization : second.OnlyMemberSerialization;
            primary.OnException ??= second.OnException;
            return primary;
        }
    }

    public static class ChunkSerializer
    {
        public static readonly ChunkSerializationSetting Default = new ChunkSerializationSetting
        {
            NullToBoolean = true,
            OnlyMemberSerialization = true,
            SilentExceptions = true,
        };

        public static T Serialize<T>(IEnumerable<ChunkDataAttribute> chunkDatas, ChunkSerializationSetting? setting = null) where T : new()
        {
            return (T)Serialize(typeof(T), chunkDatas, setting);
        }

        public static object Serialize(Type target, IEnumerable<ChunkDataAttribute> chunkDatas, ChunkSerializationSetting? setting = null)
        {
            void SetValue(object instance, MemberInfo target, object? value)
            {
                if (target is PropertyInfo property)
                {
                    if (property.PropertyType == typeof(bool) && value is null && setting.Value.NullToBoolean)
                        property.SetValue(instance, true);
                    else
                        property.SetValue(instance, value);
                }
                else if (target is FieldInfo field)
                {
                    if (field.FieldType == typeof(bool) && value is null && setting.Value.NullToBoolean)
                        field.SetValue(instance, true);
                    else
                        field.SetValue(instance, value);
                }
            }

            if (target?.IsAbstract != false || target.IsGenericType || target.IsInterface)
                throw new ArgumentException("Type must not be a null, abstract, interface, or generic type", nameof(target));

            if (chunkDatas?.Any() == false)
                throw new ArgumentException("No chunks or null", nameof(chunkDatas));

            setting ??= Default;

            var fields = target.GetFields();
            var properties = target.GetProperties();
            var fieldsAndProperies = new List<MemberInfo>(fields.Length + properties.Length);
            fieldsAndProperies.AddRange(fields);
            fieldsAndProperies.AddRange(properties);

            var instance = Activator.CreateInstance(target);

            foreach (var memeberInfo in fieldsAndProperies)
            {
                try
                {
                    if (memeberInfo is FieldInfo field && (field.IsStatic || field.IsInitOnly)) continue;
                    else if (memeberInfo is PropertyInfo property && !property.CanWrite) continue;

                    var attrb = memeberInfo.GetCustomAttributes<ChuckSerializableAttribute>();
                    if (attrb?.Any() != true)
                    {
                        if (setting.Value.OnlyMemberSerialization)
                            continue;

                        var chunkKey = StringMisc.ToCustomInterval(memeberInfo.Name, StringMisc.IntervalType.DotInterval, StringMisc.CaseType.UpperCase);
                        var chunk = chunkDatas.FirstOrDefault(c => c.Key.Equals(chunkKey, StringComparison.OrdinalIgnoreCase));
                        if (!(chunk is null))
                            SetValue(instance, memeberInfo, chunk.Value);
                    }
                    else
                    {
                        foreach (var chunkSerializable in attrb)
                        {
                            var chunk = chunkDatas.FirstOrDefault(c => c.Key.Equals(chunkSerializable.Key, StringComparison.OrdinalIgnoreCase));
                            if (chunk is null)
                                continue;

                            SetValue(instance, memeberInfo, chunk.Value);
                            break; // No longer sets values
                        }
                    }
                }
                catch (Exception ex) when (setting.Value.SilentExceptions)
                {
                    setting.Value.OnException?.Invoke(new ChuckSerializationExceptionWrapper(ex, memeberInfo));
                }
            }

            return instance;
        }
    }
}
