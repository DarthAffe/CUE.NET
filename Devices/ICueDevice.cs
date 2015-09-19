namespace CUE.NET.Devices
{
    public interface ICueDevice
    {
        IDeviceInfo DeviceInfo { get; }

        void UpdateLeds(bool fullUpdate = false);
    }
}
