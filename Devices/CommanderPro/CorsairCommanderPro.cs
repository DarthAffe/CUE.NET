// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Native;

namespace CUE.NET.Devices.CommanderPro
{
    /// <summary>
    /// Represents the SDK for a corsair CommanderPro stand.
    /// </summary>
    public class CorsairCommanderPro : AbstractCueDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets specific information provided by CUE for the CommanderPro stand.
        /// </summary>
        public CorsairCommanderProDeviceInfo CommanderProDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairCommanderPro"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the CommanderPro stand</param>
        internal CorsairCommanderPro(CorsairCommanderProDeviceInfo info)
            : base(info)
        {
            this.CommanderProDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the CommanderPro stand.
        /// </summary>
        public override void Initialize()
        {
            int deviceCount = _CUESDK.CorsairGetDeviceCount();

            // Get CommanderPro stand device index
            int CommanderProIndex = -1;
            for (int i = 0; i < deviceCount; i++)
            {
                _CorsairDeviceInfo nativeDeviceInfo = (_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo));
                GenericDeviceInfo info = new GenericDeviceInfo(nativeDeviceInfo);
                if (info.Type != CorsairDeviceType.CommanderPro)
                    continue;

                CommanderProIndex = i;
                break;
            }
            if (CommanderProIndex < 0)
                throw new WrapperException("Can't determine Commander Pro device index");

            _CorsairLedPositions nativeLedPositions = (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(CommanderProIndex), typeof(_CorsairLedPositions));
            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;

            // Put the positions in an array for sorting later on
            List<_CorsairLedPosition> positions = new List<_CorsairLedPosition>();
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = (_CorsairLedPosition)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
                ptr = new IntPtr(ptr.ToInt64() + structSize);
                positions.Add(ledPosition);
            }

            // Sort for easy iteration by clients
            foreach (_CorsairLedPosition ledPosition in positions.OrderBy(p => p.ledId))
                InitializeLed(ledPosition.ledId, new RectangleF((float)ledPosition.left, (float)ledPosition.top, (float)ledPosition.width, (float)ledPosition.height));

            base.Initialize();
        }

        #endregion
    }
}
