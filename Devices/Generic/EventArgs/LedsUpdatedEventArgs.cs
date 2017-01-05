// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;

namespace CUE.NET.Devices.Generic.EventArgs
{
    /// <summary>
    /// Represents the information supplied with an <see cref="ICueDevice.LedsUpdated"/>-event.
    /// </summary>
    public class LedsUpdatedEventArgs : System.EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of <see cref="LedUpateRequest"/> from the updated leds.
        /// </summary>
        public IEnumerable<LedUpateRequest> UpdatedLeds { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LedsUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="updatedLeds">The updated leds.</param>
        public LedsUpdatedEventArgs(IEnumerable<LedUpateRequest> updatedLeds)
        {
            this.UpdatedLeds = updatedLeds;
        }

        #endregion
    }
}
