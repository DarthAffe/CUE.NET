namespace CUE.NET.Wrapper
{
    public interface ICueDevice
    {
        IDeviceInfo DeviceInfo { get; }

        void UpdateLeds(bool fullUpdate = false);
    }
}
