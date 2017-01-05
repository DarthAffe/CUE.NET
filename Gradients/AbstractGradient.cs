// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using System.Linq;
using CUE.NET.Devices.Generic;

namespace CUE.NET.Gradients
{
    /// <summary>
    /// Represents a basic gradient.
    /// </summary>
    public abstract class AbstractGradient : IGradient
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of the stops used by this gradient.
        /// </summary>
        public IList<GradientStop> GradientStops { get; } = new List<GradientStop>();

        /// <summary>
        /// Gets or sets if the Gradient wraps around if there isn't a second stop to take.
        /// Example: There is a stop at offset 0f, 0.5f and 0.75f. 
        /// Without wrapping offset 1f will be calculated the same as 0.75f. With wrapping it would be the same as 0f.
        /// </summary>
        public bool WrapGradient { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
        /// </summary>
        protected AbstractGradient()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
        /// </summary>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        protected AbstractGradient(params GradientStop[] gradientStops)
        {
            foreach (GradientStop gradientStop in gradientStops)
                GradientStops.Add(gradientStop);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
        /// </summary>
        /// <param name="wrapGradient">Specifies whether the gradient should wrapp or not (see <see cref="WrapGradient"/> for an example of what this means).</param>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        protected AbstractGradient(bool wrapGradient, params GradientStop[] gradientStops)
        {
            this.WrapGradient = wrapGradient;

            foreach (GradientStop gradientStop in gradientStops)
                GradientStops.Add(gradientStop);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clips the offset and ensures, that it is inside the bounds of the stop list.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected float ClipOffset(float offset)
        {
            float max = GradientStops.Max(n => n.Offset);
            if (offset > max)
                return max;

            float min = GradientStops.Min(n => n.Offset);
            return offset < min ? min : offset;
        }

        /// <summary>
        /// Gets the color of the gradient on the specified offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the color from.</param>
        /// <returns>The color at the specific offset.</returns>
        public abstract CorsairColor GetColor(float offset);

        #endregion
    }
}
