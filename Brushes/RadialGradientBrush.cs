// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Drawing;
using CUE.NET.Gradients;
using CUE.NET.Helper;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing a radial gradient around a center point.
    /// </summary>
    public class RadialGradientBrush : AbstractBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the center point (as percentage in the range [0..1]) around which the brush should be drawn.
        /// </summary>
        public PointF Center { get; set; } = new PointF(0.5f, 0.5f);

        /// <summary>
        /// Gets or sets the gradient drawn by the brush. If null it will default to full transparent.
        /// </summary>
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        public RadialGradientBrush()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public RadialGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        /// <param name="center">The center point (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public RadialGradientBrush(PointF center, IGradient gradient)
        {
            this.Center = center;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="point">The point from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            if(Gradient == null) return Color.Transparent;

            PointF centerPoint = new PointF(rectangle.X + rectangle.Width * Center.X, rectangle.Y + rectangle.Height * Center.Y);

            // Calculate the distance to the farthest point from the center as reference (this has to be a corner)
            // ReSharper disable once RedundantCast - never trust this ...
            float refDistance = (float)Math.Max(Math.Max(Math.Max(GradientHelper.CalculateDistance(rectangle.Location, centerPoint),
                GradientHelper.CalculateDistance(new PointF(rectangle.X + rectangle.Width, rectangle.Y), centerPoint)),
                GradientHelper.CalculateDistance(new PointF(rectangle.X, rectangle.Y + rectangle.Height), centerPoint)),
                GradientHelper.CalculateDistance(new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), centerPoint));

            float distance = GradientHelper.CalculateDistance(point, centerPoint);
            float offset = distance / refDistance;
            return FinalizeColor(Gradient.GetColor(offset));
        }

        #endregion
    }
}
