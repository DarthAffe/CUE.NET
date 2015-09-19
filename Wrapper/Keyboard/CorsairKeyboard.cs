using System;
using System.Collections.Generic;
using CUE.NET.Enums.Keyboard;

namespace CUE.NET.Wrapper.Keyboard
{
    public class CorsairKeyboard : AbstractCueDevice
    {
        #region Properties & Fields

        public CorsairKeyboardDeviceInfo KeyboardDeviceInfo { get; }

        private Dictionary<CorsairKeyboardKeyId, CorsairKey> _keys = new Dictionary<CorsairKeyboardKeyId, CorsairKey>();
        public CorsairKey this[CorsairKeyboardKeyId keyId]
        {
            get { return _keys[keyId]; }
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
