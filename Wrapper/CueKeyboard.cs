using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CUE.NET.Enums;
using CUE.NET.Native;

namespace CUE.NET.Wrapper
{
    public class CueKeyboard : AbstractCueDevice
    {
        #region Properties & Fields

        #endregion

        #region Constructors

        public CueKeyboard(CorsairDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        public void SetKeyColor(char key, Color color)
        {
            CorsairLedId id = _CUESDK.CorsairGetLedIdForKeyName(key);
            _CorsairLedColor ledColor = new _CorsairLedColor { ledId = id, r = color.R, g = color.G, b = color.B };

            //TODO DarthAffe 18.09.2015: Generalize and move to base class
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(_CorsairLedColor)));
            Marshal.StructureToPtr(ledColor, ptr, true);
            _CUESDK.CorsairSetLedsColors(1, ptr);
        }

        #endregion
    }
}
