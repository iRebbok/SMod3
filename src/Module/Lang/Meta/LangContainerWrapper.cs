using SMod3.Core;
using SMod3.Core.Meta;

namespace SMod3.Module.Lang.Meta
{
	public sealed class LangContainerWrapper : BaseWrapper
	{
		public LangSetting LangSetting { get; internal set; }

		public LangAttributeWrapper LangAttribute { get; internal set; }

		public LangContainerWrapper(Plugin owner) : base(owner) { }
	}
}
