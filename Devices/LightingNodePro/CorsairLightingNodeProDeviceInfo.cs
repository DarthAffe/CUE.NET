// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Native;

namespace CUE.NET.Devices.LightingNodePro
{
    /// <summary>
    /// Represents specific information for a CUE headset stand.
    /// </summary>
    public class CorsairLightingNodeProDeviceInfo : GenericDeviceInfo
    {
        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairLightingNodeProDeviceInfo" />.
        /// </summary>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairLightingNodeProDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        { }

        #endregion
    }
}