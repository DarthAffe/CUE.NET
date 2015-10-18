// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Drawing;

namespace CUE.NET.Gradients
{
    /// <summary>
    /// Represents a stop on a gradient.
    /// </summary>
    public class GradientStop
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the percentage offset to place this stop. This should be inside the range of [0..1] but it's not necessary.
        /// </summary>
        public float Offset { get; set; }

        /// <summary>
        /// Gets or sets the color of the stop.
        /// </summary>
        public Color Color { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientStop"/> class.
        /// </summary>
        /// <param name="offset">The percentage offset to place this stop.</param>
        /// <param name="color">The color of the stop.</param>
        public GradientStop(float offset, Color color)
        {
            this.Offset = offset;
            this.Color = color;
        }

        #endregion
    }
}
