using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Devices
{
    public interface ICueDevice
    {
        IDeviceInfo DeviceInfo { get; }

        UpdateMode UpdateMode { get; set; }

        float UpdateFrequency { get; set; }

        void Update(bool flushLeds = false);
    }
}
