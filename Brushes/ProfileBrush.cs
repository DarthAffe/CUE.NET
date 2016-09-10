using System.Collections.Generic;
using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing the lighting of a CUE profile.
    /// </summary>
    public class ProfileBrush : AbstractBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the strongly-typed target used for the effect.
        /// </summary>
        protected override IBrush EffectTarget => this;

        private Dictionary<CorsairLedId, Color> _colors;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileBrush"/> class.
        /// </summary>
        /// <param name="keyLights">The light settings of the CUE profile.</param>
        internal ProfileBrush(Dictionary<CorsairLedId, Color> keyLights)
        {
            this._colors = keyLights;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        protected override Color GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
        {
            CorsairLed led = CueSDK.KeyboardSDK[renderTarget.LedId];
            if (led == null) return Color.Transparent;

            Color color;
            return !_colors.TryGetValue(led.Id, out color) ? Color.Transparent : color;
        }

        #endregion
    }
}
