// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a single target of a brush render.
    /// </summary>
    public class BrushRenderTarget
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the id of the target-key.
        /// </summary>
        public CorsairKeyboardKeyId Key { get; }

        /// <summary>
        /// Gets the point representing the position to render the target-key.
        /// </summary>
        public PointF Point { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushRenderTarget"/> class.
        /// </summary>
        /// <param name="key">The id of the target-key.</param>
        /// <param name="point">The point representing the position to render the target-key.</param>
        public BrushRenderTarget(CorsairKeyboardKeyId key, PointF point)
        {
            this.Point = point;
            this.Key = key;
        }

        #endregion
    }
}
