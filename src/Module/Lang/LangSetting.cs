using System;

namespace SMod3.Module.Lang
{
    public class LangSetting
    {
        public string Key { get; }
        public string Default { get; }
        public string Filename { get; }
        public string Value { get; internal set; }

        public LangSetting(string key, string defaultText, string filename = null)
        {
            Key = (key ?? throw new ArgumentNullException(nameof(key))).ToLowerInvariant();
            Default = defaultText;
            Filename = filename;
        }
    }
}
