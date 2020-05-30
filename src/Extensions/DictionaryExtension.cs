using System;
using System.Collections.Generic;
using System.Linq;

namespace SMod3.Extensions
{
	public static class DictionaryExtension
	{
		public static int RemoveWhereByValue<TKey, TValue>(this Dictionary<TKey, TValue> source, Predicate<TValue> predicate)
		{
			var values = source.Where(pair => predicate(pair.Value)).ToList();
			foreach (var pair in values) source.Remove(pair.Key);
			return values.Count;
		}

		public static int RemoveWhereByKey<TKey, TValue>(this Dictionary<TKey, TValue> source, Predicate<TKey> predicate)
		{
			var values = source.Where(pair => predicate(pair.Key)).ToList();
			foreach (var pair in values) source.Remove(pair.Key);
			return values.Count;
		}

		public static int RemoveWhereByPair<TKey, TValue>(this Dictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			var values = source.Where(pair => predicate(pair)).ToList();
			foreach (var pair in values) source.Remove(pair.Key);
			return values.Count;
		}
	}
}
