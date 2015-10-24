/* 
 * ###################################################################################################################
 * #                                             Thanks to AterialDawn                                               #
 * #                                                                                                                 #
 * # Source: https://github.com/AterialDawn/CUEAudioVisualizer/blob/master/CUEAudioVisualizer/Utility.cs             #
 * #   Date: 23.10.2015                                                                                              #
 * # Commit: d415fd1dda2e7fd98391ac83c000d7e4a781558e                                                                #
 * #                                                                                                                 #
 * ###################################################################################################################
 * 
 * > Note < This code is refactored and all parts not used in this tutorial are removed.
 */

using System;

namespace Example_AudioAnalyzer_full.TakeAsIs
{
    public static class Utility
    {
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static float LinearInterpolate(float absMin, float absMax, float value)
        {
            return (value / (1f / (absMax - absMin))) + absMin;
        }
    }
}
