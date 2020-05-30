using System.Reflection;

using SMod3.Core;
using SMod3.Module.Attributes.Meta;

namespace SMod3.Module.Config.Meta
{
	public class ConfigAttributeWrapper : BaseAttributeWrapper
	{
		public string ConfigKey { get; }
		public bool Randomized { get; }

		public ConfigAttributeWrapper(Plugin owner, string key, bool randomized, object instance, FieldInfo field) : base(owner, instance, field)
		{
			ConfigKey = key;
			Randomized = randomized;
		}
	}
}
