// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System;

namespace CUE.NET.Devices.Generic.Enums
{
    /// <summary>
    /// Contains list of device capabilities
    /// </summary>
    [Flags]
    public enum CorsairDeviceCaps
    {
        /// <summary>
        /// For devices that do not support any SDK functions
        /// </summary>
        None = 0,

        /// <summary>
        /// For devices that has controlled lighting
        /// </summary>
        Lighting = 1
    };
}
