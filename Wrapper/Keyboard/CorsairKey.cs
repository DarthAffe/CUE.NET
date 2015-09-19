using CUE.NET.Enums.Keyboard;

namespace CUE.NET.Wrapper.Keyboard
{
    public class CorsairKey
    {
        #region Properties & Fields

        public CorsairKeyboardKeyId KeyId { get; }
        public CorsairLed Led { get; }

        #endregion

        #region Constructors

        internal CorsairKey(CorsairKeyboardKeyId keyId, CorsairLed led)
        {
            this.KeyId = keyId;
            this.Led = led;
        }

        #endregion

        #region Methods

        #endregion
    }
}
