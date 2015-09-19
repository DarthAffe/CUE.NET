using System.Drawing;
using CUE.NET.Enums;
using CUE.NET.Native;

namespace CUE.NET.Wrapper
{
    public class CueKeyboard : AbstractCueDevice
    {
        #region Properties & Fields

        #endregion

        #region Constructors

        internal CueKeyboard(CorsairDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        public void SetKeyColor(char key, Color color)
        {
            CorsairLedId id = _CUESDK.CorsairGetLedIdForKeyName(key);
            _CorsairLedColor ledColor = new _CorsairLedColor { ledId = id, r = color.R, g = color.G, b = color.B };
            SetKeyColors(ledColor);
        }

        public void SetKeyColors(char[] keys, Color color)
        {
            _CorsairLedColor[] ledColors = new _CorsairLedColor[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                CorsairLedId id = _CUESDK.CorsairGetLedIdForKeyName(keys[i]);
                ledColors[i] = new _CorsairLedColor { ledId = id, r = color.R, g = color.G, b = color.B };
            }
            SetKeyColors(ledColors);
        }

        #endregion
    }
}
