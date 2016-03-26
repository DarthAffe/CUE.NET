// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Drawing;

namespace CUE.NET.Devices.Generic
{
    public class LedUpateRequest
    {
        #region Properties & Fields

        public int LedId { get; }

        public Color Color { get; set; }

        #endregion

        #region Constructors

        public LedUpateRequest(int ledId, Color color)
        {
            this.LedId = ledId;
            this.Color = color;
        }

        #endregion
    }
}
