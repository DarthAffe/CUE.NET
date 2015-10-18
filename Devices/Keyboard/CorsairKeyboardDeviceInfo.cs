// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Native;

namespace CUE.NET.Devices.Keyboard
{
    /// <summary>
    /// Represents specific information for a CUE keyboard.
    /// </summary>
    public class CorsairKeyboardDeviceInfo : GenericDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public CorsairPhysicalKeyboardLayout PhysicalLayout { get; private set; }

        /// <summary>
        /// Gets the logical layout of the keyboard as set in CUE settings.
        /// </summary>
        public CorsairLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed CorsairDeviceInfo.
        /// </summary>
        /// <param name="nativeInfo">The native CorsairDeviceInfo-struct</param>
        internal CorsairKeyboardDeviceInfo(_CorsairDeviceInfo nativeInfo)
            : base(nativeInfo)
        {
            this.PhysicalLayout = (CorsairPhysicalKeyboardLayout)nativeInfo.physicalLayout;
            this.LogicalLayout = (CorsairLogicalKeyboardLayout)nativeInfo.logicalLayout;
        }

        #endregion
    }
}
