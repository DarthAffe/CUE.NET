namespace CUE.NET.Devices
{
    public interface ICueDevice
    {
        IDeviceInfo DeviceInfo { get; }

        void UpdateLeds(bool forceUpdate = false);
    }
}
