// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Drawing;

namespace CUE.NET.Helper
{
    public static class ColorHelper
    {
        #region byte/float conversion

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

        public static Color CreateColorFromFloat(float a, float r, float g, float b)
        {
            return Color.FromArgb(GetIntColorFromFloat(a), GetIntColorFromFloat(r), GetIntColorFromFloat(g), GetIntColorFromFloat(b));
        }

        private static byte GetIntColorFromFloat(float f)
        {
            // ReSharper disable once RedundantCast - never trust this ...
            float calcF = (float)Math.Max(0f, Math.Min(1f, f));
            return (byte)(calcF.Equals(1f) ? 255 : calcF * 256f);
        }

        #endregion

        #region Blending

        public static Color Blend(this Color bg, Color fg)
        {
            if (fg.A == 255)
                return fg;

            if (fg.A == 0)
                return bg;

            float resultA = (1f - (1f - fg.GetFloatA()) * (1f - bg.GetFloatA()));
            float resultR = (fg.GetFloatR() * fg.GetFloatA() / resultA + bg.GetFloatR() * bg.GetFloatA() * (1f - fg.GetFloatA()) / resultA);
            float resultG = (fg.GetFloatG() * fg.GetFloatA() / resultA + bg.GetFloatG() * bg.GetFloatA() * (1f - fg.GetFloatA()) / resultA);
            float resultB = (fg.GetFloatB() * fg.GetFloatA() / resultA + bg.GetFloatB() * bg.GetFloatA() * (1f - fg.GetFloatA()) / resultA);
            return CreateColorFromFloat(resultA, resultR, resultG, resultB);
        }

        #endregion

        #region RGB/HSV conversion
        // https://en.wikipedia.org/wiki/HSL_and_HSV

        public static float GetHSVSaturation(this Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            // ReSharper disable RedundantCast - never trust this ...
            return (max == 0) ? 0 : 1f - ((float)min / (float)max);
            // ReSharper restore RedundantCast
        }

        public static float GetHSVValue(this Color color)
        {
            return Math.Max(color.R, Math.Max(color.G, color.B)) / 255f;
        }

        // Based on http://stackoverflow.com/questions/3018313/algorithm-to-convert-rgb-to-hsv-and-hsv-to-rgb-in-range-0-255-for-both/6930407#6930407 as of 27.09.2015
        public static Color ColorFromHSV(float hue, float saturation, float value, byte alpha = 255)
        {
            if (saturation <= 0.0)
            {
                int val = GetIntColorFromFloat(value);
                return Color.FromArgb(alpha, val, val, val);
            }

            float hh = (hue % 360f) / 60f;
            int i = (int)hh;
            float ff = hh - i;
            float p = value * (1f - saturation);
            float q = value * (1f - (saturation * ff));
            float t = value * (1f - (saturation * (1f - ff)));

            switch (i)
            {
                case 0:
                    return Color.FromArgb(alpha, GetIntColorFromFloat(value), GetIntColorFromFloat(t), GetIntColorFromFloat(p));
                case 1:
                    return Color.FromArgb(alpha, GetIntColorFromFloat(q), GetIntColorFromFloat(value), GetIntColorFromFloat(p));
                case 2:
                    return Color.FromArgb(alpha, GetIntColorFromFloat(p), GetIntColorFromFloat(value), GetIntColorFromFloat(t));
                case 3:
                    return Color.FromArgb(alpha, GetIntColorFromFloat(p), GetIntColorFromFloat(q), GetIntColorFromFloat(value));
                case 4:
                    return Color.FromArgb(alpha, GetIntColorFromFloat(t), GetIntColorFromFloat(p), GetIntColorFromFloat(value));
                default:
                    return Color.FromArgb(alpha, GetIntColorFromFloat(value), GetIntColorFromFloat(p), GetIntColorFromFloat(q));
            }
        }

        #endregion
    }
}
