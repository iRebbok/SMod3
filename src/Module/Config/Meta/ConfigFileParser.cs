using SMod3.Core;
using SMod3.Module.EventSystem;
using SMod3.Module.EventSystem.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SMod3.Module.Config.Meta
{
    /// <summary>
    ///		Configuration parser what is used for reading the configuration.
    /// </summary>
    /// <remarks>
    ///		It does not use the game configuration parser functions that can throw exceptions,
    ///		so it gives it the ability to jump through the game versions and not be afraid of changes.
    /// </remarks>
    public sealed class ConfigFileParser : IConfigFile
    {
        #region Fields

        public static Regex ConfigLineRegex { get; } = new Regex(@"^(?:\s*)([^#:\s]+)(?:\s*):(?:\s*)(.*?)(?:\s*)$",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        public static readonly IReadOnlyCollection<string> trueWords = Array.AsReadOnly(new string[16]
        {
            "true",
            "t",
            "y",
            "yes",
            "enable",
            "enabled",
            "active",
            "activated",
            "sure",
            "yeah",
            "yea",
            "yep",
            "affirmative",
            "aye",
            "okay",
            "1"
        });

        public static readonly IReadOnlyCollection<string> falseWords = Array.AsReadOnly(new string[14]
        {
            "false",
            "f",
            "n",
            "no",
            "not",
            "disable",
            "disabled",
            "disactive",
            "disactivated",
            "nope",
            "nah",
            "negative",
            "nay",
            "0"
        });

        private static readonly Random random = new Random();

        private readonly Dictionary<string, string> configValues = new Dictionary<string, string>();

        #endregion

        #region Implemented

        public string ConfigPath { get; private set; }

        public bool? GetBoolValue(string key, bool? def = null, bool isRandomized = false)
        {
            return GetBool(key, def, isRandomized);
        }

        public bool GetBoolValue(string key, bool def, bool isRandomized = false)
        {
            return GetBool(key, def, isRandomized);
        }

        public byte? GetByteValue(string key, byte? def = null, bool isRandomized = false)
        {
            return GetByte(key, def, isRandomized);
        }

        public byte GetByteValue(string key, byte def, bool isRandomized = false)
        {
            return GetByte(key, def, isRandomized);
        }

        public decimal? GetDecimalValue(string key, decimal? def = null, bool isRandomized = false)
        {
            return GetDecimal(key, def, isRandomized);
        }

        public decimal GetDecimalValue(string key, decimal def, bool isRandomized = false)
        {
            return GetDecimal(key, def, isRandomized);
        }

        public double? GetDoubleValue(string key, double? def = null, bool isRandomized = false)
        {
            return GetDouble(key, def, isRandomized);
        }

        public double GetDoubleValue(string key, double def, bool isRandomized = false)
        {
            return GetDouble(key, def, isRandomized);
        }

        public short? GetInt16Value(string key, short? def = null, bool isRandomized = false)
        {
            return GetShort(key, def, isRandomized);
        }

        public short GetInt16Value(string key, short def, bool isRandomized = false)
        {
            return GetShort(key, def, isRandomized);
        }

        public int? GetInt32Value(string key, int? def = null, bool isRandomized = false)
        {
            return GetInt(key, def, isRandomized);
        }

        public int GetInt32Value(string key, int def, bool isRandomized = false)
        {
            return GetInt(key, def, isRandomized);
        }

        public long? GetInt64Value(string key, long? def = null, bool isRandomized = false)
        {
            return GetLong(key, def, isRandomized);
        }

        public long GetInt64Value(string key, long def, bool isRandomized = false)
        {
            return GetLong(key, def, isRandomized);
        }

        public sbyte? GetSByteValue(string key, sbyte? def = null, bool isRandomized = false)
        {
            return GetSByte(key, def, isRandomized);
        }

        public sbyte GetSByteValue(string key, sbyte def, bool isRandomized = false)
        {
            return GetSByte(key, def, isRandomized);
        }

        public float? GetSingleValue(string key, float? def = null, bool isRandomized = false)
        {
            return GetFloat(key, def, isRandomized);
        }

        public float GetSingleValue(string key, float def, bool isRandomized = false)
        {
            return GetFloat(key, def, isRandomized);
        }

        public ushort? GetUInt16Value(string key, ushort? def = null, bool isRandomized = false)
        {
            return GetUShort(key, def, isRandomized);
        }

        public ushort GetUInt16Value(string key, ushort def, bool isRandomized = false)
        {
            return GetUShort(key, def, isRandomized);
        }

        public uint? GetUInt32Value(string key, uint? def = null, bool isRandomized = false)
        {
            return GetUInt(key, def, isRandomized);
        }

        public uint GetUInt32Value(string key, uint def, bool isRandomized = false)
        {
            return GetUInt(key, def, isRandomized);
        }

        public ulong? GetUInt64Value(string key, ulong? def = null, bool isRandomized = false)
        {
            return GetULong(key, def, isRandomized);
        }

        public ulong GetUInt64Value(string key, ulong def, bool isRandomized = false)
        {
            return GetULong(key, def, isRandomized);
        }


        public string[] GetStringArrayValue(string key, string[] def, bool isRandomized = false)
        {
            return GetStringArray(key, def, isRandomized, true);
        }

        public string[] GetStringArrayValue(string key, bool isRandomized = false)
        {
            return GetStringArray(key, isRandomized, false, true);
        }

        public string GetRawValue(string key, string def = null, bool isRandomized = false)
        {
            return GetString(key, def, isRandomized);
        }

        #endregion

        public static ConfigFileParser Parser { get; } = new ConfigFileParser();

        private ConfigFileParser() { }

        /// <summary>
        ///		Sets the path to the configuration.
        ///		Called from outside.
        /// </summary>
        public void SetConfigPath(string path)
        {
            ConfigPath = path;
        }

        /// <summary>
        ///		Used for configuring the parser configuration,
        ///		the actual call reloads the configuration.
        /// </summary>
        /// <param name="rawData">
        ///		The configuration values are separated into a string,
        ///		this performs <see cref="File.ReadAllLines(string)"/>.
        /// </param>
        public void PopulateDictionary(string[] rawData)
        {
            //ServerMod.DebugLog("CONFIG_READER", "PopulateDictionary called...");
            configValues.Clear();
            foreach (string text in rawData)
            {
                Match match = ConfigLineRegex.Match(text);
                if (match == null || !match.Success)
                {
                    continue;
                }

                string key = match.Groups[1].Value.ToLower();
                string value = match.Groups[2].Value;

                if (configValues.ContainsKey(key))
                {
                    PluginManager.Manager.Logger.Raw($"Duplicate config key '{key}' with value '{value}' detected. Ignoring....");
                }
                else
                {
                    configValues.Add(key, value);
                }
            }


            bool packageConfigDebugMessages = GetBool("package_config_debug", false);
            bool sortConfigDebugMessages = GetBool("sort_config_debug", false);


            if (!packageConfigDebugMessages)
            {
                foreach (KeyValuePair<string, string> pair in (sortConfigDebugMessages ? configValues.OrderBy(keyValuePair => keyValuePair.Key).AsEnumerable() : configValues))
                {
                    PluginManager.Manager.Logger.Debug("CONFIG_READER", $"${pair.Key}: {pair.Value}");
                }

            }
            else
            {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<string, string> pair in (sortConfigDebugMessages ? configValues.OrderBy(keyValuePair => keyValuePair.Key).AsEnumerable() : configValues))
                {
                    builder.AppendLine($"{pair.Key} - {pair.Value}");
                }


                PluginManager.Manager.Logger.Debug("CONFIG_READER_LIST", builder.ToString());
                builder.Clear();
            }
        }

        /// <summary>
        ///		Used for announcing the configuration value as an event.
        /// </summary>
        /// <returns>
        ///		The set value if it matches the required type,
        ///		otherwise the default value.
        /// </returns>
        public static T HandleConfigSet<T>(string configKey, T sourceValue, T def)
        {
            SetConfigEvent ev = new SetConfigEvent(configKey, sourceValue, def);
            EventManager.Manager.HandleEvent<IEventHandlerSetConfig>(ev);
            if (ev.Value != null && ev.Value is T resultValue) return resultValue;
            return def;
        }

        /// <summary>
        ///		Used for randomizing the selection a.k.a. (c) RValue.
        /// </summary>
        /// <param name="input">
        ///		Parsed RValue with the standard type.
        /// </param>
        // todo: implement the regex
        public static string SetRandomValue(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string filterInput = input.Trim();

            if (filterInput.StartsWith("{") && filterInput.EndsWith("}") && filterInput.Length > 2)
            {
                filterInput = filterInput.Substring(1, input.Length - 2);

                // ServerMod.DebugLog("RANDOM_CONFIG_VALUE", "Entry: " + filterInput);

                string[] options = filterInput.Split('|');
                List<string> optionsList = new List<string>();
                List<int> weightsList = new List<int>();
                int weightSum = 0;

                foreach (string option in options)
                {
                    string[] parts = option.Split(new char[] { '%' }, 2);

                    int weight;
                    if (parts.Length > 1 && int.TryParse(parts[0].Trim(), out weight))
                    {
                        /*for (int i = 0; i < weight; i++)
						{
							optionsList.Add(parts[1]);
						}*/
                        string optionValue = parts[1].Trim();
                        int index = optionsList.IndexOf(optionValue);
                        //ServerMod.DebugLog("RANDOMCONFIGS","Index: " + index + " Option: " + optionValue);
                        if (index != -1)
                        {
                            weightsList[index] += weight;
                        }
                        else
                        {
                            weightsList.Add(weight);
                            optionsList.Add(optionValue);
                        }
                        weightSum += weight;
                    }
                    else
                    {
                        optionsList.Add(option);
                        weightsList.Add(1);
                        weightSum += 1;
                    }
                }

                if (optionsList.Count > 0)
                {
                    //ServerMod.DebugLog("RANDOMCONFIGS","Options count: " + optionsList.Count + " Weights count: " + weightsList.Count);
                    /*
					int chosenIndex = UnityEngine.Random.Range(0, optionsList.Count);
					// ServerMod.DebugLog("RANDOM_CONFIG_VALUE", "Value chosen: " + optionsList[chosenIndex]);
					return optionsList[chosenIndex];*/
                    int chosenWeight = random.Next(0, weightSum) /* UnityEngine.Random.Range(0, weightSum) */;
                    for (int i = 0; i < weightsList.Count; i++)
                    {
                        //ServerMod.DebugLog("RANDOMCONFIGS","Index: " + i);
                        //ServerMod.DebugLog("RANDOMCONFIGS","Option: " + optionsList[i] + " Weight: " + weightsList[i]);
                        if (chosenWeight < weightsList[i])
                        {
                            return optionsList[i];
                        }
                        else
                        {
                            chosenWeight -= weightsList[i];
                        }
                    }
                }
                else
                {
                    // ServerMod.DebugLog("RANDOM_CONFIG_VALUE", "No values found.");
                }
            }

            return input;
        }

        /// <summary>
        ///		Used for randomizing the list.
        /// </summary>
        public static string[] SetListRandomized(string[] list)
        {
            return list.Select(enrty => SetRandomValue(enrty)).ToArray();
        }

        /// <summary>
        ///		Checks the key content in the configuration.
        /// </summary>
        public bool Contains(string key)
        {
            return configValues.ContainsKey(key);
        }

        /// <summary>
        ///		Sets the value of a specific key.
        /// </summary>
        public void SetValue(string key, string val)
        {
            configValues[key] = val;
        }

        /// <summary>
        ///		Wrapper on the parent method.
        /// </summary>
        /// <remarks>
        ///		Don't use it to take nullable values,
        ///		this will cause you to not be able to apply the null value for configurations.
        /// </remarks>
        public string GetRawString(string key, string def = null, bool randomValue = false, bool replaceNewlines = false, bool isString = false)
        {
            return GetRawString(key, out _, def, randomValue, replaceNewlines, isString);
        }

        /// <summary>
        ///		Parent of all value retrievals from the configuration.
        /// </summary>
        /// <param name="replaceNewlines">
        ///		Used for applying a line break.
        /// </param>
        /// <param name="isString">
        ///		Defines the event call.
        /// </param>
        public string GetRawString(string key, out bool isNullable, string def = null, bool randomValue = false, bool replaceNewlines = false, bool isString = false)
        {
            isNullable = false;

            if (string.IsNullOrEmpty(key) || configValues == null) return def;

            string result;
            if (configValues.TryGetValue(key.ToLower(), out result) && result.Equals("default", StringComparison.InvariantCultureIgnoreCase)) result = def;
            else result = def;

            // It was more convenient to take nullable values
            if (result.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                isNullable = true;
                return null;
            }

            // ServerMod - Replace \n with Environment.NewLine char @ Dankrushen
            if (replaceNewlines && !string.IsNullOrEmpty(result))
            {
                result = result.Replace(@"\n", Environment.NewLine);
            }
            // End ServerMod - Replace \n with Environment.NewLine char @ Dankrushen


            if (isString) result = HandleConfigSet(key, result, def);

            if (randomValue) result = SetRandomValue(result);

            return result;
        }

        public string GetString(string key, string def = null, bool randomValue = false, bool replaceNewlines = true)
        {
            return GetRawString(key, def, randomValue, replaceNewlines, true);
        }

        public bool? GetBool(string key, bool? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            bool? result = def;
            if (trueWords.Any(w => sourceString.Equals(w, StringComparison.InvariantCultureIgnoreCase))) result = true;
            else if (falseWords.Any(w => sourceString.Equals(w, StringComparison.InvariantCultureIgnoreCase))) result = false;

            return HandleConfigSet(key, result, def);
        }

        public bool GetBool(string key, bool def = false, bool randomValue = false)
        {
            return GetBool(key, (bool?)def, randomValue) ?? def;
        }

        public sbyte? GetSByte(string key, sbyte? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (sbyte.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out sbyte result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public sbyte GetSByte(string key, sbyte def = 0, bool randomValue = false)
        {
            return GetSByte(key, (sbyte?)def, randomValue) ?? def;
        }

        public byte? GetByte(string key, byte? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (byte.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out byte result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public byte GetByte(string key, byte def = 0, bool randomValue = false)
        {
            return GetByte(key, (byte?)def, randomValue) ?? def;
        }

        public ushort? GetUShort(string key, ushort? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (ushort.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out ushort result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public ushort GetUShort(string key, ushort def = 0, bool randomValue = false)
        {
            return GetUShort(key, (ushort?)def, randomValue) ?? def;
        }

        public short? GetShort(string key, short? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (short.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out short result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public short GetShort(string key, short def = 0, bool randomValue = false)
        {
            return GetShort(key, (short?)def, randomValue) ?? def;
        }

        public uint? GetUInt(string key, uint? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (uint.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out uint result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public uint GetUInt(string key, uint def = 0U, bool randomValue = false)
        {
            return GetUInt(key, (uint?)def, randomValue) ?? def;
        }

        public int? GetInt(string key, int? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (int.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public int GetInt(string key, int def = 0, bool randomValue = false)
        {
            return GetInt(key, (int?)def, randomValue) ?? def;
        }

        public ulong? GetULong(string key, ulong? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (ulong.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out ulong result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public ulong GetULong(string key, ulong def = 0, bool randomValue = false)
        {
            return GetULong(key, (ulong?)def, randomValue) ?? def;
        }

        public long? GetLong(string key, long? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false);
            if (isNullable) return null;

            if (long.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out long result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public long GetLong(string key, long def = 0L, bool randomValue = false)
        {
            return GetLong(key, (long?)def, randomValue) ?? def;
        }

        public float? GetFloat(string key, float? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false)?.Replace(',', '.');
            if (isNullable) return null;

            if (float.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public float GetFloat(string key, float def = 0f, bool randomValue = false)
        {
            return GetFloat(key, (float?)def, randomValue) ?? def;
        }

        public double? GetDouble(string key, double? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false)?.Replace(',', '.');
            if (isNullable) return null;

            if (double.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public double GetDouble(string key, double def = 0D, bool randomValue = false)
        {
            return GetDouble(key, (double?)def, randomValue) ?? def;
        }

        public decimal? GetDecimal(string key, decimal? def, bool randomValue = false)
        {
            string sourceString = GetRawString(key, out bool isNullable, null, randomValue, false, false)?.Replace(',', '.');
            if (isNullable) return null;

            if (decimal.TryParse(sourceString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return HandleConfigSet(key, result, def);

            return def;
        }

        public decimal GetDecimal(string key, decimal def = 0M, bool randomValue = false)
        {
            return GetDecimal(key, (decimal?)def, randomValue) ?? def;
        }

        public string[] GetStringArray(string key, string[] def, bool isRandomized = false, bool trim = false, bool isString = false)
        {
            string sourceString = GetRawString(key, null);
            if (string.IsNullOrEmpty(sourceString))
                return isString ? HandleConfigSet(key, def, def) : def;

            string[] array = sourceString.Split(',');
            if (trim)
                array = array.Select(source => source.Trim()).ToArray();

            array = (array.Length <= 0) ? def : array; // Allow randomization in the default values

            if (isRandomized) array = SetListRandomized(array);

            return isString ? HandleConfigSet(key, array, def) : array;
        }

        public string[] GetStringArray(string key, bool isRandomized = false, bool trim = false, bool isString = true)
        {
            return GetStringArray(key, Array.Empty<string>(), isRandomized, trim, isString);
        }

        public Dictionary<string, string> GetStringDictionary(string key, Dictionary<string, string> def, bool isRandomized = false, char splitChar = ':', bool isString = false)
        {
            string[] sourceArray = GetStringArray(key, isRandomized, false);

            Dictionary<string, string> toReturnDictionary = new Dictionary<string, string>();
            if (sourceArray.Length == 0) return isString ? HandleConfigSet(key, def, def) : def;

            char[] splitChars = new char[1] { splitChar };
            for (int z = 0; z < sourceArray.Length; z++)
            {
                string sourceString = sourceArray[z];
                string[] splittedArray = sourceString.Split(splitChars, 2);
                if (splittedArray.Length == 2) toReturnDictionary.Add(splittedArray[0], splittedArray[1]);
            }

            if (toReturnDictionary.Keys.Count == 0) toReturnDictionary = def;

            return isString ? HandleConfigSet(key, toReturnDictionary, def) : toReturnDictionary;
        }

        public Dictionary<string, string> GetStringDictionary(string key, bool isRandomized = false, char splitChar = ':', bool isString = true)
        {
            return GetStringDictionary(key, new Dictionary<string, string>(), isRandomized, splitChar, isString);
        }

        public Dictionary<int, int> GetIntDictionary(string key, Dictionary<int, int> def, bool isRandomized = false, char splitChar = ':')
        {
            Dictionary<string, string> sourceDictionary = GetStringDictionary(key, isRandomized, splitChar);

            Dictionary<int, int> toReturnDictionary = new Dictionary<int, int>();
            if (sourceDictionary.Keys.Count == 0) return HandleConfigSet(key, def, def);

            foreach (KeyValuePair<string, string> keyType in sourceDictionary)
            {
                if (int.TryParse(keyType.Key, NumberStyles.Any, CultureInfo.InvariantCulture, out int dictKey) &&
                    int.TryParse(keyType.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int dickValue))
                    toReturnDictionary.Add(dictKey, dickValue);
            }

            if (toReturnDictionary.Keys.Count == 0) toReturnDictionary = def;

            return HandleConfigSet(key, toReturnDictionary, def);
        }

        public Dictionary<int, int> GetIntDictionary(string key, bool isRandomized = false, char splitChar = ':')
        {
            return GetIntDictionary(key, new Dictionary<int, int>(), isRandomized, splitChar);
        }
    }
}
