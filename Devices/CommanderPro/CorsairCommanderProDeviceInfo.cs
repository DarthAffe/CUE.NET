// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Native;

namespace CUE.NET.Devices.CommanderPro
{
    /// <summary>
    /// Represents specific information for a CUE headset stand.
    /// </summary>
    public class CorsairCommanderProDeviceInfo : GenericDeviceInfo
    {
        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairCommanderProDeviceInfo" />.
        /// </summary>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairCommanderProDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        { }

        #endregion
    }
}