// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Native;

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

        /// <summary>
        /// Initializes the mouse.
        /// </summary>
        public override void Initialize()
        {
            // Glaive is a special flake that doesn't follow the default layout
            if (MouseDeviceInfo.Model == "GLAIVE RGB"|| MouseDeviceInfo.Model == "GLAIVE RGB Demo")
            {
                InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1)); // Logo
                InitializeLed(CorsairMouseLedId.B2, new RectangleF(2, 0, 1, 1)); // Front
                InitializeLed(CorsairMouseLedId.B5, new RectangleF(3, 0, 1, 1)); // Sides
                return;
            }
            if (MouseDeviceInfo.Model == "HARPOON RGB")
            {
                InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1)); // Logo
                return;
            }
            switch (MouseDeviceInfo.PhysicalLayout)
            {
                case CorsairPhysicalMouseLayout.Zones1:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B4, new RectangleF(3, 0, 1, 1));
                    break;
                default:
                    throw new WrapperException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }

            base.Initialize();
        }

        #endregion
    }
}
