// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Drawing;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a single target of a brush render.
    /// </summary>
    public class BrushRenderTarget
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the ID of the target-LED.
        /// </summary>
        public CorsairLedId LedId { get; }

        /// <summary>
        /// Gets the rectangle representing the area to render the target-LED.
        /// </summary>
        public RectangleF Rectangle { get; }

        /// <summary>
        /// Gets the point representing the position to render the target-LED.
        /// </summary>
        public PointF Point { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushRenderTarget"/> class.
        /// </summary>
        /// <param name="ledId">The ID of the target-LED.</param>
        /// <param name="rectangle">The rectangle representing the area to render the target-LED.</param>
        public BrushRenderTarget(CorsairLedId ledId, RectangleF rectangle)
        {
            this.Rectangle = rectangle;
            this.LedId = ledId;

            Point = rectangle.GetCenter();
        }

        #endregion
    }
}
