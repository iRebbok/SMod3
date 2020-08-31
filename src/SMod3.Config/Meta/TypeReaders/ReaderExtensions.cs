using System;
using System.Reflection;

using SMod3.Core.Fundamental;
using SMod3.Core.Misc;

namespace SMod3.Module.Config.Meta.TypeReaders
{
    public static class ReaderExtensions
    {
        public static void AssertCorrentType(this ITypeReader reader, ConfigAttributeWrapper wrapper)
        {
            if (!reader.CanRead(wrapper.GetTargetType()))
                throw new ArgumentException("Cannot read that type", "type");
        }

        public static Type GetTargetType(this ConfigAttributeWrapper wrapper) => wrapper.Type switch
        {
            BaseAttributeWrapper.TargetType.Field => (wrapper.Target as FieldInfo)!.FieldType,
            BaseAttributeWrapper.TargetType.Property => (wrapper.Target as PropertyInfo)!.PropertyType,
            _ => throw new ArgumentOutOfRangeException()
        };

        public static void AssignValue<T>(this ConfigAttributeWrapper wrapper, T value)
        {
            if (wrapper.AllowAssign(value, out var prepatedValue))
                wrapper.SetValue(prepatedValue);
        }

        public static bool AllowAssign<T>(this ConfigAttributeWrapper wrapper, T value, out object? preparedValue)
        {
            var targetType = wrapper.GetTargetType();

            var valueNullableType = Nullable.GetUnderlyingType(typeof(T));
            var targetNullableType = Nullable.GetUnderlyingType(targetType);

            var targetNullable = !(targetNullableType is null);
            var valueNullable = !(valueNullableType is null);

            if (!wrapper.Source.DoNotAssign && targetNullable && valueNullable && value is null)
            {
                preparedValue = value;
                return true;
            }

            if (!targetNullable && valueNullable && !(value is null))
            {
                preparedValue = Convert.ChangeType(value, valueNullableType);
                return true;
            }

            preparedValue = value;
            return value is null && !wrapper.Source.DoNotAssign;
        }

        public static void SetValue(this ConfigAttributeWrapper wrapper, object? value)
        {
            switch (wrapper.Type)
            {
                case BaseAttributeWrapper.TargetType.Field:
                    (wrapper.Target as FieldInfo)!.SetValue(wrapper.Instance, value);
                    break;
                case BaseAttributeWrapper.TargetType.Property:
                    (wrapper.Target as PropertyInfo)!.SetValue(wrapper.Instance, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetConfigKey(this ConfigAttributeWrapper wrapper)
        {
            if (!string.IsNullOrEmpty(wrapper.Source.Key))
                return wrapper.Source.Key!;

            string key = StringMisc.ToLowerSnakeCase(wrapper.Target.Name);
            wrapper.Source.Key = key;
            return key;
        }
    }
}
