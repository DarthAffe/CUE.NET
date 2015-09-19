using CUE.NET.Native;

namespace CUE.NET.Wrapper.Headset
{
    public class CorsairHeadsetDeviceInfo : GenericDeviceInfo
    {
        public CorsairHeadsetDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        { }
    }
}
