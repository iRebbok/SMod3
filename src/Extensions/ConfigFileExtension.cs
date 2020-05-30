using System;
using System.Collections.Generic;
using System.Globalization;

using SMod3.Module.Config;
using SMod3.Module.Config.Meta;

namespace SMod3.Extensions
{
	public static class ConfigFileExtension
	{
		public static sbyte[] GetSByteArrayValue(this IConfigFile config, string key, sbyte[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<sbyte>();
			foreach (var source in sourceArray)
				if (sbyte.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static sbyte[] GetSByteArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetSByteArrayValue(config, key, Array.Empty<sbyte>(), isRandomized);
		}

		public static byte[] GetByteArrayValue(this IConfigFile config, string key, byte[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<byte>();
			foreach (var source in sourceArray)
				if (byte.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static byte[] GetByteArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetByteArrayValue(config, key, Array.Empty<byte>(), isRandomized);
		}

		public static ushort[] GetUInt16ArrayValue(this IConfigFile config, string key, ushort[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<ushort>();
			foreach (var source in sourceArray)
				if (ushort.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static ushort[] GetUInt16ArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetUInt16ArrayValue(config, key, Array.Empty<ushort>(), isRandomized);
		}

		public static short[] GetInt16ArrayValue(this IConfigFile config, string key, short[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<short>();
			foreach (var source in sourceArray)
				if (short.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static short[] GetInt16ArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetInt16ArrayValue(config, key, Array.Empty<short>(), isRandomized);
		}

		public static uint[] GetUInt32ArrayValue(this IConfigFile config, string key, uint[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<uint>();
			foreach (var source in sourceArray)
				if (uint.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static uint[] GetUInt32ArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetUInt32ArrayValue(config, key, Array.Empty<uint>(), isRandomized);
		}

		public static int[] GetInt32ArrayValue(this IConfigFile config, string key, int[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<int>();
			foreach (var source in sourceArray)
				if (int.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static int[] GetInt32ArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetInt32ArrayValue(config, key, Array.Empty<int>(), isRandomized);
		}

		public static ulong[] GetUInt64ArrayValue(this IConfigFile config, string key, ulong[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<ulong>();
			foreach (var source in sourceArray)
				if (ulong.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static ulong[] GetUInt64ArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetUInt64ArrayValue(config, key, Array.Empty<ulong>(), isRandomized);
		}

		public static long[] GetInt64ArrayValue(this IConfigFile config, string key, long[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<long>();
			foreach (var source in sourceArray)
				if (long.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static long[] GetInt64ArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetInt64ArrayValue(config, key, Array.Empty<long>(), isRandomized);
		}

		public static float[] GetSingleArrayValue(this IConfigFile config, string key, float[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<float>();
			foreach (var source in sourceArray)
				if (float.TryParse(source.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static float[] GetSingleArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetSingleArrayValue(config, key, Array.Empty<float>(), isRandomized);
		}

		public static double[] GetDoubleArrayValue(this IConfigFile config, string key, double[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<double>();
			foreach (var source in sourceArray)
				if (double.TryParse(source.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static double[] GetDoubleArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetDoubleArrayValue(config, key, Array.Empty<double>(), isRandomized);
		}

		public static decimal[] GetDecimalArrayValue(this IConfigFile config, string key, decimal[] def, bool isRandomized = false)
		{
			var sourceArray = config.GetStringArrayValue(key, isRandomized);
			var result = new List<decimal>();
			foreach (var source in sourceArray)
				if (decimal.TryParse(source.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
					result.Add(value);
			if (result.Count == 0 && def != null) result.AddRange(def);
			return ConfigFileParser.HandleConfigSet(key, result.ToArray(), def);
		}

		public static decimal[] GetDecimalArrayValue(this IConfigFile config, string key, bool isRandomized = false)
		{
			return GetDecimalArrayValue(config, key, Array.Empty<decimal>(), isRandomized);
		}
	}
}
