// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using CUE.NET.Enums.Mouse;
using CUE.NET.Native;

namespace CUE.NET.Wrapper.Mouse
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
