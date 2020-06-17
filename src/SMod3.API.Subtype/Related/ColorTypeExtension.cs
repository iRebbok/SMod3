using System;
using System.Collections.Generic;

namespace SMod3.API
{
    public static partial class Extension
    {
        /// <summary>
        ///     Available colors in console.
        /// </summary>
        public static IReadOnlyCollection<ColorType> ColorsConsole { get; } = Array.AsReadOnly<ColorType>(new ColorType[10]
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

        /// <summary>
        ///     Available colors in the player list.
        /// </summary>
        public static IReadOnlyCollection<ColorType> ColorsPlayerList { get; } = Array.AsReadOnly<ColorType>(new ColorType[22]
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

        /// <summary>
        ///     Override for lowercase return.
        /// </summary>
        public static string ToString(this ColorType enumtype)
        {
            return enumtype.ToString().ToLowerInvariant();
        }

        public static bool TryGetColorType(string color, out ColorType result)
        {
            return Enum.TryParse(color, true, out result);
        }

        public static ColorType GetColorType(string color)
        {
            TryGetColorType(color, out ColorType result);
            return result;
        }

        public static ColorType GetColorType(string color, ColorType defaultColor)
        {
            if (TryGetColorType(color, out ColorType result))
                defaultColor = result;
            return defaultColor;
        }
    }
}
