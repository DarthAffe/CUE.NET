// ReSharper disable MemberCanBePrivate.Global

using System.Drawing;

namespace CUE.NET.Helper
{
    public static class ColorHelper
    {
        public static float GetFloatA(this Color color)
        {
            return color.A / 255f;
        }

        public static float GetFloatR(this Color color)
        {
            return color.R / 255f;
        }

        public static float GetFloatG(this Color color)
        {
            return color.G / 255f;
        }

        public static float GetFloatB(this Color color)
        {
            return color.B / 255f;
        }

        public static Color Blend(this Color bg, Color fg)
        {
            if (fg.A == 255)
                return fg;

            if (fg.A == 0)
                return bg;

            float resultA = (1 - (1 - fg.GetFloatA()) * (1 - bg.GetFloatA()));
            float resultR = (fg.GetFloatR() * fg.GetFloatA() / resultA + bg.GetFloatR() * bg.GetFloatA() * (1 - fg.GetFloatA()) / resultA);
            float resultG = (fg.GetFloatG() * fg.GetFloatA() / resultA + bg.GetFloatG() * bg.GetFloatA() * (1 - fg.GetFloatA()) / resultA);
            float resultB = (fg.GetFloatB() * fg.GetFloatA() / resultA + bg.GetFloatB() * bg.GetFloatA() * (1 - fg.GetFloatA()) / resultA);
            return CreateColorFromFloat(resultA, resultR, resultG, resultB);
        }

        public static Color CreateColorFromFloat(float a, float r, float g, float b)
        {
            return Color.FromArgb((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }
}
