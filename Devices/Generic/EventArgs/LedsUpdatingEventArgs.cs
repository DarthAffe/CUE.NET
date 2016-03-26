// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;

namespace CUE.NET.Devices.Generic.EventArgs
{
    public class LedsUpdatingEventArgs : System.EventArgs
    {
        #region Properties & Fields

        public ICollection<LedUpateRequest> UpdatingLeds { get; }

        #endregion

        #region Constructors

        public LedsUpdatingEventArgs(ICollection<LedUpateRequest> updatingLeds)
        {
            this.UpdatingLeds = updatingLeds;
        }

        #endregion
    }
}
