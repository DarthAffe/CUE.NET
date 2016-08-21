// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Native;

namespace CUE.NET.Devices.Generic
{
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, ...)
    /// </summary>
    public abstract class AbstractCueDevice : ICueDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets generic information provided by CUE for the device.
        /// </summary>
        public IDeviceInfo DeviceInfo { get; }

        private UpdateMode _updateMode = UpdateMode.Manual;
        /// <summary>
        /// Gets or sets the update-mode for the device.
        /// </summary>
        public UpdateMode UpdateMode
        {
            get { return _updateMode; }
            set
            {
                _updateMode = value;
                CheckUpdateLoop();
            }
        }

        /// <summary>
        /// Gets or sets the update-frequency in seconds. (Calculate by using '1f / updates per second')
        /// </summary>
        public float UpdateFrequency { get; set; } = 1f / 30f;

        /// <summary>
        /// Gets a dictionary containing all LEDs of the device.
        /// </summary>
        protected Dictionary<int, CorsairLed> Leds { get; } = new Dictionary<int, CorsairLed>();

        private CancellationTokenSource _updateTokenSource;
        private CancellationToken _updateToken;
        private Task _updateTask;
        private DateTime _lastUpdate = DateTime.Now;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a catched exception is thrown inside the device.
        /// </summary>
        public event ExceptionEventHandler Exception;

        /// <summary>
        /// Occurs when the device starts updating.
        /// </summary>
        public event UpdatingEventHandler Updating;

        /// <summary>
        /// Occurs when the device update is done.
        /// </summary>
        public event UpdatedEventHandler Updated;

        /// <summary>
        /// Occurs when the device starts to update the leds.
        /// </summary>
        public event LedsUpdatingEventHandler LedsUpdating;

        /// <summary>
        /// Occurs when the device updated the leds.
        /// </summary>
        public event LedsUpdatedEventHandler LedsUpdated;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCueDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected AbstractCueDevice(IDeviceInfo info)
        {
            this.DeviceInfo = info;

            CheckUpdateLoop();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the LED-Object with the specified id.
        /// </summary>
        /// <param name="ledId">The LED-Id to look for.</param>
        /// <returns></returns>
        protected CorsairLed GetLed(int ledId)
        {
            if (!Leds.ContainsKey(ledId))
                Leds.Add(ledId, new CorsairLed());

            return Leds[ledId];
        }

        /// <summary>
        /// Checks if automatic updates should occur and starts/stops the update-loop if needed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the requested update-mode is not available.</exception>
        protected async void CheckUpdateLoop()
        {
            bool shouldRun;
            switch (UpdateMode)
            {
                case UpdateMode.Manual:
                    shouldRun = false;
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

        /// <summary>
        /// Performs an update for all dirty keys, or all keys if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all keys (including clean ones) should be updated.</param>
        public void Update(bool flushLeds = false)
        {
            OnUpdating();
            
            DeviceUpdate();

            ICollection<LedUpateRequest> ledsToUpdate = (flushLeds ? Leds : Leds.Where(x => x.Value.IsDirty)).Select(x => new LedUpateRequest(x.Key, x.Value.RequestedColor)).ToList();

            foreach (CorsairLed led in Leds.Values)
                led.Update();

            UpdateLeds(ledsToUpdate);

            OnUpdated();
        }

        /// <summary>
        /// Performs device specific updates.
        /// </summary>
        protected abstract void DeviceUpdate();
        
        private void UpdateLeds(ICollection<LedUpateRequest> updateRequests)
        {
            updateRequests = updateRequests.Where(x => x.Color != Color.Transparent).ToList();

            OnLedsUpdating(updateRequests);

            if (updateRequests.Any()) // CUE seems to crash if 'CorsairSetLedsColors' is called with a zero length array
            {
                int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
                IntPtr ptr = Marshal.AllocHGlobal(structSize * updateRequests.Count);
                IntPtr addPtr = new IntPtr(ptr.ToInt64());
                foreach (LedUpateRequest ledUpdateRequest in updateRequests)
                {
                    _CorsairLedColor color = new _CorsairLedColor
                    {
                        ledId = ledUpdateRequest.LedId,
                        r = ledUpdateRequest.Color.R,
                        g = ledUpdateRequest.Color.G,
                        b = ledUpdateRequest.Color.B
                    };

                    Marshal.StructureToPtr(color, addPtr, false);
                    addPtr = new IntPtr(addPtr.ToInt64() + structSize);
                }
                _CUESDK.CorsairSetLedsColors(updateRequests.Count, ptr);
                Marshal.FreeHGlobal(ptr);
            }

            OnLedsUpdated(updateRequests);
        }

        /// <summary>
        /// Resets all loaded LEDs back to default.
        /// </summary>
        internal void ResetLeds()
        {
            foreach (CorsairLed led in Leds.Values)
                led.Reset();
        }

        #region EventCaller

        /// <summary>
        /// Handles the needed event-calls for an exception.
        /// </summary>
        /// <param name="ex">The exception previously thrown.</param>
        protected virtual void OnException(Exception ex)
        {
            try
            {
                Exception?.Invoke(this, new ExceptionEventArgs(ex));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before updating.
        /// </summary>
        protected virtual void OnUpdating()
        {
            try
            {
                long lastUpdateTicks = _lastUpdate.Ticks;
                _lastUpdate = DateTime.Now;
                Updating?.Invoke(this, new UpdatingEventArgs((float)((DateTime.Now.Ticks - lastUpdateTicks) / 10000000f)));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after an update.
        /// </summary>
        protected virtual void OnUpdated()
        {
            try
            {
                Updated?.Invoke(this, new UpdatedEventArgs());
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls before the leds are updated.
        /// </summary>
        protected virtual void OnLedsUpdating(ICollection<LedUpateRequest> updatingLeds)
        {
            try
            {
                LedsUpdating?.Invoke(this, new LedsUpdatingEventArgs(updatingLeds));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }

        /// <summary>
        /// Handles the needed event-calls after the leds are updated.
        /// </summary>
        protected virtual void OnLedsUpdated(IEnumerable<LedUpateRequest> updatedLeds)
        {
            try
            {
                LedsUpdated?.Invoke(this, new LedsUpdatedEventArgs(updatedLeds));
            }
            catch
            {
                // Well ... that's not my fault
            }
        }
        #endregion

        #endregion
    }
}
