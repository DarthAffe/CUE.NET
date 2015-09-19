// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Native;

namespace CUE.NET.Devices.Mouse
{
    public class CorsairMouseDeviceInfo : GenericDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Physical layout of the mouse.
        /// </summary>
        public CorsairPhysicalMouseLayout PhysicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed CorsairDeviceInfo.
        /// </summary>
        /// <param name="nativeInfo">The native CorsairDeviceInfo-struct</param>
        internal CorsairMouseDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        {
            this.PhysicalLayout = (CorsairPhysicalMouseLayout)nativeInfo.physicalLayout;
        }

        #endregion
    }
}
