// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Runtime.InteropServices;
using CUE.NET.Enums;
using CUE.NET.Native;

namespace CUE.NET.Wrapper
{
    public class CorsairDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Device type.
        /// </summary>
        public CorsairDeviceType Type { get; private set; }

        //TODO DarthAffe 17.09.2015: This could be an Enum
        /// <summary>
        /// Device model (like “K95RGB”).
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// Physical layout of the keyboard or mouse.
        /// </summary>
        public CorsairPhysicalLayout PhysicalLayout { get; private set; }

        //TODO DarthAffe 17.09.2015: Would device-specific infos be useful?
        /// <summary>
        /// Logical layout of the keyboard as set in CUE settings.
        /// </summary>
        public CorsairLogicalLayout LogicalLayout { get; private set; }

        /// <summary>
        /// Mask that describes device capabilities, formed as logical "or" of CorsairDeviceCaps enum values
        /// </summary>
        public CorsairDeviceCaps CapsMask { get; private set; }

        #endregion

        #region Constructors

        public CorsairDeviceInfo(_CorsairDeviceInfo nativeInfo)
        {
            this.Type = nativeInfo.type;
            this.Model = nativeInfo.model == IntPtr.Zero ? null : Marshal.PtrToStringAuto(nativeInfo.model);
            this.PhysicalLayout = nativeInfo.physicalLayout;
            this.LogicalLayout = nativeInfo.logicalLayout;
            this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;
        }

        #endregion
    }
}
