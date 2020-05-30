using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SMod3.Core;
using SMod3.Extensions;
using SMod3.Module.Attributes;
using SMod3.Module.Attributes.Meta;
using SMod3.Module.Config.Attributes;
using SMod3.Module.Config.Meta;

namespace SMod3.Module.Config
{
	public sealed class ConfigManager : BaseAttributeManager
	{
		/// <summary>
		///		Contains all the attributes <see cref="ConfigOptionAttribute"/> registered by the plugin.
		/// </summary>
		private readonly Dictionary<Plugin, HashSet<ConfigAttributeWrapper>> configFields = new Dictionary<Plugin, HashSet<ConfigAttributeWrapper>>();

		private readonly ReadOnlyDictionary<Type, Func<string, object, bool, object>> configGetters = new ReadOnlyDictionary<Type, Func<string, object, bool, object>>(
			new Dictionary<Type, Func<string, object, bool, object>>
			{
				[typeof(sbyte)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetSByteValue(key, def is sbyte ? (sbyte?)def : null, isRandomized),
				[typeof(sbyte?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetSByteValue(key, def is sbyte ? (sbyte?)def : null, isRandomized),
				[typeof(sbyte[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetSByteArrayValue(key, def is sbyte[]? (sbyte[])def : null, isRandomized),

				[typeof(byte)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetByteValue(key, def is byte ? (byte?)def : null, isRandomized),
				[typeof(byte?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetByteValue(key, def is byte ? (byte?)def : null, isRandomized),
				[typeof(byte[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetByteArrayValue(key, def is byte[]? (byte[])def : null, isRandomized),

				[typeof(ushort)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt16Value(key, def is ushort ? (ushort?)def : null, isRandomized),
				[typeof(ushort?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt16Value(key, def is ushort ? (ushort?)def : null, isRandomized),
				[typeof(ushort[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt16ArrayValue(key, def is ushort[]? (ushort[])def : null, isRandomized),

				[typeof(short)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt16Value(key, def is short ? (short?)def : null, isRandomized),
				[typeof(short?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt16Value(key, def is short ? (short?)def : null, isRandomized),
				[typeof(short[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt16ArrayValue(key, def is short[]? (short[])def : null, isRandomized),

				[typeof(uint)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt32Value(key, def is uint ? (uint?)def : null, isRandomized),
				[typeof(uint?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt32Value(key, def is uint ? (uint?)def : null, isRandomized),
				[typeof(uint[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt32ArrayValue(key, def is uint[]? (uint[])def : null, isRandomized),

				[typeof(int)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt32Value(key, def is int ? (int?)def : null, isRandomized),
				[typeof(int?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt32Value(key, def is int ? (int?)def : null, isRandomized),
				[typeof(int[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt32ArrayValue(key, def is int[]? (int[])def : null, isRandomized),

				[typeof(ulong)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt64Value(key, def is ulong ? (ulong?)def : null, isRandomized),
				[typeof(ulong?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt64Value(key, def is ulong ? (ulong?)def : null, isRandomized),
				[typeof(ulong[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetUInt64ArrayValue(key, def is ulong[]? (ulong[])def : null, isRandomized),

				[typeof(long)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt64Value(key, def is long ? (long?)def : null, isRandomized),
				[typeof(long?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt64Value(key, def is long ? (long?)def : null, isRandomized),
				[typeof(long[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetInt64ArrayValue(key, def is long[]? (long[])def : null, isRandomized),

				[typeof(float)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetSingleValue(key, def is float ? (float?)def : null, isRandomized),
				[typeof(float?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetSingleValue(key, def is float ? (float?)def : null, isRandomized),
				[typeof(float[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetSingleArrayValue(key, def is float[]? (float[])def : null, isRandomized),

				[typeof(double)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetDoubleValue(key, def is double ? (double?)def : null, isRandomized),
				[typeof(double?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetDoubleValue(key, def is double ? (double?)def : null, isRandomized),
				[typeof(double[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetDoubleArrayValue(key, def is double[]? (double[])def : null, isRandomized),

				[typeof(decimal)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetDecimalValue(key, def is decimal ? (decimal?)def : null, isRandomized),
				[typeof(decimal?)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetDecimalValue(key, def is decimal ? (decimal?)def : null, isRandomized),
				[typeof(decimal[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetDecimalArrayValue(key, def is decimal[]? (decimal[])def : null, isRandomized),

				[typeof(string)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetRawValue(key, def is string ? (string)def : null, isRandomized),
				[typeof(string[])] = (key, def, isRandomized) => ConfigFileParser.Parser.GetStringArrayValue(key, def is string[]? (string[])def : null, isRandomized),

				[typeof(Dictionary<string, string>)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetStringDictionary(key, def is Dictionary<string, string> ? (Dictionary<string, string>)def : null, isRandomized),
				[typeof(Dictionary<int, int>)] = (key, def, isRandomized) => ConfigFileParser.Parser.GetIntDictionary(key, def is Dictionary<int, int> ? (Dictionary<int, int>)def : null, isRandomized),
			});

		public override string LoggingTag => "CONFIG_MANAGER";

		public static ConfigManager Manager { get; } = new ConfigManager();

		public IConfigFile Config => ConfigFileParser.Parser;

		public override void RegisterAttributes<TInstance>(Plugin plugin, TInstance instance)
		{
			if (plugin == null || instance == null) return;

			if (string.IsNullOrEmpty(plugin.Definer.ConfigPrefix))
			{
				Error($"{plugin} tried to register attributes, but doesn't have a defined {nameof(PluginDefineAttribute.ConfigPrefix)}");
				return;
			}

			var attributes = AttributeManager.Manager.PullAttributes<ConfigOptionAttribute, TInstance>(instance);
			foreach (var pair in attributes)
			{
				string key = pair.Key.Key ?? PluginManager.ToLowerSnakeCase(pair.Value.Name, false);

				if (!configFields.TryGetValue(plugin, out HashSet<ConfigAttributeWrapper> hashset))
				{
					hashset = new HashSet<ConfigAttributeWrapper>();
					configFields.Add(plugin, hashset);
				}

				hashset.Add(new ConfigAttributeWrapper(plugin, key, pair.Key.Randomized, instance, pair.Value));
			}
		}

		public override void RefreshAttributes(Plugin plugin)
		{
			if (plugin == null) return;
			if (!configFields.TryGetValue(plugin, out HashSet<ConfigAttributeWrapper> hashset)) return;

			foreach (var wrapper in hashset)
			{
				var value = ProcessValue(wrapper.ConfigKey, wrapper.Field.GetValue(wrapper.Instance), wrapper.Randomized, wrapper.Field.FieldType);
				if (value != null && wrapper.Field.FieldType.IsAssignableFrom(value.GetType())) wrapper.Field.SetValue(wrapper.Instance, value);
			}
		}

		private object ProcessValue(string configKey, object def, bool isRandomized, Type fieldType)
		{
			if (fieldType == null || string.IsNullOrEmpty(configKey)) return null;
			if (configGetters.TryGetValue(fieldType, out var getter))
			{
				return getter.Invoke(configKey, def, isRandomized);
			}
			return null;
		}

		public override void Dispose(Plugin owner)
		{
			if (owner == null) return;

			configFields.Remove(owner);
		}
	}
}
