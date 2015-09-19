using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Devices
{
    public interface IDeviceInfo
    {
        /// <summary>
        /// Device type
        /// </summary>
        CorsairDeviceType Type { get; }
        
        /// <summary>
        /// Device model (like “K95RGB”).
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Flags that describes device capabilities
        /// </summary>
        CorsairDeviceCaps CapsMask { get; }
    }
}
