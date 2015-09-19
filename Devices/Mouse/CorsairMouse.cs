using CUE.NET.Devices.Generic;

namespace CUE.NET.Devices.Mouse
{
    //TODO DarthAffe 18.09.2015: Implement
    public class CorsairMouse : AbstractCueDevice
    {
        #region Properties & Fields

        public CorsairMouseDeviceInfo MouseDeviceInfo { get; }

        #endregion

        #region Constructors

        internal CorsairMouse(CorsairMouseDeviceInfo info)
            : base(info)
        {
            this.MouseDeviceInfo = info;
        }

        #endregion

        #region Methods

        #endregion
    }
}
