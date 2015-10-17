using CUE.NET.Devices.Generic;
using CUE.NET.Native;

namespace CUE.NET.Devices.Headset
{
    /// <summary>
    /// Represents specific information for a CUE headset.
    /// </summary>
    public class CorsairHeadsetDeviceInfo : GenericDeviceInfo
    {
        /// <summary>
        /// Internal constructor of managed <see cref="CorsairHeadsetDeviceInfo" />.
        /// </summary>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairHeadsetDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        { }
    }
}
