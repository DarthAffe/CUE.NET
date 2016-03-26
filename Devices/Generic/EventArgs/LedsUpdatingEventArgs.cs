// ReSharper disable MemberCanBePrivate.Global

using System.Collections.Generic;

namespace CUE.NET.Devices.Generic.EventArgs
{
    public class LedsUpdatingEventArgs : System.EventArgs
    {
        #region Properties & Fields

        public ICollection<KeyValuePair<int, CorsairLed>> UpdatingLeds { get; private set; }

        #endregion

        #region Constructors

        public LedsUpdatingEventArgs(ICollection<KeyValuePair<int, CorsairLed>> updatingLeds)
        {
            this.UpdatingLeds = updatingLeds; // No copy here - before the update changing this is ok.
        }

        #endregion
    }
}
