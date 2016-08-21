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

        protected override IBrush EffectTarget => this;

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
        /// Returns the <see cref="Color" /> of the brush.
        /// </summary>
        /// <param name="rectangle">This value isn't used.</param>
        /// <param name="point">This value isn't used.</param>
        /// <returns>The <see cref="Color" /> of the brush.</returns>
        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            return FinalizeColor(Color);
        }

        #endregion
    }
}