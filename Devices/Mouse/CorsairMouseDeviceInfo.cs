// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Native;

namespace CUE.NET.Devices.Mouse
{
    /// <summary>
    /// Represents specific information for a CUE mouse.
    /// </summary>
    public class CorsairMouseDeviceInfo : GenericDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the mouse.
        /// </summary>
        public CorsairPhysicalMouseLayout PhysicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairMouseDeviceInfo" />.
        /// </summary>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairMouseDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        {
            this.PhysicalLayout = (CorsairPhysicalMouseLayout)nativeInfo.physicalLayout;
        }

        #endregion
    }
}
