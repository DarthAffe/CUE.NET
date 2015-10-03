using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Native;

namespace CUE.NET.Devices.Generic
{
    public abstract class AbstractCueDevice : ICueDevice
    {
        private UpdateMode _updateMode = UpdateMode.AutoOnEffect;

        #region Properties & Fields

        public IDeviceInfo DeviceInfo { get; }

        public UpdateMode UpdateMode
        {
            get { return _updateMode; }
            set
            {
                _updateMode = value;
                CheckUpdateLoop();
            }
        }
        public float UpdateFrequency { get; set; } = 1f / 30f;

        private Dictionary<int, CorsairLed> Leds { get; } = new Dictionary<int, CorsairLed>();

        protected abstract bool HasEffect { get; }

        private CancellationTokenSource _updateTokenSource;
        private CancellationToken _updateToken;
        private Task _updateTask;

        #endregion

        #region Constructors

        protected AbstractCueDevice(IDeviceInfo info)
        {
            this.DeviceInfo = info;

            CheckUpdateLoop();
        }

        #endregion

        #region Methods

        protected CorsairLed GetLed(int ledId)
        {
            if (!Leds.ContainsKey(ledId))
                Leds.Add(ledId, new CorsairLed());

            return Leds[ledId];
        }

        protected async void CheckUpdateLoop()
        {
            bool shouldRun;
            switch (UpdateMode)
            {
                case UpdateMode.Manual:
                    shouldRun = false;
                    break;
                case UpdateMode.AutoOnEffect:
                    shouldRun = HasEffect;
                    break;
                case UpdateMode.Continuous:
                    shouldRun = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (shouldRun && _updateTask == null) // Start task
            {
                _updateTokenSource?.Dispose();
                _updateTokenSource = new CancellationTokenSource();
                _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token));
            }
            else if (!shouldRun && _updateTask != null) // Stop task
            {
                _updateTokenSource.Cancel();
                await _updateTask;
                _updateTask.Dispose();
                _updateTask = null;
            }
        }

        private void UpdateLoop()
        {
            while (!_updateToken.IsCancellationRequested)
            {
                long preUpdateTicks = DateTime.Now.Ticks;
                Update();
                int sleep = (int)((UpdateFrequency * 1000f) - ((DateTime.Now.Ticks - preUpdateTicks) / 10000f));
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        public virtual void Update(bool flushLeds = false)
        {
            IList<KeyValuePair<int, CorsairLed>> ledsToUpdate = (flushLeds ? Leds : Leds.Where(x => x.Value.IsDirty)).ToList();

            foreach (CorsairLed led in Leds.Values)
                led.Update();

            UpdateLeds(ledsToUpdate);
        }

        private static void UpdateLeds(ICollection<KeyValuePair<int, CorsairLed>> ledsToUpdate)
        {
            ledsToUpdate = ledsToUpdate.Where(x => x.Value.Color != Color.Transparent).ToList();

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
