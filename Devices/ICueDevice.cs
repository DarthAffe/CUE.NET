using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Devices
{
    public delegate void OnExceptionEventHandler(object sender, OnExceptionEventArgs args);

    public interface ICueDevice
    {

        IDeviceInfo DeviceInfo { get; }

        UpdateMode UpdateMode { get; set; }

        float UpdateFrequency { get; set; }

        event OnExceptionEventHandler OnException;

        void Update(bool flushLeds = false);
    }
}
