// ReSharper disable MemberCanBePrivate.Global

using System.Collections.Generic;

namespace CUE.NET.Devices.Generic.EventArgs
{
    public class LedsUpdatedEventArgs : System.EventArgs
    {
        #region Properties & Fields

        public IEnumerable<KeyValuePair<int, CorsairLed>> UpdatedLeds { get; private set; }

        #endregion

        #region Constructors

        public LedsUpdatedEventArgs(IEnumerable<KeyValuePair<int, CorsairLed>> updatedLeds)
        {
            this.UpdatedLeds = new List<KeyValuePair<int, CorsairLed>>(updatedLeds); // Copy this - we don't want anyone to change the original led list.
        }

        #endregion
    }
}
