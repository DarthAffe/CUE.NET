// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Exceptions;

namespace CUE.NET.Devices.Mouse
{
    /// <summary>
    /// Represents the SDK for a corsair mouse.
    /// </summary>
    public class CorsairMouse : AbstractCueDevice, IEnumerable<CorsairLed>
    {
        #region Properties & Fields

        #region Indexer

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> with the specified ID.
        /// </summary>
        /// <param name="ledId">The ID of the LED to get.</param>
        /// <returns>The LED with the specified ID.</returns>
        public CorsairLed this[CorsairMouseLedId ledId]
        {
            get
            {
                CorsairLed led;
                return base.Leds.TryGetValue((int)ledId, out led) ? led : null;
            }
        }

        #endregion

        /// <summary>
        /// Gets specific information provided by CUE for the mouse.
        /// </summary>
        public CorsairMouseDeviceInfo MouseDeviceInfo { get; }

        /// <summary>
        /// Gets a read-only collection containing all LEDs of the mouse.
        /// </summary>
        public new IEnumerable<CorsairLed> Leds => new ReadOnlyCollection<CorsairLed>(base.Leds.Values.ToList());

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

            InitializeLeds();
        }

        #endregion

        #region Methods

        private void InitializeLeds()
        {
            switch (MouseDeviceInfo.PhysicalLayout)
            {
                case CorsairPhysicalMouseLayout.Zones1:
                    InitializeLed((int)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    InitializeLed((int)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed((int)CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    InitializeLed((int)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed((int)CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed((int)CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    InitializeLed((int)CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed((int)CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed((int)CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    InitializeLed((int)CorsairMouseLedId.B4, new RectangleF(3, 0, 1, 1));
                    break;
                default:
                    throw new WrapperException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }
        }

        protected override void DeviceUpdate()
        {
            //DarthAffe 21.08.2016: Mice can't own brushes or groups - nothing to do here for now
        }

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates over all LEDs of the mouse.
        /// </summary>
        /// <returns>An enumerator for all LDS of the mouse.</returns>
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
