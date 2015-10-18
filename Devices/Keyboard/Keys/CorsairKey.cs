// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Devices.Keyboard.Keys
{
    /// <summary>
    /// Represents a key of a corsair keyboard.
    /// </summary>
    public class CorsairKey
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the key-ID of the key.
        /// </summary>
        public CorsairKeyboardKeyId KeyId { get; }

        /// <summary>
        /// Gets the LED of the key.
        /// </summary>
        public CorsairLed Led { get; }

        /// <summary>
        /// Gets a rectangle representing the physical location of the key.
        /// </summary>
        public RectangleF KeyRectangle { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairKey"/> class.
        /// </summary>
        /// <param name="keyId">The key-ID of the key.</param>
        /// <param name="led">The LED of the key.</param>
        /// <param name="keyRectangle">The rectangle representing the physical location of the key.</param>
        internal CorsairKey(CorsairKeyboardKeyId keyId, CorsairLed led, RectangleF keyRectangle)
        {
            this.KeyId = keyId;
            this.Led = led;
            this.KeyRectangle = keyRectangle;
        }

        #endregion
    }
}
