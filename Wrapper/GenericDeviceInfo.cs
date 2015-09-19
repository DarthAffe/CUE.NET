using System;
using System.Runtime.InteropServices;
using CUE.NET.Enums;
using CUE.NET.Native;

namespace CUE.NET.Wrapper
{
    public class GenericDeviceInfo : IDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Device type.
        /// </summary>
        public CorsairDeviceType Type { get; }

        //TODO DarthAffe 17.09.2015: This could be an Enum
        /// <summary>
        /// Device model (like “K95RGB”).
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Flags that describes device capabilities
        /// </summary>
        public CorsairDeviceCaps CapsMask { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed CorsairDeviceInfo.
        /// </summary>
        /// <param name="nativeInfo">The native CorsairDeviceInfo-struct</param>
        internal GenericDeviceInfo(_CorsairDeviceInfo nativeInfo)
        {
            this.Type = nativeInfo.type;
            this.Model = nativeInfo.model == IntPtr.Zero ? null : Marshal.PtrToStringAuto(nativeInfo.model);
            this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;
        }

        #endregion
    }
}
