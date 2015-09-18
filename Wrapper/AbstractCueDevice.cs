namespace CUE.NET.Wrapper
{
    public abstract class AbstractCueDevice : ICueDevice
    {
        #region Properties & Fields

        public CorsairDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        protected AbstractCueDevice(CorsairDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        #endregion
    }
}
