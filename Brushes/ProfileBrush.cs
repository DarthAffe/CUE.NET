using System.Collections.Generic;
using System.Drawing;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Brushes
{
    public class ProfileBrush : AbstractBrush
    {
        #region Properties & Fields
        
        private Dictionary<CorsairKeyboardKeyId, Color> _keyLights;

        #endregion

        #region Constructors

        internal ProfileBrush(Dictionary<CorsairKeyboardKeyId, Color> keyLights)
        {
            this._keyLights = keyLights;
        }

        #endregion

        #region Methods

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
