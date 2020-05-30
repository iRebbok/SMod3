using System;
using System.Collections.Generic;

namespace SMod3.Core.API
{
	public static class Extension
	{
		public static IReadOnlyList<ColorType> ColorsConsole { get; } = Array.AsReadOnly<ColorType>(new ColorType[10]
		{
			ColorType.GRAY,
			ColorType.DEFAULT, // Interpreted as gray like any other color absented in this array
			ColorType.BLUE,
			ColorType.RED,
			ColorType.CYAN,
			ColorType.YELLOW,
			ColorType.MAGENTA,
			ColorType.GREEN,
			ColorType.WHITE,
			ColorType.BLACK,
		});

		public static IReadOnlyList<ColorType> ColorsPlayerList { get; } = Array.AsReadOnly<ColorType>(new ColorType[22]
		{
			ColorType.DEFAULT,
			ColorType.PINK,
			ColorType.RED,
			ColorType.BROWN,
			ColorType.SILVER,
			ColorType.LIGHT_GREEN,
			ColorType.CRIMSON,
			ColorType.CYAN,
			ColorType.AQUA,
			ColorType.DEEP_PINK,
			ColorType.TOMATO,
			ColorType.YELLOW,
			ColorType.MAGENTA,
			ColorType.BLUE_GREEN,
			ColorType.ORANGE,
			ColorType.LIME,
			ColorType.GREEN,
			ColorType.CARMINE,
			ColorType.NICKEL,
			ColorType.MINT,
			ColorType.ARMY_GREEN,
			ColorType.PUMPKIN
		});

		public static IReadOnlyList<RoleType> RolePlayables { get; } = Array.AsReadOnly<RoleType>(new RoleType[18]
		{
			RoleType.SCP_049,
			RoleType.SCP_049_2,
			RoleType.SCP_079,
			RoleType.SCP_096,
			RoleType.SCP_106,
			RoleType.SCP_173,
			RoleType.SCP_457,
			RoleType.SCP_939_53,
			RoleType.SCP_939_89,
			RoleType.CLASSD,
			RoleType.CHAOS_INSURGENCY,
			RoleType.SCIENTIST,
			RoleType.FACILITY_GUARD,
			RoleType.NTF_CADET,
			RoleType.NTF_LIEUTENANT,
			RoleType.NTF_COMMANDER,
			RoleType.NTF_SCIENTIST,
			RoleType.TUTORIAL
		});

		public static IReadOnlyList<RoleType> RoleSCPs { get; } = Array.AsReadOnly<RoleType>(new RoleType[9]
		{
			RoleType.SCP_049,
			RoleType.SCP_049_2,
			RoleType.SCP_079,
			RoleType.SCP_096,
			RoleType.SCP_106,
			RoleType.SCP_173,
			RoleType.SCP_457,
			RoleType.SCP_939_53,
			RoleType.SCP_939_89
		});

		public static IReadOnlyList<RoleType> RoleBannableSCPs { get; } = Array.AsReadOnly<RoleType>(new RoleType[8]
		{
			RoleType.SCP_049,
			RoleType.SCP_079,
			RoleType.SCP_096,
			RoleType.SCP_106,
			RoleType.SCP_173,
			RoleType.SCP_457,
			RoleType.SCP_939_53,
			RoleType.SCP_939_89
		});

		public static IReadOnlyList<RoleType> RolePickableSCPs { get; } = Array.AsReadOnly<RoleType>(new RoleType[7]
		{
			RoleType.SCP_049,
			RoleType.SCP_079,
			RoleType.SCP_096,
			RoleType.SCP_106,
			RoleType.SCP_173,
			RoleType.SCP_939_53,
			RoleType.SCP_939_89
		});

		public static string ToString(this ColorType enumtype)
		{
			return enumtype.ToString().ToLowerInvariant();
		}

		public static bool TryGetColorType(string color, out ColorType result)
		{
			return Enum.TryParse<ColorType>(color, true, out result);
		}

		public static ColorType GetColorType(string color)
		{
			TryGetColorType(color, out ColorType result);
			return result;
		}

		public static ColorType GetColorType(string color, ColorType defaultColor)
		{
			if (TryGetColorType(color, out ColorType result))
			{
				defaultColor = result;
			}
			return defaultColor;
		}
	}
}
