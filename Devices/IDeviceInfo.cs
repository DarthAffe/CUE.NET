// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using CUE.NET.Devices.Generic.Enums;

namespace CUE.NET.Devices
{
    /// <summary>
    /// Represents generic device information.
    /// </summary>
    public interface IDeviceInfo
    {
        /// <summary>
        /// Gets the device type.
        /// </summary>
        CorsairDeviceType Type { get; }

        /// <summary>
        /// Gets the device model (like “K95RGB”).
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Gets flags, which describe device capabilities.
        /// </summary>
        CorsairDeviceCaps CapsMask { get; }
    }
}
