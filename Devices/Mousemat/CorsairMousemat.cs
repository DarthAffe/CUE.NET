// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Mousemat.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Native;

namespace CUE.NET.Devices.Mousemat
{
    /// <summary>
    /// Represents the SDK for a corsair mousemat.
    /// </summary>
    public class CorsairMousemat : AbstractCueDevice, IEnumerable<CorsairLed>
    {
        #region Properties & Fields

        #region Indexer

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> with the specified ID.
        /// </summary>
        /// <param name="ledId">The ID of the LED to get.</param>
        /// <returns>The LED with the specified ID.</returns>
        public CorsairLed this[CorsairMousematLedId ledId]
        {
            get
            {
                CorsairLed led;
                return base.Leds.TryGetValue((int)ledId, out led) ? led : null;
            }
        }

        #endregion

        /// <summary>
        /// Gets specific information provided by CUE for the mousemat.
        /// </summary>
        public CorsairMousematDeviceInfo MousematDeviceInfo { get; }

        /// <summary>
        /// Gets a read-only collection containing all LEDs of the mousemat.
        /// </summary>
        public new IEnumerable<CorsairLed> Leds => new ReadOnlyCollection<CorsairLed>(base.Leds.Values.ToList());

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairMousemat"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mousemat</param>
        internal CorsairMousemat(CorsairMousematDeviceInfo info)
            : base(info)
        {
            this.MousematDeviceInfo = info;
            InitializeLeds();
        }

        #endregion

        #region Methods

        private void InitializeLeds()
        {
            int deviceCount = _CUESDK.CorsairGetDeviceCount();

            // Get mousemat device index
            int mousematIndex = -1;
            for (int i = 0; i < deviceCount; i++)
            {
                _CorsairDeviceInfo nativeDeviceInfo = (_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo));
                GenericDeviceInfo info = new GenericDeviceInfo(nativeDeviceInfo);
                if (info.Type != CorsairDeviceType.Mousemat)
                    continue;

                mousematIndex = i;
                break;
            }
            if (mousematIndex < 0)
                throw new WrapperException("Can't determine mousemat device index");

            _CorsairLedPositions nativeLedPositions = (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(mousematIndex), typeof(_CorsairLedPositions));
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
                InitializeLed((int)ledPosition.ledId, new RectangleF((float)ledPosition.left, (float)ledPosition.top, (float)ledPosition.width, (float)ledPosition.height));
        }

        protected override void DeviceUpdate()
        {
            //TODO DarthAffe 10.09.2016: Implement
        }

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates over all LEDs of the mousemat.
        /// </summary>
        /// <returns>An enumerator for all LEDS of the mousemat.</returns>
        public IEnumerator<CorsairLed> GetEnumerator()
        {
            return Leds.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion
    }
}