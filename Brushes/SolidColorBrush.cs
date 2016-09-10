// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;

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
        public Color Color { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidColorBrush"/> class.
        /// </summary>
        /// <param name="color">The color drawn by the brush.</param>
        public SolidColorBrush(Color color)
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
        protected override Color GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
        {
            return Color;
        }

        #endregion
    }
}