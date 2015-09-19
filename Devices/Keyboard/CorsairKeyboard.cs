using System;
using System.Collections.Generic;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Devices.Keyboard
{
    public class CorsairKeyboard : AbstractCueDevice
    {
        #region Properties & Fields

        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();
        public CorsairKey this[CorsairKeyboardKeyId keyId]
        {
            get { return this._keys[keyId]; }
            private set { throw new NotSupportedException(); }
        }

        #endregion

        #region Constructors

        internal CorsairKeyboard(CorsairKeyboardDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;

            this.InitializeKeys();
        }

        #endregion

        #region Methods

        private void InitializeKeys()
        {
            foreach (CorsairKeyboardKeyId keyId in Enum.GetValues(typeof(CorsairKeyboardKeyId)))
                this._keys.Add(keyId, new CorsairKey(keyId, this.GetLed((int)keyId)));
        }

        #endregion
    }
}
