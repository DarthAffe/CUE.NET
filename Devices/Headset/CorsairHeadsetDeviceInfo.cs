using CUE.NET.Devices.Generic;
using CUE.NET.Native;

namespace CUE.NET.Devices.Headset
{
    /// <summary>
    /// Stub for planned headset-support.
    /// </summary>
    public class CorsairHeadsetDeviceInfo : GenericDeviceInfo
    {
        internal CorsairHeadsetDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        { }
    }
}
