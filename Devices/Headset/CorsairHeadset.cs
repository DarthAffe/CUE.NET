using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Headset.Enums;

namespace CUE.NET.Devices.Headset
{
    public class CorsairHeadset : AbstractCueDevice, IEnumerable<CorsairLed>
    {
        #region Properties & Fields

        #region Indexer

        public CorsairLed this[CorsairHeadsetLedId ledId]
        {
            get
            {
                CorsairLed led;
                return base.Leds.TryGetValue((int)ledId, out led) ? led : null;
            }
        }

        #endregion

        public CorsairHeadsetDeviceInfo HeadsetDeviceInfo { get; }

        protected override bool HasEffect => false;

        public new IEnumerable<CorsairLed> Leds => new ReadOnlyCollection<CorsairLed>(base.Leds.Values.ToList());

        #endregion

        #region Constructors

        internal CorsairHeadset(CorsairHeadsetDeviceInfo info)
            : base(info)
        {
            this.HeadsetDeviceInfo = info;
            InitializeLeds();
        }

        #endregion

        #region Methods

        private void InitializeLeds()
        {
            GetLed((int)CorsairHeadsetLedId.LeftLogo);
            GetLed((int)CorsairHeadsetLedId.RightLogo);
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
