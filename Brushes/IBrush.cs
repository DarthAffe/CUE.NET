// ReSharper disable UnusedMemberInSuper.Global

using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a basic brush.
    /// </summary>
    public interface IBrush
    {
        /// <summary>
        /// Gets or sets the calculation mode used for the rectangle/points used for color-selection in brushes.
        /// </summary>
        BrushCalculationMode BrushCalculationMode { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage brightness of the brush.
        /// </summary>
        float Brightness { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage opacity of the brush.
        /// </summary>
        float Opacity { get; set; }

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="point">The point from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        Color GetColorAtPoint(RectangleF rectangle, PointF point);
    }
}