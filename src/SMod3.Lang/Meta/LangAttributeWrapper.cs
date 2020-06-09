using SMod3.Core;
using SMod3.Module.Attributes.Meta;
using System.Reflection;

namespace SMod3.Module.Lang.Meta
{
    public sealed class LangAttributeWrapper : BaseAttributeWrapper
    {
        public string Key { get; }

        public string Filename { get; }

        public LangAttributeWrapper(Plugin owner, string key, string file, object instance, FieldInfo field) : base(owner, instance, field)
        {
            Key = key;
            Filename = file;
        }
    }
}
