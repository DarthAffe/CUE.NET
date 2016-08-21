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
        /// Gets the color at an specific point getting the color of the key at the given point.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="point">The point from which the color should be taken.</param>
        /// <returns>The color of the key at the specified point.</returns>
        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            CorsairKey key = CueSDK.KeyboardSDK[point];
            if (key == null) return Color.Transparent;

            Color color;
            if (!_keyLights.TryGetValue(key.KeyId, out color))
                return Color.Transparent;

            return FinalizeColor(color);
        }

        #endregion
    }
}
