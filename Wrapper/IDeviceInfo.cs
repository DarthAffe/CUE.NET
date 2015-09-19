using CUE.NET.Enums;

namespace CUE.NET.Wrapper
{
    public interface IDeviceInfo
    {
        CorsairDeviceType Type { get; }

        //TODO DarthAffe 17.09.2015: This could be an Enum
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
