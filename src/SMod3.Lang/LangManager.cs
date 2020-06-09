using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SMod3.Core;
using SMod3.Module.Attributes;
using SMod3.Module.Attributes.Meta;
using SMod3.Module.Lang.Attributes;
using SMod3.Module.Lang.Meta;

namespace SMod3.Module.Lang
{
    public sealed class LangManager : BaseAttributeManager
    {
        public static LangManager Manager { get; } = new LangManager();

        private LangManager() { Reload(); }

        #region Fields

        private readonly Dictionary<Plugin, HashSet<LangSetting>> langSettings = new Dictionary<Plugin, HashSet<LangSetting>>();
        private readonly Dictionary<Plugin, HashSet<LangAttributeWrapper>> langFields = new Dictionary<Plugin, HashSet<LangAttributeWrapper>>();

        private readonly Dictionary<string, string> langValues = new Dictionary<string, string>();
        private readonly Dictionary<string, LangContainerWrapper> langContainers = new Dictionary<string, LangContainerWrapper>();

        #endregion

        public override string LoggingTag => "LANG_MANAGER";

        private string langPath;
        public string LangPath
        {
            get
            {
                if (string.IsNullOrEmpty(langPath))
                {
                    langPath = Path.Combine(PluginManager.Manager.GamePath, "sm_translations");
                }
                return langPath;
            }
        }

        public void Reload()
        {
            langValues.Clear();

            if (!Directory.Exists(LangPath))
            {
                Directory.CreateDirectory(LangPath);
                Debug($"No translations for path: {LangPath}");
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(LangPath);
            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {
                using (StreamReader stream = file.OpenText())
                {
                    string line;
                    while ((line = stream.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line) ||
                            line.StartsWith("#", StringComparison.InvariantCultureIgnoreCase) ||
                            line.StartsWith("//", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        string[] keyvalue = line.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries).Select(@string => @string.Trim()).ToArray();

                        //if (keyvalue.Length != 2 || keyvalue[0].Contains(" "))
                        //{
                        //    PluginManager.Manager.Logger.Error("LANG_MANAGER", string.Format("Cant load keyvalue from '{0}' = '{1}'", file.Name, line));
                        //    continue;
                        //}

                        string key = keyvalue[0].ToLowerInvariant();
                        string value = keyvalue[1];

                        //if (langValues.ContainsKey(key))
                        //{
                        //    PluginManager.Manager.Logger.Error("LANG_MANAGER", string.Format("Duplicate key detected '{0}' at '{1}' file", key, file.Name));
                        //    continue;
                        //}

                        langValues.Add(key, value);
                    }
                }
            }

            foreach (var container in langContainers.Values)
            {
                container.LangSetting.Value = GetTranslation(container.LangSetting.Key);
            }
        }

        public bool IsRegistered(Plugin plugin, string key)
        {
            if (langContainers.TryGetValue(key, out var container))
            {
                return container.Owner == plugin;
            }
            return false;
        }

        public bool IsRegisteredKey(string key)
        {
            return langContainers.ContainsKey(key);
        }

        public bool RegisterTranslation(Plugin plugin, LangSetting setting)
        {
            return RegisterTranslation(plugin, setting, out _);
        }

        public bool RegisterTranslation(Plugin plugin, LangSetting setting, out LangContainerWrapper langContainer)
        {
            langContainer = null;
            //if (plugin == null || setting == null) return false;

            //string key = setting.Key;
            //if (IsRegisteredKey(key))
            //{
            //    Warn($"{plugin} is trying to register a duplicate setting: {key}");
            //    return false;
            //}

            //if (!langSettings.TryGetValue(plugin, out var settings))
            //{
            //    settings = new HashSet<LangSetting>();
            //    langSettings.Add(plugin, settings);
            //}

            ////string filename = setting.Filename ?? plugin.Definer.LangFile;
            ////if (string.IsNullOrEmpty(filename))
            ////{
            ////    Warn($"{plugin} is trying to register with an unknown filename");
            ////    return false;
            ////}

            //langContainer = new LangContainerWrapper(plugin);
            //langContainers[key] = langContainer;
            //langContainer.LangSetting = setting;


            //if (langValues.TryGetValue(key, out var lang))
            //{
            //    Debug($"{key} exists in translation files.");
            //    setting.Value = lang;
            //    return true;
            //}

            //langValues.Add(key, setting.Default);
            //using (StreamWriter stream = new StreamWriter(Path.Combine(LangPath, $"{filename}.txt"), true, Encoding.UTF8))
            //{
            //    stream.WriteLine($"{setting.Key}: {setting.Default}");
            //}
            return true;
        }

        public string GetTranslation(string key)
        {
            if (langContainers.TryGetValue(key, out var container))
            {
                return container.LangSetting.Value;
            }
            return null;
        }

        public LangContainerWrapper GetTranslationAsContainer(string key)
        {
            return langContainers[key];
        }

        public string GetTranslationAsRaw(string key)
        {
            return langValues[key];
        }

        public override void Dispose(Plugin owner)
        {
            if (!langSettings.TryGetValue(owner, out var settings)) return;
            foreach (var setting in settings) langContainers.Remove(setting.Key);
            langSettings.Remove(owner);
            langFields.Remove(owner);
        }

        public override void RegisterAttributes<TInstance>(Plugin plugin, TInstance instance)
        {
            if (plugin == null || instance == null) return;


            var attributes = AttributeManager.Manager.PullAttributes<LangOptionAttribute, TInstance>(instance);
            //foreach (var pair in attributes)
            //{
            //    var key = pair.Key.Key ?? PluginManager.ToLowerSnakeCase(pair.Value.Name, true);
            //    var file = pair.Key.Filename ?? plugin.Definer.LangFile;

            //    if (string.IsNullOrEmpty(file))
            //    {
            //        Error($"{plugin} is trying to register attrbites without {nameof(PluginInfoAttribute.LangFile)}");
            //        return;
            //    }

            //    if (pair.Value.FieldType != typeof(string))
            //    {
            //        Error($"{plugin} is trying to register attribute lang {pair.Value.Name}, but the type ({pair.Value.FieldType}) is not a {nameof(String)}.");
            //        continue;
            //    }

            //    if (!RegisterTranslation(plugin, new LangSetting(key, pair.Value.GetValue(instance) as string, file), out var container))
            //    {
            //        Error($"Unable to register attribute translation {pair.Value.Name} from {plugin}.");
            //        continue;
            //    }

            //    var attributeWrapper = new LangAttributeWrapper(plugin, key, file, instance, pair.Value);
            //    container.LangAttribute = attributeWrapper;

            //    if (!langFields.TryGetValue(plugin, out var fieldWrappers))
            //    {
            //        fieldWrappers = new HashSet<LangAttributeWrapper>();
            //        langFields.Add(plugin, fieldWrappers);
            //    }
            //    fieldWrappers.Add(attributeWrapper);
            //}
        }

        public override void RefreshAttributes(Plugin plugin)
        {
            if (!langFields.TryGetValue(plugin, out var fields)) return;
            foreach (var field in fields)
            {
                var value = GetTranslation(field.Key);
                if (value != null) field.Field.SetValue(field.Instance, value);
            }
        }
    }
}
