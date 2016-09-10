// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Exceptions;

namespace CUE.NET.Devices.Mouse
{
    /// <summary>
    /// Represents the SDK for a corsair mouse.
    /// </summary>
    public class CorsairMouse : AbstractCueDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets specific information provided by CUE for the mouse.
        /// </summary>
        public CorsairMouseDeviceInfo MouseDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairMouse"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mouse</param>
        internal CorsairMouse(CorsairMouseDeviceInfo info)
            : base(info)
        {
            this.MouseDeviceInfo = info;
        }

        #endregion

        #region Methods

        protected override void InitializeLeds()
        {
            switch (MouseDeviceInfo.PhysicalLayout)
            {
                case CorsairPhysicalMouseLayout.Zones1:
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    InitializeLed((CorsairLedId)CorsairMouseLedId.B4, new RectangleF(3, 0, 1, 1));
                    break;
                default:
                    throw new WrapperException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }
        }

        #endregion
    }
}
