// ReSharper disable UnusedMember.Global

using System;
using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Brushes
{
    //TODO DarthAffe 30.09.2015: Like this the brush seems kinda useless. Think about making it cool.

    /// <summary>
    /// Represents a brush drawing random colors.
    /// </summary>
    public class RandomColorBrush : AbstractBrush
    {
        #region Properties & Fields

        protected override IBrush EffectTarget => this;

        private Random _random = new Random();

        #endregion

        #region Methods

        /// <summary>
        /// Gets a random color.
        /// </summary>
        /// <param name="rectangle">This value isn't used.</param>
        /// <param name="point">This value isn't used.</param>
        /// <returns>A random color.</returns>
        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            return FinalizeColor(ColorHelper.ColorFromHSV((float)_random.NextDouble() * 360f, 1, 1));
        }

        #endregion
    }
}
