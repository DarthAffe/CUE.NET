// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents a request to update a led.
    /// </summary>
    public class LedUpateRequest
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the id of the led to update.
        /// </summary>
        public int LedId { get; }

        /// <summary>
        /// Gets the requested color of the led.
        /// </summary>
        public Color Color { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LedUpateRequest"/> class.
        /// </summary>
        /// <param name="ledId">The id of the led to update.</param>
        /// <param name="color">The requested color of the led.</param>
        public LedUpateRequest(int ledId, Color color)
        {
            this.LedId = ledId;
            this.Color = color;
        }

        #endregion
    }
}
