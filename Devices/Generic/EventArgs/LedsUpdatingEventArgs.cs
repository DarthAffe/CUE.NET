// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;

namespace CUE.NET.Devices.Generic.EventArgs
{
    /// <summary>
    /// Represents the information supplied with an <see cref="ICueDevice.LedsUpdating"/>-event.
    /// </summary>
    public class LedsUpdatingEventArgs : System.EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets a list of <see cref="LedUpateRequest"/> from the updating leds.
        /// </summary>
        public ICollection<LedUpateRequest> UpdatingLeds { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LedsUpdatingEventArgs"/> class.
        /// </summary>
        /// <param name="updatingLeds">The updating leds.</param>
        public LedsUpdatingEventArgs(ICollection<LedUpateRequest> updatingLeds)
        {
            this.UpdatingLeds = updatingLeds;
        }

        #endregion
    }
}
