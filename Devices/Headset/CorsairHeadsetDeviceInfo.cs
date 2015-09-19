using CUE.NET.Devices.Generic;
using CUE.NET.Native;

namespace CUE.NET.Devices.Headset
{
    public class CorsairHeadsetDeviceInfo : GenericDeviceInfo
    {
        internal CorsairHeadsetDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        { }
    }
}
