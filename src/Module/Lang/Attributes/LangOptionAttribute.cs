using System;

using SMod3.Module.Attributes.Meta;

namespace SMod3.Module.Lang.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class LangOptionAttribute : BaseAttribute
	{
		public string Key { get; }

		public string Filename { get; }

		public LangOptionAttribute() { }

		public LangOptionAttribute(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentException("Lang keys cannot be null, whitespace, or empty.", nameof(key));
			}

			Key = key;
		}

		public LangOptionAttribute(string key, string filename) : this(key)
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentException("Lang filename cannot be null, whitespace, ot empty.", nameof(filename));
			}

			Filename = filename;
		}
	}
}
