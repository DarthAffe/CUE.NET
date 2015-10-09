using CUE.NET.Devices.Generic;

namespace CUE.NET.Devices.Mouse
{
    //TODO DarthAffe 20.09.2015: Find someone to test this
    public class CorsairMouse : AbstractCueDevice
    {
        #region Properties & Fields

        public CorsairMouseDeviceInfo MouseDeviceInfo { get; }

        protected override bool HasEffect => false;

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
