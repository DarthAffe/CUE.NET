// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Headset.Enums;
using CUE.NET.Effects;

namespace CUE.NET.Devices.Headset
{
    /// <summary>
    /// Represents the SDK for a corsair headset.
    /// </summary>
    public class CorsairHeadset : AbstractCueDevice, IEnumerable<CorsairLed>
    {
        #region Properties & Fields

        #region Indexer

        /// <summary>
        /// Gets the <see cref="CorsairLed" /> with the specified ID.
        /// </summary>
        /// <param name="ledId">The ID of the LED to get.</param>
        /// <returns>The LED with the specified ID.</returns>
        public CorsairLed this[CorsairHeadsetLedId ledId]
        {
            get
            {
                CorsairLed led;
                return base.Leds.TryGetValue((int)ledId, out led) ? led : null;
            }
        }

        #endregion

        /// <summary>
        /// Gets specific information provided by CUE for the headset.
        /// </summary>
        public CorsairHeadsetDeviceInfo HeadsetDeviceInfo { get; }

        /// <summary>
        /// Gets a value indicating if the headset has an active effect to deal with or not.
        /// </summary>
        protected override bool HasEffect => false;

        /// <summary>
        /// Gets a read-only collection containing all LEDs of the headset.
        /// </summary>
        public new IEnumerable<CorsairLed> Leds => new ReadOnlyCollection<CorsairLed>(base.Leds.Values.ToList());

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairHeadset"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the headset</param>
        internal CorsairHeadset(CorsairHeadsetDeviceInfo info)
            : base(info)
        {
            this.HeadsetDeviceInfo = info;
            InitializeLeds();
        }

        #endregion

        #region Methods

        protected override void ApplyEffect(IEffect effect)
        {
            //TODO DarthAffe 18.10.2015: How should brushes be applied to headsets?
            foreach (CorsairLed led in effect.LedList)
                led.Color = effect.EffectBrush.GetColorAtPoint(new RectangleF(0, 0, 2, 2), new PointF(1, 1));
        }

        private void InitializeLeds()
        {
            GetLed((int)CorsairHeadsetLedId.LeftLogo);
            GetLed((int)CorsairHeadsetLedId.RightLogo);
        }

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates over all LEDs of the headset.
        /// </summary>
        /// <returns>An enumerator for all LDS of the headset.</returns>
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
