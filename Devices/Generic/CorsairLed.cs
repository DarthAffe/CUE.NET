// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents a single LED of a CUE-device.
    /// </summary>
    public class CorsairLed
    {
        #region Properties & Fields
        
        /// <summary>
        /// Gets the key-ID of the Led.
        /// </summary>
        public CorsairLedId Id { get; set; }

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
        public Color RequestedColor { get; private set; } = Color.Transparent;

        private Color _color = Color.Transparent;
        /// <summary>
        /// Gets the current color of the LED. Sets the <see cref="RequestedColor" /> for the next update. />.
        /// </summary>
        public Color Color
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
        public bool IsLocked { get; set; } = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairLed"/> class.
        /// </summary>
        /// <param name="id">The ID of the LED</param>
        /// <param name="ledRectangle">The rectangle representing the physical location of the LED.</param>
        internal CorsairLed(CorsairLedId id, RectangleF ledRectangle)
        {
            this.Id = id;
            this.LedRectangle = ledRectangle;
        }

        #endregion

        #region Methods

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
            _color = Color.Transparent;
            RequestedColor = Color.Transparent;
            IsLocked = false;
        }

        #endregion
    }
}
