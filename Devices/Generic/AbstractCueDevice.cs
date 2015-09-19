using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CUE.NET.Native;

namespace CUE.NET.Devices.Generic
{
    public abstract class AbstractCueDevice : ICueDevice
    {
        #region Properties & Fields

        public IDeviceInfo DeviceInfo { get; }

        private Dictionary<int, CorsairLed> Leds { get; } = new Dictionary<int, CorsairLed>();

        #endregion

        #region Constructors

        protected AbstractCueDevice(IDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        protected CorsairLed GetLed(int ledId)
        {
            if (!Leds.ContainsKey(ledId))
                Leds.Add(ledId, new CorsairLed());

            return Leds[ledId];
        }

        public virtual void UpdateLeds(bool fullUpdate = false)
        {
            IList<KeyValuePair<int, CorsairLed>> ledsToUpdate = (fullUpdate ? Leds : Leds.Where(x => x.Value.IsDirty)).ToList();

            if (!ledsToUpdate.Any())
                return; // CUE seems to crash if 'CorsairSetLedsColors' is called with a zero length array

            int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
            IntPtr ptr = Marshal.AllocHGlobal(structSize * ledsToUpdate.Count);
            IntPtr addPtr = new IntPtr(ptr.ToInt64());
            foreach (KeyValuePair<int, CorsairLed> led in ledsToUpdate)
            {
                _CorsairLedColor color = new _CorsairLedColor
                {
                    ledId = led.Key,
                    r = led.Value.Color.R,
                    g = led.Value.Color.G,
                    b = led.Value.Color.B
                };

                Marshal.StructureToPtr(color, addPtr, false);
                addPtr = new IntPtr(addPtr.ToInt64() + structSize);
            }
            _CUESDK.CorsairSetLedsColors(ledsToUpdate.Count, ptr);
            Marshal.FreeHGlobal(ptr);
        }

        #endregion
    }
}
