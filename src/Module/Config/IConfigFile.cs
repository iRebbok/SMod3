namespace SMod3.Module.Config
{
	public interface IConfigFile
	{
		/// <summary>
		///		Gets the config path.
		///		<para>
		///			Like /gamefolder/config_gameplay.txt
		///		</para>
		/// </summary>
		string ConfigPath { get; }

		sbyte? GetSByteValue(string key, sbyte? def = null, bool isRandomized = false);
		sbyte GetSByteValue(string key, sbyte def, bool isRandomized = false);
		byte? GetByteValue(string key, byte? def = null, bool isRandomized = false);
		byte GetByteValue(string key, byte def, bool isRandomized = false);
		ushort? GetUInt16Value(string key, ushort? def = null, bool isRandomized = false);
		ushort GetUInt16Value(string key, ushort def, bool isRandomized = false);
		short? GetInt16Value(string key, short? def = null, bool isRandomized = false);
		short GetInt16Value(string key, short def, bool isRandomized = false);
		uint? GetUInt32Value(string key, uint? def = null, bool isRandomized = false);
		uint GetUInt32Value(string key, uint def, bool isRandomized = false);
		int? GetInt32Value(string key, int? def = null, bool isRandomized = false);
		int GetInt32Value(string key, int def, bool isRandomized = false);
		ulong? GetUInt64Value(string key, ulong? def = null, bool isRandomized = false);
		ulong GetUInt64Value(string key, ulong def, bool isRandomized = false);
		long? GetInt64Value(string key, long? def = null, bool isRandomized = false);
		long GetInt64Value(string key, long def, bool isRandomized = false);
		float? GetSingleValue(string key, float? def = null, bool isRandomized = false);
		float GetSingleValue(string key, float def, bool isRandomized = false);
		double? GetDoubleValue(string key, double? def = null, bool isRandomized = false);
		double GetDoubleValue(string key, double def, bool isRandomized = false);
		decimal? GetDecimalValue(string key, decimal? def = null, bool isRandomized = false);
		decimal GetDecimalValue(string key, decimal def, bool isRandomized = false);
		bool? GetBoolValue(string key, bool? def = null, bool isRandomized = false);
		bool GetBoolValue(string key, bool def, bool isRandomized = false);
		string GetRawValue(string key, string def = null, bool isRandomized = false);

		string[] GetStringArrayValue(string key, bool isRandomized = false);
		string[] GetStringArrayValue(string key, string[] def, bool isRandomized = false);
	}
}
