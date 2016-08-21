using System.Collections.Generic;
using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing the lighting of a CUE profile.
    /// </summary>
    public class ProfileBrush : AbstractBrush
    {
        #region Properties & Fields

        protected override IBrush EffectTarget => this;

        private Dictionary<CorsairKeyboardKeyId, Color> _keyLights;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileBrush"/> class.
        /// </summary>
        /// <param name="keyLights">The light settings of the CUE profile.</param>
        internal ProfileBrush(Dictionary<CorsairKeyboardKeyId, Color> keyLights)
        {
            this._keyLights = keyLights;
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
            CorsairKey key = CueSDK.KeyboardSDK[renderTarget.Key];
            if (key == null) return Color.Transparent;

            Color color;
            return !_keyLights.TryGetValue(key.KeyId, out color) ? Color.Transparent : color;
        }

        #endregion
    }
}
