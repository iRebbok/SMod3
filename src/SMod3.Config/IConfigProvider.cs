using System.Collections.Generic;

namespace SMod3.Module.Config
{
    public interface IConfigProvider
    {
        /// <summary>
        ///		Gets the config path.
        /// </summary>
        string ConfigPath { get; }

        sbyte? GetSByteValue(string key, sbyte? def = null);
        sbyte GetSByteValue(string key, sbyte def);
        byte? GetByteValue(string key, byte? def = null);
        byte GetByteValue(string key, byte def);
        ushort? GetUInt16Value(string key, ushort? def = null);
        ushort GetUInt16Value(string key, ushort def);
        short? GetInt16Value(string key, short? def = null);
        short GetInt16Value(string key, short def);
        uint? GetUInt32Value(string key, uint? def = null);
        uint GetUInt32Value(string key, uint def);
        int? GetInt32Value(string key, int? def = null);
        int GetInt32Value(string key, int def);
        ulong? GetUInt64Value(string key, ulong? def = null);
        ulong GetUInt64Value(string key, ulong def);
        long? GetInt64Value(string key, long? def = null);
        long GetInt64Value(string key, long def);
        float? GetSingleValue(string key, float? def = null);
        float GetSingleValue(string key, float def);
        double? GetDoubleValue(string key, double? def = null);
        double GetDoubleValue(string key, double def);
        decimal? GetDecimalValue(string key, decimal? def = null);
        decimal GetDecimalValue(string key, decimal def);
        bool? GetBoolValue(string key, bool? def = null);
        bool GetBoolValue(string key, bool def);

        string GetRawValue(string key, string? def = null);

        void GetStringCollection(string key, ICollection<string> collection);

        string[]? GetStringArrayValue(string key, string[]? def = null);
    }
}
