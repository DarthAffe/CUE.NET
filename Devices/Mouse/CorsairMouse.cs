// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Exceptions;

namespace CUE.NET.Devices.Mouse
{
    public class CorsairMouse : AbstractCueDevice, IEnumerable<CorsairLed>
    {
        #region Properties & Fields

        #region Indexer

        public CorsairLed this[CorsairMouseLedId ledId]
        {
            get
            {
                CorsairLed led;
                return base.Leds.TryGetValue((int)ledId, out led) ? led : null;
            }
        }

        #endregion

        public CorsairMouseDeviceInfo MouseDeviceInfo { get; }

        protected override bool HasEffect => false;

        public new IEnumerable<CorsairLed> Leds => new ReadOnlyCollection<CorsairLed>(base.Leds.Values.ToList());

        #endregion

        #region Constructors

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
                    GetLed((int)CorsairMouseLedId.B1);
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    GetLed((int)CorsairMouseLedId.B1);
                    GetLed((int)CorsairMouseLedId.B2);
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    GetLed((int)CorsairMouseLedId.B1);
                    GetLed((int)CorsairMouseLedId.B2);
                    GetLed((int)CorsairMouseLedId.B3);
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    GetLed((int)CorsairMouseLedId.B1);
                    GetLed((int)CorsairMouseLedId.B2);
                    GetLed((int)CorsairMouseLedId.B3);
                    GetLed((int)CorsairMouseLedId.B4);
                    break;
                default:
                    throw new WrapperException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }
        }

        #region IEnumerable

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
