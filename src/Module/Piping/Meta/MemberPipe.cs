using System;
using System.Reflection;

using SMod3.Core;

namespace SMod3.Module.Piping.Meta
{
	public abstract class MemberPipe
	{
		protected readonly Plugin instance;
		public Plugin Source { get; }
		public string Name { get; }
		public Type Type { get; protected set; }

		protected MemberPipe(Plugin source, MemberInfo info, bool @static)
		{
			Source = source;
			Name = info.Name;
			instance = @static ? null : source;
		}
	}
}
