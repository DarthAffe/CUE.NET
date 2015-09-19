using System.Drawing;
using CUE.NET.Native;

namespace CUE.NET.Wrapper.Keyboard
{
    public class CorsairKeyboard : AbstractCueDevice
    {
        #region Properties & Fields

        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

        #endregion

        #region Constructors

        internal CorsairKeyboard(CorsairKeyboardDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;
        }

        #endregion

        #region Methods

        public void SetKeyColor(char key, Color color)
        {
            int id = _CUESDK.CorsairGetLedIdForKeyName(key);
            _CorsairLedColor ledColor = new _CorsairLedColor { ledId = id, r = color.R, g = color.G, b = color.B };
            SetKeyColors(ledColor);
        }

        public void SetKeyColors(char[] keys, Color color)
        {
            _CorsairLedColor[] ledColors = new _CorsairLedColor[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                int id = _CUESDK.CorsairGetLedIdForKeyName(keys[i]);
                ledColors[i] = new _CorsairLedColor { ledId = id, r = color.R, g = color.G, b = color.B };
            }
            SetKeyColors(ledColors);
        }

        #endregion
    }
}
