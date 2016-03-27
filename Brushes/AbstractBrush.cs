// ReSharper disable VirtualMemberNeverOverriden.Global

using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a basic brush.
    /// </summary>
    public abstract class AbstractBrush : IBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the calculation mode used for the rectangle/points used for color-selection in brushes.
        /// </summary>
        public BrushCalculationMode BrushCalculationMode { get; set; } = BrushCalculationMode.Relative;

        /// <summary>
        /// Gets or sets the overall percentage brightness of the brush.
        /// </summary>
        public float Brightness { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage opacity of the brush.
        /// </summary>
        public float Opacity { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractBrush"/> class.
        /// </summary>
        /// <param name="brightness">The overall percentage brightness of the brush. (default: 1f)</param>
        /// <param name="opacity">The overall percentage opacity of the brush. (default: 1f)</param>
        protected AbstractBrush(float brightness = 1f, float opacity = 1f)
        {
            this.Brightness = brightness;
            this.Opacity = opacity;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="point">The point from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        public abstract Color GetColorAtPoint(RectangleF rectangle, PointF point);

        /// <summary>
        /// Finalizes the color by appliing the overall brightness and opacity.<br/>
        /// This method should always be the last call of a <see cref="GetColorAtPoint" /> implementation.
        /// </summary>
        /// <param name="color">The color to finalize.</param>
        /// <returns>The finalized color.</returns>
        protected virtual Color FinalizeColor(Color color)
        {
            // Since we use HSV to calculate there is no way to make a color 'brighter' than 100%
            // Be carefull with the naming: Since we use HSV the correct term is 'value' but outside we call it 'brightness'
            // THIS IS NOT A HSB CALCULATION!!!
            float finalBrightness = color.GetHSVValue() * (Brightness < 0 ? 0 : (Brightness > 1f ? 1f : Brightness));
            byte finalAlpha = (byte)(color.A * (Opacity < 0 ? 0 : (Opacity > 1f ? 1f : Opacity)));
            return ColorHelper.ColorFromHSV(color.GetHue(), color.GetHSVSaturation(), finalBrightness, finalAlpha);
        }

        #endregion
    }
}
