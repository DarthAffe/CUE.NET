// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Diagnostics;
using System.Drawing;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents a single LED of a CUE-device.
    /// </summary>
    [DebuggerDisplay("{Id} {Color}")]
    public class CorsairLed
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the Device this <see cref="CorsairLed"/> is associated with.
        /// </summary>
        public ICueDevice Device { get; }

        /// <summary>
        /// Gets the key-ID of the Led.
        /// </summary>
        public CorsairLedId Id { get; }

        /// <summary>
        /// Gets a rectangle representing the physical location of the led.
        /// </summary>
        public RectangleF LedRectangle { get; }

        /// <summary>
        /// Indicates whether the LED has changed an internal state.
        /// </summary>
        public bool IsDirty => RequestedColor != _color;

        /// <summary>
        /// Gets the Color the LED should be set to on the next update.
        /// </summary>
        public CorsairColor RequestedColor { get; private set; } = CorsairColor.Transparent;

        private CorsairColor _color = CorsairColor.Transparent;
        /// <summary>
        /// Gets the current color of the LED. Sets the <see cref="RequestedColor" /> for the next update. />.
        /// </summary>
        public CorsairColor Color
        {
            get { return _color; }
            set
            {
                if (!IsLocked)
                    RequestedColor = RequestedColor.Blend(value);
            }
        }

        /// <summary>
        /// Gets or sets if the color of this LED can be changed.
        /// </summary>
        public bool IsLocked { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairLed"/> class.
        /// </summary>
        /// <param name="device">The <see cref="ICueDevice"/> the <see cref="CorsairLed"/> is associated with.</param>
        /// <param name="id">The <see cref="CorsairLedId"/> of the <see cref="CorsairLed"/>.</param>
        /// <param name="ledRectangle">The rectangle representing the physical location of the <see cref="CorsairLed"/>.</param>
        internal CorsairLed(ICueDevice device, CorsairLedId id, RectangleF ledRectangle)
        {
            this.Device = device;
            this.Id = id;
            this.LedRectangle = ledRectangle;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id and the <see cref="Color"/> of this <see cref="CorsairLed"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id and the <see cref="Color"/> of this <see cref="CorsairLed"/>. For example "Enter [A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString()
        {
            return $"{Id} {Color}";
        }

        /// <summary>
        /// Updates the LED to the requested color.
        /// </summary>
        internal void Update()
        {
            _color = RequestedColor;
        }

        /// <summary>
        /// Resets the LED back to default
        /// </summary>
        internal void Reset()
        {
            _color = CorsairColor.Transparent;
            RequestedColor = CorsairColor.Transparent;
            IsLocked = false;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a <see cref="CorsairLed" /> to a <see cref="CorsairLedId" />.
        /// </summary>
        /// <param name="led">The <see cref="CorsairLed"/> to convert.</param>
        public static implicit operator CorsairLedId(CorsairLed led)
        {
            return led?.Id ?? CorsairLedId.Invalid;
        }

        /// <summary>
        /// Converts a <see cref="CorsairLed" /> to a <see cref="CorsairColor" />.
        /// </summary>
        /// <param name="led">The <see cref="CorsairLed"/> to convert.</param>
        public static implicit operator CorsairColor(CorsairLed led)
        {
            return led?.Color;
        }

        #endregion
    }
}
