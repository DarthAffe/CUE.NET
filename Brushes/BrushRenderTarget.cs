// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Drawing;
using CUE.NET.Devices.Generic.Enums;

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
        /// Gets the point representing the position to render the target-LED.
        /// </summary>
        public PointF Point { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushRenderTarget"/> class.
        /// </summary>
        /// <param name="ledId">The ID of the target-LED.</param>
        /// <param name="point">The point representing the position to render the target-LED.</param>
        public BrushRenderTarget(CorsairLedId ledId, PointF point)
        {
            this.Point = point;
            this.LedId = ledId;
        }

        #endregion
    }
}
