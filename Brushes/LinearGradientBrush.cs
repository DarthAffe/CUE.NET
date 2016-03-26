// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System.Drawing;
using CUE.NET.Gradients;
using CUE.NET.Helper;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing a linear gradient.
    /// </summary>
    public class LinearGradientBrush : AbstractBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the start point (as percentage in the range [0..1]) of the gradient drawn by the brush. (default: 0f, 0.5f)
        /// </summary>
        public PointF StartPoint { get; set; } = new PointF(0f, 0.5f);

        /// <summary>
        /// Gets or sets the end point (as percentage in the range [0..1]) of the gradient drawn by the brush. (default: 1f, 0.5f)
        /// </summary>
        public PointF EndPoint { get; set; } = new PointF(1f, 0.5f);

        /// <summary>
        /// Gets or sets the gradient drawn by the brush. If null it will default to full transparent.
        /// </summary>
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        public LinearGradientBrush()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public LinearGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        /// <param name="startPoint">The start point (as percentage in the range [0..1]).</param>
        /// <param name="endPoint">The end point (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public LinearGradientBrush(PointF startPoint, PointF endPoint, IGradient gradient)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
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
            if (Gradient == null) return Color.Transparent;

            PointF startPoint = new PointF(StartPoint.X * rectangle.Width, StartPoint.Y * rectangle.Height);
            PointF endPoint = new PointF(EndPoint.X * rectangle.Width, EndPoint.Y * rectangle.Height);

            float offset = GradientHelper.CalculateLinearGradientOffset(startPoint, endPoint, point);
            return FinalizeColor(Gradient.GetColor(offset));
        }

        #endregion
    }
}
