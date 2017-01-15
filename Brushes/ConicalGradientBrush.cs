// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Gradients;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing a conical gradient.
    /// </summary>
    public class ConicalGradientBrush : AbstractBrush, IGradientBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the origin (radian-angle) the brush is drawn to. (default: -π/2)
        /// </summary>
        public float Origin { get; set; } = (float)Math.Atan2(-1, 0);

        /// <summary>
        /// Gets or sets the center point (as percentage in the range [0..1]) of the gradient drawn by the brush. (default: 0.5f, 0.5f)
        /// </summary>
        public PointF Center { get; set; } = new PointF(0.5f, 0.5f);

        /// <summary>
        /// Gets or sets the gradient drawn by the brush. If null it will default to full transparent.
        /// </summary>
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConicalGradientBrush"/> class.
        /// </summary>
        public ConicalGradientBrush()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConicalGradientBrush"/> class.
        /// </summary>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public ConicalGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConicalGradientBrush"/> class.
        /// </summary>
        /// <param name="center">The center point (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public ConicalGradientBrush(PointF center, IGradient gradient)
        {
            this.Center = center;
            this.Gradient = gradient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConicalGradientBrush"/> class.
        /// </summary>
        /// <param name="center">The center point (as percentage in the range [0..1]).</param>
        /// <param name="origin">The origin (radian-angle) the brush is drawn to.</param>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public ConicalGradientBrush(PointF center, float origin, IGradient gradient)
        {
            this.Center = center;
            this.Origin = origin;
            this.Gradient = gradient;
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
            float centerX = rectangle.Width * Center.X;
            float centerY = rectangle.Height * Center.Y;

            double angle = Math.Atan2(renderTarget.Point.Y - centerY, renderTarget.Point.X - centerX) - Origin;
            if (angle < 0) angle += Math.PI * 2;
            float offset = (float)(angle / (Math.PI * 2));

            return Gradient.GetColor(offset);
        }

        #endregion
    }
}
