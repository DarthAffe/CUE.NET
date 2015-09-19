using System;
using System.Collections.Generic;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;

namespace CUE.NET.Devices.Keyboard
{
    public class CorsairKeyboard : AbstractCueDevice
    {
        #region Properties & Fields

        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();
        public CorsairKey this[CorsairKeyboardKeyId keyId]
        {
            get
            {
                CorsairKey key;
                return _keys.TryGetValue(keyId, out key) ? key : null;
            }
            private set { throw new NotSupportedException(); }
        }

        #endregion

        #region Constructors

        internal CorsairKeyboard(CorsairKeyboardDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;

            InitializeKeys();
        }

        #endregion

        #region Methods

        private void InitializeKeys()
        {
            foreach (CorsairKeyboardKeyId keyId in Enum.GetValues(typeof(CorsairKeyboardKeyId)))
                _keys.Add(keyId, new CorsairKey(keyId, GetLed((int)keyId)));
        }

        #endregion
    }
}
