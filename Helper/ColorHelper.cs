// ReSharper disable MemberCanBePrivate.Global

using System;
using CUE.NET.ColorCorrection;
using CUE.NET.Devices.Generic;

namespace CUE.NET.Helper
{
    /// <summary>
    /// Offers some extensions and helper-methods for color related things.
    /// </summary>
    public static class ColorHelper
    {
        #region byte/float conversion

        /// <summary>
        /// Converts the alpha-value of the <see cref="CorsairColor"/> to a float value in the range [0..1].
        /// </summary>
        /// <param name="color">The color to take the alpha-value from.</param>
        /// <returns>The float-value in the range of [0..1]</returns>
        public static float GetFloatA(this CorsairColor color)
        {
            return color.A / 255f;
        }

        /// <summary>
        /// Converts the red-value of the <see cref="CorsairColor"/> to a float value in the range [0..1].
        /// </summary>
        /// <param name="color">The color to take the red-value from.</param>
        /// <returns>The float-value in the range of [0..1]</returns>
        public static float GetFloatR(this CorsairColor color)
        {
            return color.R / 255f;
        }

        /// <summary>
        /// Converts the green-value of the <see cref="CorsairColor"/> to a float value in the range [0..1].
        /// </summary>
        /// <param name="color">The color to take the green-value from.</param>
        /// <returns>The float-value in the range of [0..1]</returns>
        public static float GetFloatG(this CorsairColor color)
        {
            return color.G / 255f;
        }

        /// <summary>
        /// Converts the blue-value of the <see cref="CorsairColor"/> to a float value in the range [0..1].
        /// </summary>
        /// <param name="color">The color to take the blue-value from.</param>
        /// <returns>The float-value in the range of [0..1]</returns>
        public static float GetFloatB(this CorsairColor color)
        {
            return color.B / 255f;
        }

        /// <summary>
        /// Creates a <see cref="CorsairColor"/> object from the respective rgb-float-values in the range [0..1].
        /// </summary>
        /// <param name="a">The alpha-value in the range [0..1].</param>
        /// <param name="r">The red-value in the range [0..1].</param>
        /// <param name="g">The green-value in the range [0..1].</param>
        /// <param name="b">The blue-value in the range [0..1].</param>
        /// <returns>The color-object created representing the given values.</returns>
        public static CorsairColor CreateColorFromFloat(float a, float r, float g, float b)
        {
            return new CorsairColor(GetIntColorFromFloat(a), GetIntColorFromFloat(r), GetIntColorFromFloat(g), GetIntColorFromFloat(b));
        }

        /// <summary>
        /// Converts the given float-value to a integer-color in the range [0..255].
        /// </summary>
        /// <param name="f">The float color-value</param>
        /// <returns>The integer-value int the range [0..255].</returns>
        public static byte GetIntColorFromFloat(float f)
        {
            // ReSharper disable once RedundantCast - never trust this ...
            float calcF = (float)Math.Max(0f, Math.Min(1f, f));
            return (byte)(calcF.Equals(1f) ? 255 : calcF * 256f);
        }

        #endregion

        #region Blending

        /// <summary>
        /// Blends two colors.
        /// </summary>
        /// <param name="bg">The background-color.</param>
        /// <param name="fg">The foreground-color</param>
        /// <returns>The resulting color.</returns>
        public static CorsairColor Blend(this CorsairColor bg, CorsairColor fg)
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

        /// <summary>
        /// Gets the hue-value (HSV-color space) of the color.
        /// </summary>
        /// <param name="color">The color to take the hue from.</param>
        /// <returns>The hue-value (HSV-color space) of the color.</returns>
        public static float GetHSVHue(this CorsairColor color)
        {
            if (color.R == color.G && color.G == color.B) return 0.0f;

            float min = Math.Min(Math.Min(color.R, color.G), color.B);
            float max = Math.Max(Math.Max(color.R, color.G), color.B);

            float hue = 0f;
            if (Math.Abs(max - color.R) < float.Epsilon) // r is max
                hue = (color.G - color.B) / (max - min);
            else if (Math.Abs(max - color.G) < float.Epsilon) // g is max
                hue = 2f + (color.B - color.R) / (max - min);
            else // b is max
                hue = 4f + (color.R - color.G) / (max - min);

            hue = hue * 60f;
            if (hue < 0f)
                hue += 360f;

            return hue;
        }

        /// <summary>
        /// Gets the saturation-value (HSV-color space) of the color.
        /// </summary>
        /// <param name="color">The color to take the saturation from.</param>
        /// <returns>The saturation-value (HSV-color space) of the color.</returns>
        public static float GetHSVSaturation(this CorsairColor color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            // ReSharper disable RedundantCast - never trust this ...
            return (max == 0) ? 0 : 1f - ((float)min / (float)max);
            // ReSharper restore RedundantCast
        }

        /// <summary>
        /// Gets the value-value (HSV-color space) of the color.
        /// </summary>
        /// <param name="color">The color to take the value from.</param>
        /// <returns>The value-value (HSV-color space) of the color.</returns>
        public static float GetHSVValue(this CorsairColor color)
        {
            return Math.Max(color.R, Math.Max(color.G, color.B)) / 255f;
        }

        // Based on http://stackoverflow.com/questions/3018313/algorithm-to-convert-rgb-to-hsv-and-hsv-to-rgb-in-range-0-255-for-both/6930407#6930407 as of 27.09.2015
        /// <summary>
        /// Creates a <see cref="CorsairColor"/> object from the respective hsv-float-values in the range [0..1].
        /// </summary>
        /// <param name="hue">The hue of the color.</param>
        /// <param name="saturation">The saturation of the color.</param>
        /// <param name="value">The value of the color.</param>
        /// <param name="alpha">The alpha of the color.</param>
        /// <returns>The color-object created representing the given values.</returns>
        public static CorsairColor ColorFromHSV(float hue, float saturation, float value, byte alpha = 255)
        {
            if (saturation <= 0.0)
            {
                byte val = GetIntColorFromFloat(value);
                return new CorsairColor(alpha, val, val, val);
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
                    return new CorsairColor(alpha, GetIntColorFromFloat(value), GetIntColorFromFloat(t), GetIntColorFromFloat(p));
                case 1:
                    return new CorsairColor(alpha, GetIntColorFromFloat(q), GetIntColorFromFloat(value), GetIntColorFromFloat(p));
                case 2:
                    return new CorsairColor(alpha, GetIntColorFromFloat(p), GetIntColorFromFloat(value), GetIntColorFromFloat(t));
                case 3:
                    return new CorsairColor(alpha, GetIntColorFromFloat(p), GetIntColorFromFloat(q), GetIntColorFromFloat(value));
                case 4:
                    return new CorsairColor(alpha, GetIntColorFromFloat(t), GetIntColorFromFloat(p), GetIntColorFromFloat(value));
                default:
                    return new CorsairColor(alpha, GetIntColorFromFloat(value), GetIntColorFromFloat(p), GetIntColorFromFloat(q));
            }
        }

        #endregion
    }
}
