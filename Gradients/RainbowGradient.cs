// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Helper;

namespace CUE.NET.Gradients
{
    /// <summary>
    /// Represents a rainbow gradient which circles through all colors of the HUE-color-space.<br />
    /// See <see cref="http://upload.wikimedia.org/wikipedia/commons/a/ad/HueScale.svg" /> as reference
    /// </summary>
    public class RainbowGradient : IGradient
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the hue (in degrees) to start from.
        /// </summary>
        public float StartHue { get; set; }

        /// <summary>
        /// Gets or sets the hue (in degrees) to end the with.
        /// </summary>
        public float EndHue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RainbowGradient"/> class.
        /// </summary>
        /// <param name="startHue">The hue (in degrees) to start from (default: 0)</param>
        /// <param name="endHue">The hue (in degrees) to end with (default: 360)</param>
        public RainbowGradient(float startHue = 0f, float endHue = 360f)
        {
            this.StartHue = startHue;
            this.EndHue = endHue;
        }

        #endregion

        #region Methods

        #endregion

        /// <summary>
        /// Gets the color on the rainbow at the given offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the color from.</param>
        /// <returns>The color at the specific offset.</returns>
        public CorsairColor GetColor(float offset)
        {
            float range = EndHue - StartHue;
            float hue = (StartHue + (range * offset)) % 360f;
            if (hue < 0)
                hue += 360;
            return ColorHelper.ColorFromHSV(hue, 1f, 1f);
        }
    }
}
