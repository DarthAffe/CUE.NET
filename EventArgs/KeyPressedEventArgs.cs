// ReSharper disable MemberCanBePrivate.Global

using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.EventArgs
{
    /// <summary>
    ///  Represents the data provided by the <see cref="CueSDK.KeyPressed"/>-event.
    /// </summary>
    public class KeyPressedEventArgs : System.EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the id of the key.
        /// </summary>
        public CorsairKeyId KeyId { get; }

        /// <summary>
        /// Gets the current status of the key (true = pressed, flase = released).
        /// </summary>
        public bool IsPressed { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressedEventArgs"/> class.
        /// </summary>
        /// <param name="keyId">The id of the key.</param>
        /// <param name="isPressed">The current status of the key (true = pressed, flase = released).</param>
        public KeyPressedEventArgs(CorsairKeyId keyId, bool isPressed)
        {
            this.KeyId = keyId;
            this.IsPressed = isPressed;
        }

        #endregion
    }
}
