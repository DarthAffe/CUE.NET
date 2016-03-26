// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;

namespace CUE.NET.Devices.Generic.EventArgs
{
    public class LedsUpdatedEventArgs : System.EventArgs
    {
        #region Properties & Fields

        public IEnumerable<LedUpateRequest> UpdatedLeds { get; }

        #endregion

        #region Constructors

        public LedsUpdatedEventArgs(IEnumerable<LedUpateRequest> updatedLeds)
        {
            this.UpdatedLeds = updatedLeds;
        }

        #endregion
    }
}
