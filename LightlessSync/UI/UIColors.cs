using System.Globalization;
using System.Numerics;

namespace LightlessSync.UI
{
    internal static class UIColors
    {
        private static readonly Dictionary<string, string> HexColors = new(StringComparer.OrdinalIgnoreCase)
        {
            { "LightlessPurple", "#ad8af5" },
            { "LightlessBlue", "#64c7e8" },
            { "PairBlue", "#4e98b1" },
            { "DimRed", "#bd0000" },
        };

        public static Vector4 Get(string name)
        {
            if (!HexColors.TryGetValue(name, out var hex))
                throw new ArgumentException($"Color '{name}' not found in UIColors.");

            return HexToRgba(hex);
        }

        public static Vector4 HexToRgba(string hexColor)
        {
            hexColor = hexColor.TrimStart('#');
            int r = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
            int a = hexColor.Length == 8 ? int.Parse(hexColor.Substring(6, 2), NumberStyles.HexNumber) : 255;
            return new Vector4(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}
