// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using System.Drawing;
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
            if (offset < min)
                return min;

            return offset;
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
