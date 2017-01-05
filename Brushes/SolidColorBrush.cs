// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;
using CUE.NET.Devices.Generic;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing only a single color.
    /// </summary>
    public class SolidColorBrush : AbstractBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the color drawn by the brush.
        /// </summary>
        public CorsairColor Color { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidColorBrush"/> class.
        /// </summary>
        /// <param name="color">The color drawn by the brush.</param>
        public SolidColorBrush(CorsairColor color)
        {
            this.Color = color;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
        {
            return Color;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a <see cref="Color" /> to a <see cref="SolidColorBrush" />.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        public static explicit operator SolidColorBrush(Color color)
        {
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush" /> to a <see cref="Color" />.
        /// </summary>
        /// <param name="brush">The <see cref="Color"/> to convert.</param>
        public static implicit operator Color(SolidColorBrush brush)
        {
            return brush.Color;
        }

        /// <summary>
        /// Converts a <see cref="CorsairColor" /> to a <see cref="SolidColorBrush" />.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        public static explicit operator SolidColorBrush(CorsairColor color)
        {
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush" /> to a <see cref="CorsairColor" />.
        /// </summary>
        /// <param name="brush">The <see cref="Color"/> to convert.</param>
        public static implicit operator CorsairColor(SolidColorBrush brush)
        {
            return brush.Color;
        }

        #endregion
    }
}