// ReSharper disable UnusedMember.Global

using System.Linq;
using CUE.NET.Devices.Generic;

namespace CUE.NET.Gradients
{
    /// <summary>
    /// Represents a linear interpolated gradient with n stops.
    /// </summary>
    public class LinearGradient : AbstractGradient
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradient"/> class.
        /// </summary>
        public LinearGradient()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradient"/> class.
        /// </summary>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        public LinearGradient(params GradientStop[] gradientStops)
            : base(gradientStops)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
        /// </summary>
        /// <param name="wrapGradient">Specifies whether the gradient should wrapp or not (see <see cref="AbstractGradient.WrapGradient"/> for an example of what this means).</param>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        public LinearGradient(bool wrapGradient, params GradientStop[] gradientStops)
            : base(wrapGradient, gradientStops)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the linear interpolated color at the given offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the color from.</param>
        /// <returns>The color at the specific offset.</returns>
        public override CorsairColor GetColor(float offset)
        {
            if (!GradientStops.Any()) return CorsairColor.Transparent;
            if (GradientStops.Count == 1) return GradientStops.First().Color;

            GradientStop gsBefore;
            GradientStop gsAfter;

            if (WrapGradient)
            {
                gsBefore = GradientStops.Where(n => n.Offset <= offset).OrderBy(n => n.Offset).LastOrDefault();
                if (gsBefore == null)
                {
                    GradientStop lastStop = GradientStops.OrderBy(n => n.Offset).Last();
                    gsBefore = new GradientStop(lastStop.Offset - 1f, lastStop.Color);
                }

                gsAfter = GradientStops.Where(n => n.Offset >= offset).OrderBy(n => n.Offset).FirstOrDefault();
                if (gsAfter == null)
                {
                    GradientStop firstStop = GradientStops.OrderBy(n => n.Offset).First();
                    gsAfter = new GradientStop(firstStop.Offset + 1f, firstStop.Color);
                }
            }
            else
            {
                offset = ClipOffset(offset);

                gsBefore = GradientStops.Where(n => n.Offset <= offset).OrderBy(n => n.Offset).Last();
                gsAfter = GradientStops.Where(n => n.Offset >= offset).OrderBy(n => n.Offset).First();
            }

            float blendFactor = 0f;
            if (!gsBefore.Offset.Equals(gsAfter.Offset))
                blendFactor = ((offset - gsBefore.Offset) / (gsAfter.Offset - gsBefore.Offset));

            byte colA = (byte)((gsAfter.Color.A - gsBefore.Color.A) * blendFactor + gsBefore.Color.A);
            byte colR = (byte)((gsAfter.Color.R - gsBefore.Color.R) * blendFactor + gsBefore.Color.R);
            byte colG = (byte)((gsAfter.Color.G - gsBefore.Color.G) * blendFactor + gsBefore.Color.G);
            byte colB = (byte)((gsAfter.Color.B - gsBefore.Color.B) * blendFactor + gsBefore.Color.B);

            return new CorsairColor(colA, colR, colG, colB);
        }

        #endregion
    }
}
