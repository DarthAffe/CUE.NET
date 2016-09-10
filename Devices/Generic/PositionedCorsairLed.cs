// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;

namespace CUE.NET.Devices.Generic
{
    public class PositionedCorsairLed : CorsairLed
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a rectangle representing the physical location of the led.
        /// </summary>
        public RectangleF LedRectangle { get; }

        #endregion

        #region Constructors

        internal PositionedCorsairLed(RectangleF ledRectangle)
        {
            this.LedRectangle = ledRectangle;
        }

        #endregion
    }
}
