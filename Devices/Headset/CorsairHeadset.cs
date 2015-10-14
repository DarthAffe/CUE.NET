using CUE.NET.Devices.Generic;

namespace CUE.NET.Devices.Headset
{
    //TODO DarthAffe 20.09.2015: Find someone to test this
    /// <summary>
    /// Stub for planned headset-support.
    /// </summary>
    public class CorsairHeadset : AbstractCueDevice
    {
        #region Properties & Fields

        protected override bool HasEffect => false;

        #endregion

        #region Constructors

        internal CorsairHeadset(CorsairHeadsetDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        #endregion
    }
}
