// ReSharper disable MemberCanBePrivate.Global

using System;
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
            return Color.FromArgb(GetIntColorFromFloat(a), GetIntColorFromFloat(r), GetIntColorFromFloat(g), GetIntColorFromFloat(b));
        }

        private static byte GetIntColorFromFloat(float f)
        {
            float calcF = (float)Math.Max(0.0, Math.Min(1.0, f));
            return (byte)(calcF == 1.0 ? 255 : calcF * 256.0);
        }
    }
}
